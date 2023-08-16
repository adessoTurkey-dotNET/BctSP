using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BctSP.Databases
{
    internal class MySqlFunction : ISqlBase
    {
        private const string SpPropertyName = "x-BctSpName";
        private static string _connectionString;

        private static readonly string[] ExcludedProperties;

        static MySqlFunction()
        {
            ExcludedProperties = new[]
            {
                SpPropertyName
            };
        }

        public MySqlFunction(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<string, object> QueryFirst(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException("MySql does not support function for query operations.");
        }

        public IEnumerable<ExpandoObject> Query(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException("MySql does not support function for query operations.");
        }

        public void NonQuery(IDictionary<string, object> parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = GetCommandText(parameters);

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            command.ExecuteNonQuery();
        }

        public async Task<IDictionary<string, object>> QueryFirstAsync(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException("MySql does not support function for query operations.");
        }

        public async Task<IEnumerable<ExpandoObject>> QueryAsync(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException("MySql does not support function for query operations.");
        }

        public async Task NonQueryAsync(IDictionary<string, object> parameters)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = GetCommandText(parameters);

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            await command.ExecuteNonQueryAsync();
        }

        private static string GetCommandText(IDictionary<string, object> parameters)
        {
            var sb = new StringBuilder();
            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                sb.Append("@");
                sb.Append(property.Key);
                sb.Append(",");
            }

            return $"SELECT {parameters[SpPropertyName]}({sb.Remove(sb.Length - 1, 1)})";
        }

    }
}