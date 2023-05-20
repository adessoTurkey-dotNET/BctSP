using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BctSP.Infrastructure;
using BctSP.Infrastructure.BaseModels;
using ImpromptuInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BctSP.Extensions
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddBctSp(this IServiceCollection services, Action<BctSpOptions> setupAction)
        {
            var bctSpOptions = new BctSpOptions();
            setupAction?.Invoke(bctSpOptions);

            if (string.IsNullOrWhiteSpace(bctSpOptions.BctSpConnectionStringOrConfigurationPath) || !bctSpOptions.DatabaseType.HasValue)
            {
                throw new Exception("Connection string and database type should be configured");
            }

            // if (string.IsNullOrWhiteSpace(bctSpOptions.ConnectionString))
            // {
            //     bctSpOptions.ConnectionString = bctSpOptions.DbConfiguration.Configuration
            //         .GetSection(bctSpOptions.DbConfiguration.ConnectionStringConfigurationPath)
            //         .Get<string>();
            // }

            services.AddSingleton(typeof(IOptions<BctSpOptions>), x => bctSpOptions);

            _ = ConfigureBctSpInfrastructure(services);

            return services;
        }

        private static IServiceCollection ConfigureBctSpInfrastructure(IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface && typeof(IBctSp).IsAssignableFrom(p));

            foreach (var type in types)
            {
                services.AddSingleton(type, (provider) =>
                {
                    IDictionary<string, object> obj = new ExpandoObject();

                    foreach (var method in type.GetMethods())
                    {
                        var methodName = GetMethodName(method);
                        var methodInfo = typeof(ProxyClass).GetMethod(methodName);

                        var delegateType = GetDelegateType(methodInfo);

                        var configuration = provider.GetService<IConfiguration>();
                        var del = Delegate.CreateDelegate(delegateType, new ProxyClass(provider, configuration),
                            methodInfo ?? throw new InvalidOperationException());

                        obj[method.Name] = del;
                    }


                    Debug.Assert(type != null, nameof(type) + " != null");
                    // ReSharper disable once HeapView.ObjectAllocation
                    return Impromptu.DynamicActLike(obj, type);
                });
            }

            return services;
        }

        private static string GetMethodName(MethodInfo method)
        {
            return method.ReturnType switch
            {
                { Name: "Task" } => "ProxyVoidAsync",

                { Name: "Task`1" } t when typeof(IEnumerable).IsAssignableFrom(
                    t.GetGenericArguments()[0]) => "ProxyListAsync",

                { Name: "Task`1" } => "ProxyObjectAsync",

                { } t when t == typeof(void) => "ProxyVoid",

                { } t when typeof(IEnumerable).IsAssignableFrom(t) => "ProxyList",

                _ => "ProxyObject",
            };
        }

        private static Type GetDelegateType(MethodInfo method)
        {
            return method.ReturnType switch
            {
                { Name: "Task" } => typeof(Func<,>).MakeGenericType(
                    typeof(BctSpCoreRequest<BctSpBaseResponse>), typeof(Task)),

                { Name: "Task`1" } t when t.GetGenericArguments().Any() => typeof(Func<,>).MakeGenericType(
                    typeof(BctSpCoreRequest<BctSpBaseResponse>),
                    typeof(Task<dynamic>)),

                { } t when t == typeof(void) => typeof(Action<>).MakeGenericType(
                    typeof(BctSpCoreRequest<BctSpBaseResponse>)),

                { } t when t != typeof(void) => typeof(Func<,>).MakeGenericType(
                    typeof(BctSpCoreRequest<BctSpBaseResponse>),
                    typeof(object)),

                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}