using BctSP.Infrastructure.BaseModels;
using Microsoft.Extensions.DependencyInjection;

namespace BctSP.Infrastructure
{
    internal partial class ProxyClass
    {
        public object ProxyObject(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var requestDictionary = GenerateRequestDictionary(request);

            var sqlDatabase = GetSql(service, requestDictionary, _configuration);
            var responseDictionaryFirst = sqlDatabase.QueryFirst(requestDictionary);

            var response = GetResponseInstance(request, false);

            MapResponseDictionaryToResponseObject(response, responseDictionaryFirst);

            return response;
        }

        public void ProxyVoid(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var requestDictionary = GenerateRequestDictionary(request);

            var sqlDatabase = GetSql(service, requestDictionary, _configuration);
            sqlDatabase.NonQuery(requestDictionary);
        }

        public object ProxyList(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var requestDictionary = GenerateRequestDictionary(request);

            var sqlDatabase = GetSql(service, requestDictionary, _configuration);
            object responseDictionary = sqlDatabase.Query(requestDictionary);

            var response = GetResponseInstance(request, true);

            MapResponseDictionaryToResponseObjectList(response, responseDictionary);

            return response;
        }
    }
}