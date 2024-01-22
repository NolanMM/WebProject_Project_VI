﻿using WebProject_Project_VI.Controllers;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services.Table_Services;

namespace WebProject_Project_VI.Services
{
    public class Database_Services
    {
        private  ILogger<HomeController>? _logger;
        private  IConfiguration? _configuration;

        private Database_Services? _instance;
        private Account_Table_Services _account_Table_Services = new Account_Table_Services();
        private Post_Table_Services _post_Table_Services = new Post_Table_Services();

        private string? SesionID;
        private readonly string Account_Table_Name = "account";
        private readonly string Post_Table_Name = "post";
        public Database_Services Set_Up_Database_Services(ILogger<HomeController>? logger, IConfiguration? configuration, string? SessionID)
        {
            _instance = new Database_Services();
            if (logger == null || configuration == null || SessionID == null)
            {
                _logger.LogError("Failed to create Database Services constructor!");
                return null;
            }
            _logger = logger;
            _configuration = configuration;
            SesionID = SessionID;
            _account_Table_Services = _account_Table_Services.Set_Up_Account_Table_Services(SessionID, configuration);
            _post_Table_Services = _post_Table_Services.Set_Up_Post_Table_Services(SessionID, configuration);
            _logger.LogInformation($"Created Database Services constructor for SessionID: {SessionID} successfully!");
            return _instance;
        }

        public async Task<List<IData>?> Read_All_Data_By_Table_Name_Async(string? table_name,string? username_account_authorized, string? password_account_authorized)
        {
            if(table_name == Account_Table_Name)
            {
                List<IData>? account_Models = await _account_Table_Services.Read_All_Data_Async(username_account_authorized, password_account_authorized);
                if (account_Models == null)
                {
                    _logger.LogError("Failed to read all data from Account Table!");
                    return null;
                }
                else
                {
                    return account_Models;
                }
            }else
            {
                List<IData>? post_Models = await _post_Table_Services.Read_All_Data_Async(username_account_authorized, password_account_authorized);
                if (post_Models == null)
                {
                    _logger.LogError("Failed to read all data from Post Table!");
                    return null;
                }
                else
                {
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
                    _logger.LogError("Failed to read all data from Account Table!");
                    return null;
                }
                else
                {
                    return is_Table_Empty;
                }
            }
            else
            {
                bool is_Table_Empty = await _post_Table_Services.Delete_All_Data_Async(username_account_authorized, password_account_authorized);
                if (is_Table_Empty == false)
                {
                    _logger.LogError("Failed to read all data from Post Table!");
                    return null;
                }
                else
                {
                    return is_Table_Empty;
                }
            }

        }
        public async Task<Account_Model?> Read_Account_Data_By_Username_Async(string? username)
        {
            Account_Model? account_Model = await _account_Table_Services.Read_Account_Data_By_Username_Async(username) as Account_Model;
            if (account_Model == null)
            {
                _logger.LogError("Failed to read account data by username from Account Table!");
                return null;
            }
            return account_Model;
        }
        public async Task<bool> Create_Account_Data_By_Passing_Values_Async(string? username, string? password, string? author_name)
        {
            bool is_Account_Created = await _account_Table_Services.Create_Account_Data_By_Passing_Values_Async(username, password, author_name);
            if (is_Account_Created == false)
            {
                _logger.LogError("Failed to create account data by passing values from Account Table!");
                return false;
            }
            else
            {
                return is_Account_Created;
            }
        }
        public async Task<bool> Delete_Account_Data_By_Username_Async(string? username)
        {
            bool is_Account_Deleted = await _account_Table_Services.Delete_Account_Data_By_Username_Async(username);
            if (is_Account_Deleted == false)
            {
                _logger.LogError("Failed to delete account data by username from Account Table!");
                return false;
            }
            else
            {
                return is_Account_Deleted;
            }
        }
        public async Task<List<Post_Model>?> Read_Posts_By_Authors_Name_Async(string? authors_name)
        {
            List<IData>? post_Models = await _post_Table_Services.Read_Post_Data_By_Author_Async(authors_name);
            if (post_Models == null)
            {
                _logger.LogError("Failed to read posts by authors name from Post Table!");
                return null;
            }
            else
            {
                List<Post_Model>? post_Models_Return = post_Models.ConvertAll(post => (Post_Model)post);
                return post_Models_Return;
            }
        }
        public async Task<bool> Update_Password_Account_Data_By_Username_Async(string? username, string? new_Password_Value)
        {
            string propertyUpdated = "Password";
            bool is_Account_Updated = await _account_Table_Services.Update_Account_Data_By_Username_Async(username, propertyUpdated, new_Password_Value);
            if (is_Account_Updated == false)
            {
                _logger.LogError("Failed to update account data by username from Account Table!");
                return false;
            }
            else
            {
                return is_Account_Updated;
            }
        }

        public async Task<bool> Update_Author_Name_Account_Data_By_Username_Async(string? username, string? new_Author_Name_Value)
        {
            string propertyUpdated = "AuthorName";
            bool is_Account_Updated = await _account_Table_Services.Update_Account_Data_By_Username_Async(username, propertyUpdated, new_Author_Name_Value);
            if (is_Account_Updated == false)
            {
                _logger.LogError("Failed to update account data by username from Account Table!");
                return false;
            }
            else
            {
                return is_Account_Updated;
            }
        }
    }
}
