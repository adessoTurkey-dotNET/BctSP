using BctSP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BctSP
{
    /// <summary>
    /// 
    /// </summary>
    public class BctSpConfiguration
    {
        /// <summary>
        /// -MsSql, MySql -> Stored procedure, PostgreSql -> function- Name
        /// </summary>
        public string BctSpName { get; set; }

        /// <summary>
        /// Custom database connection string directly or configuration path to connection string. -Use this if you want override global configuration for the request.
        /// </summary>
        public string BctSpConnectionStringOrConfigurationPath { get; set; }

        /// <summary>
        /// Custom database type. -Use this if you want override global configuration for the request.
        /// </summary>
        public BctSpDatabaseType? BctSpDatabaseType { get; set; }

        /// <summary>
        /// Custom command type. -Use this if you want override global configuration for the request.
        /// </summary>
        public BctSpCommandType BctSpCommandType { get; set; }
    }
}
