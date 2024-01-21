using System.ComponentModel.DataAnnotations;

namespace WebProject_Project_VI.Models
{
    public class Account_Model
    {
        [Required]
        public string? Username { get; set; } = String.Empty;           // Username of the account
        [Required]
        public string? Password { get; set; } = String.Empty;           // Password of the account
        [Required]
        public string? AuthorName { get; set; } = String.Empty;         // Name of the author
    }
}
