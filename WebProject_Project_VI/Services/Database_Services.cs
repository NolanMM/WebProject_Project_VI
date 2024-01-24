using System.ComponentModel.DataAnnotations;
using WebProject_Project_VI.Controllers;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services.Table_Services;

namespace WebProject_Project_VI.Services
{
    public class Database_Services
    {
        private  ILogger<HomeController>? _logger;
        private  IConfiguration? _configuration;

        private Database_Services? _instance;
        private Account_Table_Services _account_Table_Services;
        private Post_Table_Services _post_Table_Services = new Post_Table_Services();

        private string? SesionID;
        private readonly string Account_Table_Name = "account";
        private readonly string Post_Table_Name = "post";
        public Database_Services? Set_Up_Database_Services(ILogger<HomeController>? logger, IConfiguration? configuration, string? SessionID)
        {
            if (logger != null)
            {
                if (SessionID == null || configuration == null)
                {
                    _logger.LogError("Failed to create Database Services constructor!");
                    return null;
                }
                _instance = new Database_Services();
                if (configuration == null || SessionID == null)
                {
                    _logger.LogError("Failed to create Database Services constructor!");
                    return null;
                }
                _configuration = configuration;
                SesionID = SessionID;
                _account_Table_Services = _account_Table_Services.Set_Up_Account_Table_Services(SessionID, configuration);
                _post_Table_Services = _post_Table_Services.Set_Up_Post_Table_Services(SessionID, configuration);
                _logger = logger;
                _logger.LogInformation($"Created Database Services constructor for SessionID: {SessionID} successfully!");
                return _instance;
            }
            else
            {
                return null;
            }
        }
        public string ? Get_Session_Id()
        {
            return SesionID;
        }
        public async Task<List<IData>?> Read_All_Data_By_Table_Name_Async(string? table_name,string? username_account_authorized, string? password_account_authorized)
        {
            if(table_name == Account_Table_Name)
            {
                List<IData>? account_Models = await _account_Table_Services.Read_All_Data_Async(username_account_authorized, password_account_authorized);
                if (account_Models == null)
                {
                    if (_logger != null) { _logger.LogError("Failed to Read all data from " + table_name + " Table!"); }
                    return null;
                }
                else
                {
                    if (_logger != null)
                    {
                        _logger.LogInformation("Read all data from " + table_name + " Table successfully!");
                    }
                    return account_Models;
                }
            }else
            {
                List<IData>? post_Models = await _post_Table_Services.Read_All_Data_Async(username_account_authorized, password_account_authorized);
                if (post_Models == null)
                {
                    if (_logger != null)
                    {
                        _logger.LogError("Failed to Read all data from " + table_name + " Table!");
                    }
                    return null;
                }
                else
                {
                    if (_logger != null)
                    {
                        _logger.LogInformation("Read all data from " + table_name + " Table successfully!");
                    }
                    return post_Models;
                }
            }
            
        }
        public async Task<bool?> Delete_All_Data_By_Table_Name_Async(string? table_name, string? username_account_authorized, string? password_account_authorized)
        {
            if (table_name == Account_Table_Name)
            {
                bool is_Table_Empty = await _account_Table_Services.Delete_All_Data_Async(username_account_authorized, password_account_authorized);
                if (is_Table_Empty == false)
                {
                    if(_logger != null)
                        _logger.LogError("Failed to Delete all data from " + table_name + " Table!");
                    return null;
                }
                else
                {
                    if (_logger != null)
                        _logger.LogInformation("Delete all data from " + table_name + " Table successfully!");
                    return is_Table_Empty;
                }
            }
            else
            {
                bool is_Table_Empty = await _post_Table_Services.Delete_All_Data_Async(username_account_authorized, password_account_authorized);
                if (is_Table_Empty == false)
                {
                    if (_logger != null)
                        _logger.LogError("Failed to Delete all data from " + table_name + " Table!");
                    return null;
                }
                else
                {
                    if (_logger != null)
                        _logger.LogInformation("Delete all data from " + table_name + " Table successfully!");
                    return is_Table_Empty;
                }
            }

        }
        public async Task<Account_Model?> Read_Account_Data_By_Username_Async(string? username)
        {
            Account_Model? account_Model = await _account_Table_Services.Read_Account_Data_By_Username_Async(username) as Account_Model;
            if (account_Model == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to read account data by username from Account Table!");
                return null;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Read account data by username from Account Table successfully!");
                return account_Model;
            }
        }
        public async Task<bool> Create_Account_Data_By_Passing_Values_Async(string? username, string? password, string? author_name)
        {
            bool is_Account_Created = await _account_Table_Services.Create_Account_Data_By_Passing_Values_Async(username, password, author_name);
            if (is_Account_Created == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to create account data by passing values from Account Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Created account data by passing values from Account Table successfully!");
                return is_Account_Created;
            }
        }
        public async Task<bool> Delete_Account_Data_By_Username_Async(string? username)
        {
            bool is_Account_Deleted = await _account_Table_Services.Delete_Account_Data_By_Username_Async(username);
            if (is_Account_Deleted == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to delete account data by username from Account Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Deleted account data by username from Account Table successfully!");
                return is_Account_Deleted;
            }
        }
        public async Task<List<Post_Model>?> Read_Posts_By_Authors_Name_Async(string? authors_name)
        {
            List<IData>? post_Models = await _post_Table_Services.Read_Post_Data_By_Author_Async(authors_name);
            if (post_Models == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to read posts by authors name from Post Table!");
                return null;
            }
            else
            {
                List<Post_Model>? post_Models_Return = post_Models.ConvertAll(post => (Post_Model)post);
                if(post_Models_Return == null)
                {
                    if (_logger != null)
                        _logger.LogError("Failed to convert post models to post models!");
                    return null;
                }
                else
                {
                    if (_logger != null)
                        _logger.LogInformation("Read posts by authors name from Post Table successfully!");
                    return post_Models_Return;
                }
            }
        }
        public async Task<Post_Model?> Read_Post_Data_By_Title_Async(string? title)
        {
            Post_Model? post_Models = await _post_Table_Services.Read_Post_Data_By_Title_Async(title);
            if (post_Models == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to read post data by title from Post Table!");
                return null;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Read post data by title from Post Table successfully!");
                return post_Models;
            }
        }
        public async Task<bool> Create_Post_Data_By_Passing_Values_Async(string? Post_ID, string? Title, string? Content, string? Author, bool? Is_Public)
        {
            if(Post_ID ==null || Title == null || Content == null || Author == null || Is_Public == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to create post data by passing values from Post Table!");
                return false;
            }
            bool is_Post_Created = await _post_Table_Services.Create_Post_Data_By_Passing_Values_Async(Post_ID, Title, Content, Author, Is_Public);
            if (is_Post_Created == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to create post data by passing values from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Created post data by passing values from Post Table successfully!");
                return is_Post_Created;
            }
        }

        public async Task<bool> Create_Post_Data_By_Model_Async_(Post_Model post)
        {
            if (post == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to create post data by passing values from Post Table!");
                return false;
            }
            bool is_Post_Created = await _post_Table_Services.Create_Post_Data_By_Model_Async_(post);
            if (is_Post_Created == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to create post data by passing values from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Created post data by passing values from Post Table successfully!");
                return is_Post_Created;
            }

        }
        public async Task<bool> Delete_Post_By_Title_Async(string? title)
        {
            if (title == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to delete post data by title from Post Table!");
                return false;
            }
            bool is_Post_Deleted = await _post_Table_Services.Delete_Post_By_Title_Async(title);
            if (is_Post_Deleted == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to delete post data by title from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Deleted post data by title from Post Table successfully!");
                return is_Post_Deleted;
            }
        }

        public async Task<bool> Update_Password_Account_Data_By_Username_Async(string? username, string? new_Password_Value)
        {
            string propertyUpdated = "Password";
            bool is_Account_Updated = await _account_Table_Services.Update_Account_Data_By_Username_Async(username, propertyUpdated, new_Password_Value);
            if (is_Account_Updated == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update account data by username from Account Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Updated account data by username from Account Table successfully!");
                return is_Account_Updated;
            }
        }

        public async Task<bool> Update_Author_Name_Account_Data_By_Username_Async(string? username, string? new_Author_Name_Value)
        {
            string propertyUpdated = "AuthorName";
            bool is_Account_Updated = await _account_Table_Services.Update_Account_Data_By_Username_Async(username, propertyUpdated, new_Author_Name_Value);
            if (is_Account_Updated == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update account data by username from Account Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Updated account data by username from Account Table successfully!");
                return is_Account_Updated;
            }
        }
        public async Task<bool> Update_Public_Mode_Post_Data_By_Title_Async(string? title, bool? Is_Public)
        {
            string propertyUpdated = "Is_Public";
            bool is_Account_Updated = await _post_Table_Services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, Is_Public.ToString(), typeof(bool));
            if (is_Account_Updated == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Title from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Updated Post data by Title from Post Table successfully!");
                return is_Account_Updated;
            }
        }
        /*   
         *  @ Update_Property_Data_Post_Data_By_Title_And_Property_Async Function
        //  List Property Names:    Title : string
        //                          Content : string
        //                          Author : string
        //                          Number_Of_Likes : int
        //                          Number_Of_DisLikes : int     
        //                          Is_Public : bool
        //                          Number_Of_Visits : int
        //                          Date : DateTime
         */
        public async Task<bool> Update_Property_Data_Post_Data_By_Title_And_Property_Async(string? title,string? propertyUpdated, string? new_Values)
        {
            if(title == null || propertyUpdated == null || new_Values == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Title from Post Table!");
                return false;
            }
            Dictionary<string, Type> keyValuePairs = Post_Model.keyValuePairs();
            Type type_Property = keyValuePairs[propertyUpdated];
            bool is_Account_Updated = await _post_Table_Services.Update_Post_Data_String_By_Title_Async(null,title, propertyUpdated, new_Values, type_Property);
            if (is_Account_Updated == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Title from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Updated Post data by Title from Post Table successfully!");
                return is_Account_Updated;
            }
        }
        public async Task<bool> Update_Public_Mode_Post_Data_By_Post_ID_Async(int? Post_ID, bool? Is_Public)
        {
            if(Post_ID == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Post_ID from Post Table!");
                return false;
            }
            string propertyUpdated = "Is_Public";
            bool is_Account_Updated = await _post_Table_Services.Update_Post_Data_String_By_Title_Async(Post_ID , null, propertyUpdated, Is_Public.ToString(), typeof(bool));
            if (is_Account_Updated == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Post_ID from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Updated Post data by Post_ID from Post Table successfully!");
                return is_Account_Updated;
            }
        }
        /*   
         *  @ Update_Property_Data_Post_Data_By_Title_And_Property_Async Function
        //  List Property Names:    Title : string
        //                          Content : string
        //                          Author : string
        //                          Number_Of_Likes : int
        //                          Number_Of_DisLikes : int     
        //                          Is_Public : bool
        //                          Number_Of_Visits : int
        //                          Date : DateTime
         */
        public async Task<bool> Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(int? Post_ID, string? propertyUpdated, string? new_Values)
        {
            if (Post_ID == null || propertyUpdated == null || new_Values == null)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Post_ID from Post Table!");
                return false;
            }
            Dictionary<string, Type> keyValuePairs = Post_Model.keyValuePairs();
            Type type_Property = keyValuePairs[propertyUpdated];
            bool is_Account_Updated = await _post_Table_Services.Update_Post_Data_String_By_Title_Async(Post_ID, null, propertyUpdated, new_Values, type_Property);
            if (is_Account_Updated == false)
            {
                if (_logger != null)
                    _logger.LogError("Failed to update Post data by Post_ID from Post Table!");
                return false;
            }
            else
            {
                if (_logger != null)
                    _logger.LogInformation("Updated Post data by Post_ID from Post Table successfully!");
                return is_Account_Updated;
            }
        }
    }
}
