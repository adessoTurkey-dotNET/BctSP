using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BctSP.Databases
{
    internal class MySqlStoredProcedure : ISqlBase
    {
        private const string SpPropertyName = "x-BctSpName";
        private static string _connectionString;

        private static readonly string[] ExcludedProperties;

        static MySqlStoredProcedure()
        {
            ExcludedProperties = new[]
            {
                SpPropertyName
            };
        }

        public MySqlStoredProcedure(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<string, object> QueryFirst(IDictionary<string, object> parameters)
        {
            IDictionary<string, object> result = new ExpandoObject();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = parameters[SpPropertyName].ToString();
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    if (result.ContainsKey(reader.GetName(i)))
                    {
                        continue;
                    }

                    result.Add(reader.GetName(i), reader.GetValue(i));
                }

                if (result.Any())
                {
                    break;
                }
            }

            return result;
        }

        public IEnumerable<ExpandoObject> Query(IDictionary<string, object> parameters)
        {
            var result = new List<ExpandoObject>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = parameters[SpPropertyName].ToString();
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                IDictionary<string, object> response = new ExpandoObject();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    response.Add(reader.GetName(i), reader.GetValue(i));
                }

                result.Add((ExpandoObject)response);
            }

            return result;
        }

        public void NonQuery(IDictionary<string, object> parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = parameters[SpPropertyName].ToString();
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            command.ExecuteNonQuery();
        }

        public async Task<IDictionary<string, object>> QueryFirstAsync(IDictionary<string, object> parameters)
        {
            IDictionary<string, object> result = new ExpandoObject();
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = parameters[SpPropertyName].ToString();
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    if (result.ContainsKey(reader.GetName(i)))
                    {
                        continue;
                    }

                    result.Add(reader.GetName(i), reader.GetValue(i));
                }

                if (result.Any())
                {
                    break;
                }
            }

            return result;
        }

        public async Task<IEnumerable<ExpandoObject>> QueryAsync(IDictionary<string, object> parameters)
        {
            var result = new List<ExpandoObject>();
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = parameters[SpPropertyName].ToString();
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                IDictionary<string, object> response = new ExpandoObject();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    response.Add(reader.GetName(i), reader.GetValue(i));
                }

                result.Add((ExpandoObject)response);
            }

            return result;
        }

        public async Task NonQueryAsync(IDictionary<string, object> parameters)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = parameters[SpPropertyName].ToString();
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new MySqlParameter($"@{property.Key}", property.Value));
            }

            await command.ExecuteNonQueryAsync();
        }

    }
}