using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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

        private static Boolean AccountSecured = false;

        private static List<Post> posts = new List<Post>();

        private static int IdIncrement; // check to see if this int is greater then the greatest int in the db or else making posts will break

        public IActionResult Index(string filter)
        {
            List<Post> filteredPosts;

            // Apply filtering based on the selected option
            switch (filter)
            {
                case "date":
                    filteredPosts = posts.OrderBy(p => p.DateTime).ToList();
                    break;
                case "views":
                    filteredPosts = posts.OrderByDescending(p => p.ViewCount).ToList();
                    break;
                case "likes":
                    filteredPosts = posts.OrderByDescending(p => p.LikeCount).ToList();
                    break;
                case "dislikes":
                    filteredPosts = posts.OrderByDescending(p => p.DislikeCount).ToList();
                    break;
                default:
                    // Default sorting by date
                    filteredPosts = posts.OrderBy(p => p.DateTime).ToList();
                    break;
            }

            ViewData["Filter"] = filter; // Pass the selected filter to the view
            return View(filteredPosts);
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

                IdIncrement++; // check top of page for comment on fixes / development needs

                var newPost = new Post
                {
                    PostId = IdIncrement,
                    AuthorName = "YourAuthorName", // Replace with the actual authors username from session file / viewbag file
                    PostTitle = title,
                    DateTime = DateTime.Now.ToString(),
                    Content = content,
                    LikeCount = 0,
                    DislikeCount = 0,
                    ViewCount = 0
                };

                // Add the new post to the list
                posts.Add(newPost);

                // Redirect to the home page or wherever you want to go after the form submission
                return RedirectToAction("Index", new { filter = "date" });
        }


        public IActionResult FetchPost(int postId, string authorName,  string content, string title, int likecount, int dislikecount, int viewcount, string date)
        {
            var newPost = new Post
            {
                PostId = postId,
                AuthorName = authorName,
                PostTitle = title,
                DateTime = date,
                Content = content,
                LikeCount = likecount,
                DislikeCount = dislikecount,
                ViewCount = viewcount
            };

            // Add the new post to the list
            posts.Add(newPost);

            // Redirect to the home page or wherever you want to go after the form submission
            return RedirectToAction("Index", new { filter = "date" });
        }

        public IActionResult NewPost()
        {

            if (AccountSecured == true)
            {
                return View();
            }
            else
            {
            // Redirect to the login page
            return RedirectToAction("Login");
            }
        }

        public IActionResult Login()
        {

            return View();
        }

        public IActionResult LoginValidate(string userName, string passWord)
        {

            //check to see if the account is valid 


            //save the account name as a viewbag / session file for use else where
            //change AccountSecured to true


            //if not Redirect them to try again


            // Redirect to the home page or wherever you want to go after the form submission
            return RedirectToAction("Index", new { filter = "date" });
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
