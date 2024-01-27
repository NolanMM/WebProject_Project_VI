using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services.Table_Services;

namespace WebProject_Project_VI.Services
{
    public class WebSecurityServices : IWebSecurityServices
    {
        internal static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private string? UserName;

        public bool AccountSecured { get; private set; }

        public static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public string? GetUserName()
        {
            if(AccountSecured)
            return UserName;
            else
            {
                return null;
            }
        }

        public bool IsAdmin()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            return AccountSecured;
        }

        public bool IsUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string Session_Id = GetUniqueKey(16);
            Account_Table_Services accountTableServices = new Account_Table_Services();
            accountTableServices.Set_Up_Account_Table_Services(Session_Id, _configuration);

            Account_Model? account_by_username = await accountTableServices.Read_Account_Data_By_Username_Async(username) as Account_Model;
            if (account_by_username != null)
            {
                //string User = "liam";
                //string Pass = "bob";
                string User = string.Empty;
                string Pass = string.Empty;
                if (account_by_username.Username != null && account_by_username.Password != null)
                {
                    User = account_by_username.Username;
                    Pass = account_by_username.Password;
                }

                //check to see if the account is valid 
                if (username == User)
                {
                    if (password == Pass)
                    {
                        //save the account name as a viewbag / session file for use else where
                        UserName = username;

                        //change AccountSecured to true
                        AccountSecured = true;

                        // Redirect to the index or wherever you want to go after the form submission
                        return true;
                    }
                    else   //if not Redirect them to try again
                    {
                        // Redirect to theLogin or wherever you want to go after login fail
                        return false;
                    }
                }

                // Redirect to theLogin or wherever you want to go after login fail
                return false;
            }
            else
            {
                // Login fail
                return false;
            }

        }

        public bool Logout()
        {
            // change AccountSecured to false
            AccountSecured = false;
            return AccountSecured;
        }
    }
}
