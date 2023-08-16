using System.Threading.Tasks;
using BctSP.Infrastructure.BaseModels;
using Microsoft.Extensions.DependencyInjection;

namespace BctSP.Infrastructure
{
    internal partial class ProxyClass
    {
        public object ProxyObjectAsync(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            return Task.Run(async () =>
            {
                using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var requestDictionary = GenerateRequestDictionary(request);

                var attr = this.GetRequestAttribute(request);
                var sqlDatabase = GetSql(service, _configuration, attr);

                requestDictionary.Add("x-BctSpName", attr.BctSpName);
                object responseDictionaryFirst = await sqlDatabase.QueryFirstAsync(requestDictionary);

                var response = GetResponseInstance(request, false);
                MapResponseDictionaryToResponseObject(response, responseDictionaryFirst);

                return GetDynamicTask(response, request);
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task ProxyVoidAsync(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            await Task.Run(async () =>
            {
                using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var requestDictionary = GenerateRequestDictionary(request);

                var attr = this.GetRequestAttribute(request);
                var sqlDatabase = GetSql(service, _configuration, attr);

                requestDictionary.Add("x-BctSpName", attr.BctSpName);
                await sqlDatabase.NonQueryAsync(requestDictionary);
            });
        }

        public object ProxyListAsync(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            return Task.Run(async () =>
            {
                using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var requestDictionary = GenerateRequestDictionary(request);

                var attr = this.GetRequestAttribute(request);
                var sqlDatabase = GetSql(service, _configuration, attr);


                requestDictionary.Add("x-BctSpName", attr.BctSpName);
                object responseDictionary = await sqlDatabase.QueryAsync(requestDictionary);

                var response = GetResponseInstance(request, true);

                MapResponseDictionaryToResponseObjectList(response, responseDictionary);

                return GetDynamicListTask(response, request);
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}