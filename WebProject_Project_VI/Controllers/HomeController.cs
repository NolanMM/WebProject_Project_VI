using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static List<Post> posts = new List<Post>();

        private static int IdIncrement;

        public IActionResult Index()
        {
            return View(posts);
        }

        public IActionResult IncrementViews(int postId)
        {
            // Find the post in the list based on the postId
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

            if (postToUpdate != null)
            {
                // Increment the view count for the specified post
                postToUpdate.ViewCount++;
                return Json(new { success = true, viewsCount = postToUpdate.ViewCount });
            }
            else
            {
                // Handle the case where the post with the specified postId is not found
                return Json(new { success = false, error = "Post not found" });
            }
        }

        public IActionResult IncrementLikes(int postId)
        {
            // Find the post in the list based on the postId
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

            if (postToUpdate != null)
            {
                // Increment the like count for the specified post
                postToUpdate.LikeCount++;
                return Json(new { success = true, likesCount = postToUpdate.LikeCount });
            }
            else
            {
                // Handle the case where the post with the specified postId is not found
                return Json(new { success = false, error = "Post not found" });
            }
        }

        public IActionResult IncrementDislikes(int postId)
        {
            // Find the post in the list based on the postId
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

            if (postToUpdate != null)
            {
                // Increment the like count for the specified post
                postToUpdate.DislikeCount++;
                return Json(new { success = true, dislikesCount = postToUpdate.DislikeCount });
            }
            else
            {
                // Handle the case where the post with the specified postId is not found
                return Json(new { success = false, error = "Post not found" });
            }
        }

        public IActionResult CreatePost(string title, string content)
    {
        // Assuming you have a list of posts in your HomeController
        // Replace this with the actual list in your application
        // Create a new post


        IdIncrement++;

        var newPost = new Post
        {
            PostId = IdIncrement,
            AuthorName = "YourAuthorName", // Replace with the actual author's name
            PostTitle = title,
            DateTime = DateTime.Now.ToString(), // Set the current date and time
            Content = content,
            LikeCount = 0,
            DislikeCount = 0,
            ViewCount = 0
        };

        // Add the new post to the list
        posts.Add(newPost);

        // Redirect to the home page or wherever you want to go after the form submission
        return RedirectToAction("Index");
    }

        public IActionResult NewPost()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
