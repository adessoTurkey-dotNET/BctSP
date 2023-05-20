using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace BctSP.Databases
{
    internal interface ISqlBase
    {
        #region Sync

        IDictionary<string, object> QueryFirst(IDictionary<string, object> parameters);
        IEnumerable<ExpandoObject> Query(IDictionary<string, object> parameters);
        public void NonQuery(IDictionary<string, object> parameters);

        #endregion

        #region Async

        Task<IDictionary<string, object>> QueryFirstAsync(IDictionary<string, object> parameters);
        Task<IEnumerable<ExpandoObject>> QueryAsync(IDictionary<string, object> parameters);
        public Task NonQueryAsync(IDictionary<string, object> parameters);

        #endregion
    }
}