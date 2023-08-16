using BctSP.Attributes;
using BctSP.Infrastructure.BaseModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BctSP.Infrastructure
{
    internal partial class ProxyClass
    {
        public object ProxyObject(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var requestDictionary = GenerateRequestDictionary(request);
            
            var attr = this.GetRequestAttribute(request);
            var sqlDatabase = GetSql(service, _configuration, attr );

            requestDictionary.Add("x-BctSpName", attr.BctSpName);
            var responseDictionaryFirst = sqlDatabase.QueryFirst(requestDictionary);

            var response = GetResponseInstance(request, false);

            MapResponseDictionaryToResponseObject(response, responseDictionaryFirst);

            return response;
        }

        public void ProxyVoid(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var requestDictionary = GenerateRequestDictionary(request);

            var attr = this.GetRequestAttribute(request);
            var sqlDatabase = GetSql(service, _configuration, attr);

            requestDictionary.Add("x-BctSpName", attr.BctSpName);
            sqlDatabase.NonQuery(requestDictionary);
        }

        public object ProxyList(BctSpCoreRequest<BctSpBaseResponse> request)
        {
            using var service = _serviceScope.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var requestDictionary = GenerateRequestDictionary(request);

            var attr = this.GetRequestAttribute(request);
            var sqlDatabase = GetSql(service, _configuration, attr);

            requestDictionary.Add("x-BctSpName", attr.BctSpName);
            object responseDictionary = sqlDatabase.Query(requestDictionary);

            var response = GetResponseInstance(request, true);

            MapResponseDictionaryToResponseObjectList(response, responseDictionary);

            return response;
        }
    }
}