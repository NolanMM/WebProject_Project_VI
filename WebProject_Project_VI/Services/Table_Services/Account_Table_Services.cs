using MySqlConnector;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Services.Table_Services
{
    public class Account_Table_Services : ITableServices
    {
        // Information about the table inside the database server
        private readonly string table_name = "account";
        private readonly string schema = "accountdata";
        private readonly string connection_key = "AccountTableConnection";
        private readonly string Username_Authorized = "historyfellow@Account";
        private readonly string Password_Authorized = "HistoryFellow@Password";

        // Information about the connection string corresponding to the Session_Id to the database server
        private string? connection_string { get; set; } = null;
        private string? Session_Id { get; set; } = null;

        // Information about the data inside the table
        private List<Account_Model>? data { get; set; } = null;

        private Account_Table_Services? _instance;
        private IConfiguration? _configuration;
        public string? Get_Session_Id()
        {
            if(Session_Id == null)
            {
                return null;
            }
            return Session_Id;
        }
        public Type Get_Type()
        {
            return typeof(Account_Table_Services);
        }
        public ITableServices? SetUp(string? session_id, IConfiguration configuration)
        {
            if (session_id == null || configuration == null)
            {
                return null;
            }
            return Set_Up_Account_Table_Services(session_id, configuration);
        }
        public Account_Table_Services? Set_Up_Account_Table_Services(string? session_id, IConfiguration configuration)
        {
            if(session_id != null)
            {
                _instance = new Account_Table_Services();
                _configuration = configuration;
                data = new List<Account_Model>();
                Session_Id = session_id;
                connection_string = _configuration.GetConnectionString(connection_key);
                return _instance;
            }else
            {
                return null;
            }
        }
        public async Task<List<IData>?> Read_All_Data_Async(string? username_Authorized, string? password_Authorized)
        {

            if(connection_string == null)
            {
                return null;
            }

            if (data != null)
            {
                data.Clear();
            }
            if(username_Authorized == null || password_Authorized == null)
            {
                return null;
            }
            if (username_Authorized == Username_Authorized && password_Authorized == Password_Authorized)
            {
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
                    return data.ConvertAll(post => (IData)post);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Delete_All_Data_Async(string? username, string? password)
        {
            if (username == null || password == null)
            {
                return false;
            }
            if (connection_string == null)
            {
                return false;
            }
            MySqlConnectionStringBuilder Extracter = new MySqlConnectionStringBuilder(connection_string);
            if (username == Extracter.UserID && password == Extracter.Password)
            {
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
            else
            {
                return false;
            }
        }
        public async Task<bool> Create_Account_Data_By_Passing_Values_Async(string? username, string? password, string? author_name)
        {
            if (connection_string == null)
            {
                return false;
            }
            if(username == null || password == null || author_name == null)
            {
                return false;
            }
            Account_Model account = new Account_Model
            {
                Username = username,
                Password = password,
                AuthorName = author_name,
            };
            return await Create_Account_Data_By_Account_Model_Async(account);
        }

        public async Task<bool> Create_Account_Data_By_Account_Model_Async(Account_Model account)
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

        public async Task<bool> Delete_Account_Data_By_Username_Async(string? username)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"DELETE FROM {schema}.{table_name} WHERE `Username` = \"" + username + "\";";

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

        public async Task<IData?> Read_Account_Data_By_Username_Async(string? username)
        {
            if (connection_string == null)
            {
                return null;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"SELECT * FROM {schema}.{table_name} WHERE `Username` = \"" + username + "\";";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection)) 
                {
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

        public async Task<bool> Update_Account_Data_By_Username_Async(string? username, string? propertyUpdated, string? newValue)
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
