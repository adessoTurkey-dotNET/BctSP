using BctSP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BctSP.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = true)]
    public class BctSpAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public BctSpAttribute(string bctSpName, BctSpCommandType bctSpCommandType)
        {
            BctSpName = bctSpName;
            BctSpCommandType = bctSpCommandType;
        }

        /// <summary>
        /// 
        /// </summary>
        public BctSpAttribute(string bctSpName, BctSpCommandType bctSpCommandType, string bctSpConnectionStringOrConfigurationPath, BctSpDatabaseType bctSpDatabaseType)
        {
            BctSpName = bctSpName;
            BctSpCommandType = bctSpCommandType;
            BctSpConnectionStringOrConfigurationPath = bctSpConnectionStringOrConfigurationPath;
            BctSpDatabaseType = bctSpDatabaseType;
        }

        /// <summary>
        /// -MsSql, MySql, PostgreSql, Oracle -> Stored procedure, -MsSql, MySql, PostgreSql, Oracle -> function- Name
        /// </summary>
        public string BctSpName { get; }

        /// <summary>
        /// Custom database connection string directly or configuration path to connection string. -Use this if you want override global configuration for the request.
        /// </summary>
        public string BctSpConnectionStringOrConfigurationPath { get; }

        /// <summary>
        /// Custom database type. -Use this if you want override global configuration for the request.
        /// </summary>
        public BctSpDatabaseType BctSpDatabaseType { get; }

        /// <summary>
        /// Custom command type. -Use this if you want override global configuration for the request.
        /// </summary>
        public BctSpCommandType BctSpCommandType { get; }

    }
}
