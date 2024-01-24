using System.Collections.Generic;
using System.Linq;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Services
{
    public class PostFilterService
    {
        public List<Post> FilterPosts(List<Post> posts, string filter)
        {
            switch (filter)
            {
                case "date":
                    return posts.OrderBy(p => p.DateTime).ToList();
                case "views":
                    return posts.OrderByDescending(p => p.ViewCount).ToList();
                case "likes":
                    return posts.OrderByDescending(p => p.LikeCount).ToList();
                case "dislikes":
                    return posts.OrderByDescending(p => p.DislikeCount).ToList();
                default:
                    return posts.OrderBy(p => p.DateTime).ToList();
            }
        }
    }
}
