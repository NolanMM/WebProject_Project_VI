using System.ComponentModel.DataAnnotations;

namespace WebProject_Project_VI.Models
{
    public class Post_Model : IData
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
        public static Dictionary<string, Type> keyValuePairs()
        {
            Type Post_Model_type = typeof(Post_Model);
            Dictionary<string, Type> keyValuePairs = Post_Model_type.GetProperties()
               .Where(p => p.Name != "Id")
               .ToDictionary(p => p.Name, p => p.PropertyType);
            return keyValuePairs;
        }
        public string[] Get_Property()
        {
            Type Post_Model_type = Get_Type();
            string[] propertyNames = Post_Model_type.GetProperties()
               .Where(p => p.Name != "Id")
               .Select(p => p.Name)
               .ToArray();
            return propertyNames;
        }
        public Type Get_Type()
        {
            return typeof(Post_Model);
        }
        public string toString()
        {
            return $"Title: {Title}\nContent: {Content}\nAuthor: {Author}\n" +
                   $"Likes: {Number_Of_Likes}\nDislikes: {Number_Of_DisLikes}\n" +
                   $"Public: {Is_Public}\nVisits: {Number_Of_Visits}\nDate: {Date}";
        }

    }
}
