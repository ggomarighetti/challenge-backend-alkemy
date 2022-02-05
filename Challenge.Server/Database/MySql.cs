using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Challenge.Server.Database
{
    public class MySql
    {
        private string ConnectionString { get; set; } = string.Empty;

        public MySql(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<long> ExecuteQuery(string query, Dictionary<string, object> parameters = null,
            CommandType type = CommandType.StoredProcedure)
        {
            using MySqlConnection connection = new(ConnectionString);
            using MySqlCommand command = new(query, connection) { CommandType = type };

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            try
            {
                connection.Open();
                await command.ExecuteNonQueryAsync();
                return command.LastInsertedId;
            }
            catch (MySqlException e)
            {
                throw new Exception("Error en la ejecucion de comando MySql " + e.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public async Task<DataTable> ExecuteReader(string query, Dictionary<string, object> parameters = null,
            CommandType type = CommandType.StoredProcedure)
        {
            using MySqlConnection connection = new(ConnectionString);
            using MySqlCommand command = new(query, connection) { CommandType = type };

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value).Direction = ParameterDirection.Input;
                }
            }

            using DataTable result = new();

            try
            {
                connection.Open();

                using var reader = await command.ExecuteReaderAsync();

                result.Load(reader);
                reader.Close();
                return result;
            }
            catch (MySqlException e)
            {
                throw new Exception("Error en la ejecucion de comando MySql " + e.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public async Task<object> ExecuteScalar(string query, Dictionary<string, object> parameters = null,
            CommandType type = CommandType.StoredProcedure)
        {
            using MySqlConnection connection = new(ConnectionString);
            using MySqlCommand command = new(query, connection) { CommandType = type };

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value).Direction = ParameterDirection.Input;
                }
            }

            try
            {
                connection.Open();
                return await command.ExecuteScalarAsync();
            }
            catch (MySqlException e)
            {
                throw new Exception("Error en la ejecucion de comando MySql " + e.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
