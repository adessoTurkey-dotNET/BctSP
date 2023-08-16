using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using Oracle.ManagedDataAccess.Types;

namespace BctSP.Databases
{
    internal class OracleSqlStoredProcedure : ISqlBase
    {
        private const string SpPropertyName = "x-BctSpName";
        private static string _connectionString;
        private static readonly string[] ExcludedProperties;

        static OracleSqlStoredProcedure()
        {
            ExcludedProperties = new[]
            {
                SpPropertyName
            };
        }

        public OracleSqlStoredProcedure(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<string, object> QueryFirst(IDictionary<string, object> parameters)
        {
            IDictionary<string, object> result = new ExpandoObject();
            using var connection = new OracleConnection(_connectionString);
            connection.Open();
            using var command = new OracleCommand(parameters[SpPropertyName].ToString(), connection);
            command.CommandType = CommandType.StoredProcedure;

            OracleParameter refCursor = new OracleParameter();
            refCursor.OracleDbType = OracleDbType.RefCursor;
            refCursor.Direction = ParameterDirection.Output;
            command.Parameters.Add(refCursor);

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new OracleParameter($"@{property.Key}", property.Value));
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
            using var connection = new OracleConnection(_connectionString);
            connection.Open();

            using var command = new OracleCommand(parameters[SpPropertyName].ToString(), connection);
            command.CommandType = CommandType.StoredProcedure;

            OracleParameter refCursor = new OracleParameter();
            refCursor.OracleDbType = OracleDbType.RefCursor;
            refCursor.Direction = ParameterDirection.Output;
            command.Parameters.Add(refCursor);


            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new OracleParameter($"@{property.Key}", property.Value));
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
            using var connection = new OracleConnection(_connectionString);
            connection.Open();
            using var command = new OracleCommand(parameters[SpPropertyName].ToString(), connection);
            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new OracleParameter($"@{property.Key}", property.Value));
            }

            command.ExecuteNonQuery();
        }

        public async Task<IDictionary<string, object>> QueryFirstAsync(IDictionary<string, object> parameters)
        {
            IDictionary<string, object> result = new ExpandoObject();
            await using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new OracleCommand(parameters[SpPropertyName].ToString(), connection);

            command.CommandType = CommandType.StoredProcedure;

            OracleParameter refCursor = new OracleParameter();
            refCursor.OracleDbType = OracleDbType.RefCursor;
            refCursor.Direction = ParameterDirection.Output;
            command.Parameters.Add(refCursor);

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new OracleParameter($"@{property.Key}", property.Value));
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
            await using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new OracleCommand(parameters[SpPropertyName].ToString(), connection);
            command.CommandType = CommandType.StoredProcedure;

            OracleParameter refCursor = new OracleParameter();
            refCursor.OracleDbType = OracleDbType.RefCursor;
            refCursor.Direction = ParameterDirection.Output;
            command.Parameters.Add(refCursor);

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new OracleParameter($"@{property.Key}", property.Value));
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
            await using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new OracleCommand(parameters[SpPropertyName].ToString(), connection);

            command.CommandType = CommandType.StoredProcedure;

            foreach (var property in parameters.Where(x => !ExcludedProperties.Contains(x.Key)))
            {
                command.Parameters.Add(new OracleParameter($"@{property.Key}", property.Value));
            }

            await command.ExecuteNonQueryAsync();
        }

    }
}
