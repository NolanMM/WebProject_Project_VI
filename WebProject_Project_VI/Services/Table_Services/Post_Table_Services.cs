using MySqlConnector;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Services.Table_Services
{
    public class Post_Table_Services
    {
        // Information about the table inside the database server
        private readonly string table_name = "post";
        private readonly string schema = "postdata";

        // Information about the connection string corresponding to the Session_Id to the database server
        private string? connection_string { get; set; } = null;
        private string? Session_Id { get; set; } = null;

        // Information about the data inside the table
        private List<Post_Model>? data { get; set; } = null;

        private Post_Table_Services? _instance;
        private IConfiguration? _configuration;

        public Post_Table_Services? Set_Up_Post_Table_Services(string? session_id, IConfiguration configuration)
        {
            if (session_id != null)
            {
                _configuration = configuration;
                data = new List<Post_Model>();
                Session_Id = session_id;
                connection_string = _configuration.GetConnectionString("AccountTableConnection");
                return Instance;
            }
            else
            {
                return null;
            }
        }

        public Post_Table_Services Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Post_Table_Services();
                }
                return _instance;
            }
        }
        public async Task<List<Post_Model>?> Read_All_Post_Data_Async()
        {

            if (connection_string == null)
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
                data = new List<Post_Model>();
                while (await reader.ReadAsync())
                {
                    var post = new Post_Model
                    {
                        Title = reader.GetString(0),
                        Content = reader.GetString(1),
                        Author = reader.GetString(2),
                        Number_Of_Likes = reader.GetInt32(3),
                        Number_Of_DisLikes = reader.GetInt32(4),
                        Is_Public = reader.GetBoolean(5),
                        Number_Of_Visits = reader.GetInt32(6),
                        Date = reader.GetDateTime(7)
                    };
                    data.Add(post);
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
        public async Task<bool> Delete_All_Post_Data_Async()
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

        public async Task<bool> Create_Post_Data_By_Model_Async(Post_Model post)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();
                Type? Post_Model_type = typeof(Post_Model);
                string[] propertyNames = Post_Model_type.GetProperties()
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
                        var propertyValue = Post_Model_type.GetProperty(propertyName)?.GetValue(post);
                        cmd.Parameters.AddWithValue($"@{propertyName}", propertyValue);
                    }

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0 && post != null)
                    {
                        string? dataContent = post.ToString();
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

        public async Task<bool> Delete_Post_By_Title_Async(string? title)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"DELETE FROM {schema}.{table_name} WHERE `Title` = @title;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", title);

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

        public async Task<Post_Model?> Read_Post_Data_By_Title_Async(string? title)
        {
            if (connection_string == null)
            {
                return null;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"SELECT * FROM {schema}.{table_name} WHERE `Title` = @title;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", title);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Post_Model post = new Post_Model
                            {
                                Title = reader.GetString(0),
                                Content = reader.GetString(1),
                                Author = reader.GetString(2),
                                Number_Of_Likes = reader.GetInt32(3),
                                Number_Of_DisLikes = reader.GetInt32(4),
                                Is_Public = reader.GetBoolean(5),
                                Number_Of_Visits = reader.GetInt32(6),
                                Date = reader.GetDateTime(7)
                            };
                            if (post != null)
                            {
                                string? dataContent = post.ToString();
                                await connection.CloseAsync();
                                return post;
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

        public async Task<bool> Update_Post_Data_By_Title_Async(string? title, string? propertyUpdated, string? newValue)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"UPDATE {schema}.{table_name} SET `{propertyUpdated}` = @NewValue WHERE `Title` = @title;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", title);
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
