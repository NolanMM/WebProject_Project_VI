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

        public void toString()
        {
            Console.WriteLine($"Username: {Username}");
            Console.WriteLine($"Password: {Password}");
            Console.WriteLine($"Author Name: {AuthorName}");
        }
    }
}
