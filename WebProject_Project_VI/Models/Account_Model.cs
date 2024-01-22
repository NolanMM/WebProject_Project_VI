using System.ComponentModel.DataAnnotations;

namespace WebProject_Project_VI.Models
{
    public class Account_Model : IData
    {
        [Required]
        public string? Username { get; set; } = String.Empty;           // Username of the account
        [Required]
        public string? Password { get; set; } = String.Empty;           // Password of the account
        [Required]
        public string? AuthorName { get; set; } = String.Empty;         // Name of the author
        public static Dictionary<string, Type> keyValuePairs()
        {
            Type Account_Model_type = typeof(Account_Model);
            Dictionary<string, Type> keyValuePairs = Account_Model_type.GetProperties()
               .Where(p => p.Name != "Id")
               .ToDictionary(p => p.Name, p => p.PropertyType);
            return keyValuePairs;
        }
        public string[] Get_Property()
        {
            Type Account_Model_type = Get_Type();
            string[] propertyNames = Account_Model_type.GetProperties()
               .Where(p => p.Name != "Id")
               .Select(p => p.Name)
               .ToArray();
            return propertyNames;
        }
        public Type Get_Type()
        {
            return typeof(Account_Model);
        }
        public string toString()
        {
            return $"Username: {Username}\nPassword: {Password}\nAuthor Name: {AuthorName}";
        }
    }
}
