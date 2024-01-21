using MySqlConnector;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Services.Table_Services
{
    public class Account_Table_Services
    {
        // Information about the table inside the database server
        private readonly string table_name = "account";
        private readonly string schema = "accountdata";

        // Information about the connection string corresponding to the Session_Id to the database server
        private string? connection_string { get; set; } = null;
        private string? Session_Id { get; set; } = null;

        // Information about the data inside the table
        private List<Account_Model>? data { get; set; } = null;

        private Account_Table_Services? _instance;
        private IConfiguration? _configuration;
        public Account_Table_Services? SetUp(string? session_id, IConfiguration configuration)
        {
            if(session_id != null)
            {
                _configuration = configuration;
                data = new List<Account_Model>();
                Session_Id = session_id;
                connection_string = _configuration.GetConnectionString("AccountTableConnection");
                return Instance;
            }else
            {
                return null;
            }
        }

        public Account_Table_Services Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Account_Table_Services();
                }
                return _instance;
            }
        }

        public async Task<List<Account_Model>?> ReadAllAsync()
        {

            if(connection_string == null)
            {
                return null;
            }

            if (data != null)
            {
                data.Clear();
            }

            try
            {
                using MySqlConnection Connection = new MySqlConnection(connection_string);
                await Connection.OpenAsync();
                string sql = $"SELECT * FROM {schema}.{table_name};";
                using var cmd = new MySqlCommand(sql, Connection);
                using var reader = await cmd.ExecuteReaderAsync();
                data = new List<Account_Model>();
                while (await reader.ReadAsync())
                {
                    var Account = new Account_Model
                    {
                        Username = reader.GetString(0),
                        Password = reader.GetString(1),
                        AuthorName = reader.GetString(2),
                    };
                    data.Add(Account);
                }
                await Connection.CloseAsync();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<bool> DeleteAllDataAsync()
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"DELETE FROM {schema}.{table_name};";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    await connection.CloseAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> CreateDataAsync(Account_Model account)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();
                Type? Account_Model_type = typeof(Account_Model);
                string[] propertyNames = Account_Model_type.GetProperties()
                   .Where(p => p.Name != "Id")
                   .Select(p => p.Name)
                   .ToArray();

                string columnNames = string.Join(", ", propertyNames.Select(name => $"`{name}`"));
                string parameterNames = string.Join(", ", propertyNames.Select(name => $"@{name}"));

                string sql = $"INSERT INTO {schema}.{table_name} ({columnNames}) VALUES ({parameterNames});";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    // Set command parameters based on property names
                    foreach (var propertyName in propertyNames)
                    {
                        var propertyValue = Account_Model_type.GetProperty(propertyName)?.GetValue(account);
                        cmd.Parameters.AddWithValue($"@{propertyName}", propertyValue);
                    }

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0 && account != null)
                    {
                        string? dataContent = account.ToString();
                    }

                    await connection.CloseAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteDataAsync(string? username)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"DELETE FROM {schema}.{table_name} WHERE `Username` = @Username;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    await connection.CloseAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<Account_Model?> ReadDataAsync(string? username)
        {
            if (connection_string == null)
            {
                return null;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"SELECT * FROM {schema}.{table_name} WHERE `Username` = @Username;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection)) 
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Account_Model account = new Account_Model
                            {
                                Username = reader.GetString(0),
                                Password = reader.GetString(1),
                                AuthorName = reader.GetString(2),
                            };
                            if (account.Username == username)
                            {
                                return account;
                            }else
                            {
                                return null;
                            }
                        }
                    }
                }

                await connection.CloseAsync();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateDataAsync(string? username, string? propertyUpdated, string? newValue)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"UPDATE {schema}.{table_name} SET `{propertyUpdated}` = @NewValue WHERE `Username` = @Username;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@NewValue", newValue);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    await connection.CloseAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
