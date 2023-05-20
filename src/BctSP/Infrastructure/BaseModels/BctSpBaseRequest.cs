namespace BctSP.Infrastructure.BaseModels
{
    // ReSharper disable once UnusedTypeParameter
    public abstract class BctSpBaseRequest<T> : BctSpCoreRequest where T : BctSpBaseResponse
    {
        protected BctSpBaseRequest(string bctSpName)
        {
            BctSpName = bctSpName;
        }

        protected BctSpBaseRequest(string bctSpName, string bctSpConnectionStringOrConfigurationPath, BctSpDatabaseType? bctSpDatabaseType)
        {
            BctSpName = bctSpName;
            BctSpConnectionStringOrConfigurationPath = bctSpConnectionStringOrConfigurationPath;
            BctSpDatabaseType = bctSpDatabaseType;
        }

        /// <summary>
        /// -MsSql, MySql -> Stored procedure, PostgreSql -> function- Name
        /// </summary>
        public string BctSpName { get; }

        /// <summary>
        /// Custom database connection string directly or configuration path to connection string. -Use this if you want override global configuration for the request.
        /// </summary>
        public string BctSpConnectionStringOrConfigurationPath { get; set; }

        /// <summary>
        /// Custom database type. -Use this if you want override global configuration for the request.
        /// </summary>
        public BctSpDatabaseType? BctSpDatabaseType { get; set; }
    }
}