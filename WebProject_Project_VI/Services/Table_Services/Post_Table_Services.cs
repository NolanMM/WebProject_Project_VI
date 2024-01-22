using MySqlConnector;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Services.Table_Services
{
    public class Post_Table_Services : ITableServices
    {
        // Information about the table inside the database server
        private readonly string table_name = "post";
        private readonly string schema = "postdata";
        private readonly string connection_key = "PostTableConnection";
        private readonly string Username_Authorized = "historyfellow@Post";
        private readonly string Password_Authorized = "HistoryFellow@Password";

        // Information about the connection string corresponding to the Session_Id to the database server
        private string? connection_string { get; set; } = null;
        private string? Session_Id { get; set; } = null;

        // Information about the data inside the table
        private List<Post_Model>? data { get; set; } = null;

        private Post_Table_Services? _instance;
        private IConfiguration? _configuration;
        public Type Get_Type()
        {
            return typeof(Post_Table_Services);
        }
        public ITableServices? SetUp(string? session_id, IConfiguration configuration)
        {
            if(session_id == null || configuration == null)
            {
                return null;
            }
            return Set_Up_Post_Table_Services(session_id, configuration);
        }
        public Post_Table_Services? Set_Up_Post_Table_Services(string? session_id, IConfiguration configuration)
        {
            if (session_id != null)
            {
                _instance = new Post_Table_Services();
                _configuration = configuration;
                data = new List<Post_Model>();
                Session_Id = session_id;
                connection_string = _configuration.GetConnectionString(connection_key);
                return _instance;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<IData>?> Read_All_Data_Async(string? username_Authorized, string? password_Authorized)
        {

            if (connection_string == null)
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
            if (connection_string == null)
            {
                return false;
            }

            if(username == null || password == null)
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
        public async Task<bool> Create_Post_Data_By_Model_Async_(Post_Model post)
        {
            if (connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection Connection = new MySqlConnection(connection_string);
                await Connection.OpenAsync();

                string sql = $"INSERT INTO {schema}.{table_name} " +
                             "(Title, Content, Author, Number_Of_Likes, Number_Of_DisLikes, Is_Public, Number_Of_Visits, Date) " +
                             "VALUES " +
                             "(@Title, @Content, @Author, @Number_Of_Likes, @Number_Of_DisLikes, @Is_Public, @Number_Of_Visits, @Date);";

                using var cmd = new MySqlCommand(sql, Connection);

                post.Number_Of_Likes = 0;
                post.Number_Of_DisLikes = 0;
                post.Number_Of_Visits = 0;
                post.Date = DateTime.Now;

                cmd.Parameters.AddWithValue("@Title", post.Title);
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@Author", post.Author);
                cmd.Parameters.AddWithValue("@Number_Of_Likes", post.Number_Of_Likes);
                cmd.Parameters.AddWithValue("@Number_Of_DisLikes", post.Number_Of_DisLikes);
                cmd.Parameters.AddWithValue("@Is_Public", post.Is_Public);
                cmd.Parameters.AddWithValue("@Number_Of_Visits", post.Number_Of_Visits);
                cmd.Parameters.AddWithValue("@Date", post.Date);

                int affectedRows = await cmd.ExecuteNonQueryAsync();

                await Connection.CloseAsync();

                // If at least one row was affected, consider the operation successful
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> Create_Post_Data_By_Passing_Values_Async(string? Title, string? Content, string? Author, bool? Is_Public)
        {
            if(connection_string == null)
            {
                return false;
            }

            try
            {
                using MySqlConnection Connection = new MySqlConnection(connection_string);
                await Connection.OpenAsync();

                string sql = $"INSERT INTO {schema}.{table_name} " +
                             "(Title, Content, Author, Number_Of_Likes, Number_Of_DisLikes, Is_Public, Number_Of_Visits, Date) " +
                             "VALUES " +
                             "(@Title, @Content, @Author, @Number_Of_Likes, @Number_Of_DisLikes, @Is_Public, @Number_Of_Visits, @Date);";

                using var cmd = new MySqlCommand(sql, Connection);

                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@Content", Content);
                cmd.Parameters.AddWithValue("@Author", Author);
                cmd.Parameters.AddWithValue("@Number_Of_Likes", 0);
                cmd.Parameters.AddWithValue("@Number_Of_DisLikes", 0);
                cmd.Parameters.AddWithValue("@Is_Public", Is_Public);
                cmd.Parameters.AddWithValue("@Number_Of_Visits", 0);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                int affectedRows = await cmd.ExecuteNonQueryAsync();

                await Connection.CloseAsync();

                // If at least one row was affected, consider the operation successful
                return affectedRows > 0;
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

        public async Task<List<IData>?> Read_Post_Data_By_Title_Async(string? title)
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
                        List<Post_Model>  _data = new List<Post_Model>();
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
                            _data.Add(post);
                        }
                        await connection.CloseAsync();
                        return _data.ConvertAll(post=> (IData)post);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public async Task<List<IData>?> Read_Post_Data_By_Author_Async(string? author)
        {
            if (connection_string == null)
            {
                return null;
            }

            try
            {
                using MySqlConnection connection = new MySqlConnection(connection_string);
                await connection.OpenAsync();

                string sql = $"SELECT * FROM {schema}.{table_name} WHERE `Author` = @author;";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@author", author);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        List<Post_Model> _data = new List<Post_Model>();
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
                            _data.Add(post);
                        }
                        await connection.CloseAsync();
                        return _data.ConvertAll(post => (IData)post);
                    }
                }
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
