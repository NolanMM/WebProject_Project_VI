using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using WebProject_Project_VI.Services.Table_Services;

namespace WebProject_Project_VI.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController>? _logger { get; set; }
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _logger.LogInformation("Created HomeController && IConfiguration constructor successfully!");
        }

        private static Boolean AccountSecured = false;

        private static string? UserName = String.Empty;

        private static List<Post> posts = new List<Post>();

        private static int IdIncrement;

        public IActionResult Index(string filter)
        {
            List<Post> filteredPosts;

            ViewBag.UserName = UserName;

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

        public IActionResult DeletePost(int postId)
        {
            // Lambda expression to find the post to delete
			var postToDelete = posts.FirstOrDefault(p => p.PostId == postId);

            // If the post is found -> then delete
			if (postToDelete != null)
			{
				posts.Remove(postToDelete);
                return Json(new { success = true });
            }

            // Return error if can't delete the post.
			return Json(new { success = false, error = "Error deleting the post" });
		}

        public IActionResult CreatePost(string authorName, string title, string content)
        {

                IdIncrement++;
                authorName = UserName;

                var newPost = new Post
                {
                    PostId = IdIncrement,
                    AuthorName = authorName, // Replace with the actual authors username from session file / viewbag file
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

        public IActionResult EditPost(int postId)
        {
            var postToEdit = posts.FirstOrDefault(p => p.PostId == postId);
            if (postToEdit != null)
            {
                return View(postToEdit);
            }
            return RedirectToAction("Index");
        }

        //  This action is needed to update an existing post after editing. 
        public IActionResult UpdatePost(int postId, string title, string content)
        {
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);
            if (postToUpdate != null)
            {
                postToUpdate.PostTitle = title;
                postToUpdate.Content = content;
                // Update other fields if necessary
                return RedirectToAction("Index");
            }
            return View("EditPost", new Post { PostId = postId, PostTitle = title, Content = content });
        }

        public IActionResult FetchPost(int postId, string authorName,  string content, string title, int likecount, int dislikecount, int viewcount, string date)
        {

            // check increment with postID
            if (IdIncrement <= postId)
            {
                IdIncrement = postId;
            }

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

        public IActionResult LoginValidate(string username, string password)
        {


            string User = "liam";
            string Pass = "bob";

            //check to see if the account is valid 
            if (username == User)
            {
                if (password == Pass)
                {
                    //save the account name as a viewbag / session file for use else where
                    UserName = username;

                    //change AccountSecured to true
                    AccountSecured = true;

                    //Assign the new username to ViewBag
                    ViewBag.UserName = UserName;

                    // Redirect to the index or wherever you want to go after the form submission
                    return RedirectToAction("Index", new { filter = "date" });
                }
                else   //if not Redirect them to try again
                {
                    // Redirect to theLogin or wherever you want to go after login fail
                    return RedirectToAction("Login");
                }
            }
           
                // Redirect to theLogin or wherever you want to go after login fail
                return RedirectToAction("Login");
            
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
