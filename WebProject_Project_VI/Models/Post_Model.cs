using System.ComponentModel.DataAnnotations;

namespace WebProject_Project_VI.Models
{
    public class Post_Model
    {
        [Required]
        public string? Title { get; set; } = String.Empty;          // Title of the post
        [Required]
        public string? Content { get; set; } = String.Empty;        // Content of the post
        [Required]
        public string? Author { get; set; } = String.Empty;         // Username of the author           **Note: Foreign key with Account_Model.AuthorName **
        public int? Number_Of_Likes { get; set; } = 0;              // Number of likes of the post
        public int? Number_Of_DisLikes { get; set; } = 0;           // Number of dislikes of the post
        public bool? Is_Public { get; set; } = false;               // Visibility of the post
        public int? Number_Of_Visits { get; set; } = 0;             // Number of visits of the post
        [Required]
        public DateTime? Date { get; set; }                         // Date of the post
    }
}
