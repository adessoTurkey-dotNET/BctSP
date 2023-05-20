using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BctSP.Databases;
using BctSP.Extensions;
using BctSP.Infrastructure.BaseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BctSP.Infrastructure
{
    internal partial class ProxyClass
    {
        private readonly IServiceProvider _serviceScope;
        private readonly IConfiguration _configuration;

        public ProxyClass(IServiceProvider serviceScope, IConfiguration configuration)
        {
            _serviceScope = serviceScope;
            _configuration = configuration;
        }

        private static void MapResponseDictionaryToResponseObject(object responseModel, object databaseResponse)
        {
            var responseDictionary = (IDictionary<string, object>)databaseResponse;

            foreach (var property in responseModel.GetType().GetProperties())
            {
                var propertyMap = responseDictionary.Keys.FirstOrDefault(x =>
                    string.Equals(x, property.Name, StringComparison.CurrentCultureIgnoreCase));
                if (propertyMap is null)
                {
                    continue;
                }

                property.SetValue(responseModel, responseDictionary[propertyMap]);
            }
        }

        private static void MapResponseDictionaryToResponseObjectList(object responseModel, object databaseResponse)
        {
            var responseDictionaryList = (IEnumerable<IDictionary<string, object>>)databaseResponse;
            // ReSharper disable once PossibleMultipleEnumeration
            for (var i = 0; i < responseDictionaryList.Count(); i++)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var responseDictionary = responseDictionaryList.ElementAt(i);
                var instance = Activator.CreateInstance(responseModel.GetType().GetGenericArguments()[0]);
                foreach (var property in instance.GetType().GetProperties())
                {
                    var propertyMap = responseDictionary.Keys.FirstOrDefault(x =>
                        string.Equals(x, property.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (propertyMap is null)
                    {
                        continue;
                    }

                    property.SetValue(instance, responseDictionary[propertyMap]);
                }

                responseModel.GetType().GetMethod("Add")?.Invoke(responseModel, new[] { instance });
            }
        }

        private static object GetResponseInstance(BctSpCoreRequest<BctSpBaseResponse> request, bool isList)
        {
            if (isList)
            {
                return Activator.CreateInstance(
                    typeof(List<>).MakeGenericType(request?.GetType().BaseType?.GetGenericArguments()[0]));
            }

            return Activator.CreateInstance(request?.GetType().BaseType?.GetGenericArguments()[0] ??
                                            throw new ArgumentException());
        }

        private static ISqlBase GetSql(IServiceScope serviceScope, IDictionary<string, object> requestDictionary,
            IConfiguration configuration)
        {
            var bctSpOptions = (BctSpOptions)serviceScope.ServiceProvider.GetService(typeof(IOptions<BctSpOptions>));

            var databaseType = bctSpOptions.DatabaseType;
            var connectionString = bctSpOptions.BctSpConnectionStringOrConfigurationPath;
            if (requestDictionary.TryGetValue("BctSpDatabaseType", out var typeValue) && typeValue != null)
            {
                databaseType = (BctSpDatabaseType)typeValue;
            }

            if (requestDictionary.TryGetValue("BctSpConnectionStringOrConfigurationPath", out var connectionStringValue) &&
                connectionStringValue != null)
            {
                connectionString = connectionStringValue.ToString();
            }

            if (!connectionString.CheckIfConnectionString())
            {
                connectionString = configuration
                    .GetSection(connectionString)
                    .Get<string>();
            }

            Debug.Assert(databaseType != null, nameof(databaseType) + " != null");
            return GetDbContext(databaseType.Value, connectionString);
        }

        private static IDictionary<string, object> GenerateRequestDictionary(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            IDictionary<string, object> obj = new ExpandoObject();

            foreach (var property in request.GetType().GetProperties())
            {
                obj[property.Name] = property.GetValue(request);
            }

            return obj;
        }

        private static ISqlBase GetDbContext(BctSpDatabaseType databaseType, string connectionString)
        {
            return databaseType switch
            {
                BctSpDatabaseType.MsSql => new MsSql(connectionString),
                BctSpDatabaseType.MySql => new Databases.MySql(connectionString),
                BctSpDatabaseType.PostgreSql => new PostgresSql(connectionString),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static object GetDynamicTask(object response, BctSpCoreRequest<BctSpBaseResponse> request)
        {
            var tcs = typeof(TaskCompletionSource<>).MakeGenericType(
                request?.GetType().BaseType?.GetGenericArguments()[0]);

            var tt = Activator.CreateInstance(tcs);
            tt.GetType().GetMethod("SetResult")?.Invoke(tt, new[] { response });

            return tt.GetType().GetProperty("Task")?.GetValue(tt);
        }

        private static object GetDynamicListTask(object response, BctSpCoreRequest<BctSpBaseResponse> request)
        {
            var tcs = typeof(TaskCompletionSource<>).MakeGenericType(
                typeof(List<>).MakeGenericType(request?.GetType().BaseType?.GetGenericArguments()[0]));

            var tt = Activator.CreateInstance(tcs);
            tt.GetType().GetMethod("SetResult")?.Invoke(tt, new[] { response });

            return tt.GetType().GetProperty("Task")?.GetValue(tt);
        }
    }
}