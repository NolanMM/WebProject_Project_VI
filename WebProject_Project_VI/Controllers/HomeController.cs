using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;

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

        private static string SessionID = String.Empty;

        private static List<Post_Model>? posts;

        private static int IdIncrement;

        public async Task<IActionResult> Index(string filter)
        {
            string _SessionID = GetUniqueKey(10);
            SessionID = _SessionID;
            string table_name = "post";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
            List<IData>? posts__ = await database_Services.Read_All_Data_By_Table_Name_Async(table_name, Username_Authorized, Password_Authorized);
            List<Post_Model> filteredPosts = posts__.ConvertAll(post => (Post_Model)post);

            if (posts == null)
            {
                posts = new List<Post_Model>();
            }
            else
            {
                posts.Clear();
                posts = new List<Post_Model>();
            }
            ViewBag.UserName = UserName;
            posts = filteredPosts;

            // Apply filtering based on the selected option
            switch (filter)
            {
                case "date":
                    filteredPosts = posts.OrderBy(p => p.Date).ToList();
                    break;
                case "views":
                    filteredPosts = posts.OrderByDescending(p => p.Number_Of_Visits).ToList();
                    break;
                case "likes":
                    filteredPosts = posts.OrderByDescending(p => p.Number_Of_Likes).ToList();
                    break;
                case "dislikes":
                    filteredPosts = posts.OrderByDescending(p => p.Number_Of_DisLikes).ToList();
                    break;
                default:
                    // Default sorting by date
                    filteredPosts = posts.OrderBy(p => p.Date).ToList();
                    break;
            }

            ViewData["Filter"] = filter; // Pass the selected filter to the view
            return View(filteredPosts);
        }

        public async Task<IActionResult> IncrementViews(int postId)
        {
            // Find the post in the list based on the postId
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);

            if (postToUpdate != null)
            {
                // Increment the view count for the specified post
                postToUpdate.Number_Of_Visits++;
                bool isUpdated = await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postToUpdate.PostId, "Number_Of_Visits", postToUpdate.Number_Of_Visits.ToString(), "int");
                if (isUpdated)
                {
                    return Json(new { success = true, viewsCount = postToUpdate.Number_Of_Visits });
                }
                else
                {
                    return Json(new { success = false, error = "Error updating the post" });
                }
            }
            else
            {
                // Handle the case where the post with the specified postId is not found
                return Json(new { success = false, error = "Post not found" });
            }
        }

        public async Task<IActionResult> IncrementLikes(int postId)
        {
            // Find the post in the list based on the postId
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);

            if (postToUpdate != null)
            {
                // Increment the like count for the specified post
                postToUpdate.Number_Of_Likes++;
                bool isUpdated = await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postToUpdate.PostId, "Number_Of_Likes", postToUpdate.Number_Of_Likes.ToString(), "int");
                if (isUpdated)
                {
                    return Json(new { success = true, likesCount = postToUpdate.Number_Of_Likes });
                }
                else
                {
                    return Json(new { success = false, error = "Error updating the post" });
                }
            }
            else
            {
                // Handle the case where the post with the specified postId is not found
                return Json(new { success = false, error = "Post not found" });
            }
        }

        public async Task<IActionResult> IncrementDislikes(int postId)
        {
            // Find the post in the list based on the postId
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);

            if (postToUpdate != null)
            {
                // Increment the like count for the specified post
                postToUpdate.Number_Of_DisLikes++;
                bool isUpdated = await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postToUpdate.PostId, "Number_Of_DisLikes", postToUpdate.Number_Of_DisLikes.ToString(), "int");
                return Json(new { success = true, dislikesCount = postToUpdate.Number_Of_DisLikes });
            }
            else
            {
                // Handle the case where the post with the specified postId is not found
                return Json(new { success = false, error = "Post not found" });
            }
        }

        public async Task<IActionResult> DeletePost(int postId)
        {
            // Lambda expression to find the post to delete
            var postToDelete = posts.FirstOrDefault(p => p.PostId == postId);

            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);

            // If the post is found -> then delete
            if (postToDelete != null)
            {
                bool isDeleted = await database_Services.Delete_Post_By_Title_Async(postToDelete.Title);
                posts.Remove(postToDelete);
                if (isDeleted)
                    return Json(new { success = true });
                else
                {
                    return Json(new { success = false, error = "Error deleting the post" });
                }
            }

            // Return error if can't delete the post.
            return Json(new { success = false, error = "Error deleting the post" });
        }

        public async Task<IActionResult> CreatePost(string authorName, string title, string content)
        {
            if (posts == null)
            {
                posts = new List<Post_Model>();
            }
            else
            {
                posts.Clear();
                posts = new List<Post_Model>();
            }
            string table_name = "post";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
            // Act
            List<IData>? posts__ = await database_Services.Read_All_Data_By_Table_Name_Async(table_name, Username_Authorized, Password_Authorized);
            List<Post_Model> filteredPosts = posts__.ConvertAll(post => (Post_Model)post);

            IdIncrement = filteredPosts.Count();
            IdIncrement++;

            authorName = UserName;

            var newPost = new Post_Model
            {
                PostId = IdIncrement,
                Author = authorName,
                Title = title,
                Date = DateTime.Now,
                Content = content,
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 0,
                Number_Of_Visits = 0,
                Is_Public = true
            };

            // Add the new post to the list
            posts.Add(newPost);

            // Add the new post to the database
            bool isCreated = await database_Services.Create_Post_Data_By_Model_Async_(newPost);
            // Redirect to the home page
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
        public async Task<IActionResult> UpdatePost(int postId, string content)
        {
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);
            var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);
            if (postToUpdate != null)
            {
                postToUpdate.Content = content;
                bool isUpdated = await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postToUpdate.PostId, "Content", postToUpdate.Content, "string");
                return RedirectToAction("Index");
            }
            return View("EditPost", new Post { PostId = postId, PostTitle = postToUpdate.Title, Content = content });
        }

        public async Task<IActionResult> FetchPost(int postId, string authorName, string content, string title, int likecount, int dislikecount, int viewcount, string date)
        {
            if (posts == null)
            {
                posts = new List<Post_Model>();
            }
            else
            {
                posts.Clear();
                posts = new List<Post_Model>();
            }
            string table_name = "post";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
            // Act
            List<IData>? posts__ = await database_Services.Read_All_Data_By_Table_Name_Async(table_name, Username_Authorized, Password_Authorized);
            List<Post_Model> filteredPosts = posts__.ConvertAll(post => (Post_Model)post);

            // check increment with postID
            if (postId <= IdIncrement)
            {
                postId = IdIncrement++;
            }
            DateTime Condate = DateTime.Parse(date);
            var newPost = new Post_Model
            {
                PostId = postId,
                Author = authorName,
                Title = title,
                Date = Condate,
                Content = content,
                Number_Of_Likes = likecount,
                Number_Of_DisLikes = dislikecount,
                Number_Of_Visits = viewcount,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Model_Async_(newPost);
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


        public IActionResult CreateAccount()
        {
            if (AccountSecured == true)
            {
                return RedirectToAction("Index", new { filter = "date" });
            }
            else
            {
                if (ViewBag.ErrorMessage == null)
                {
                    ViewBag.ErrorMessage = string.Empty;
                }
                else
                {
                    ViewBag.ErrorMessage = ViewBag.ErrorMessage;
                }
            }
            return View();
        }

        public async Task<IActionResult> CreateAccountValidate(string username, string authorname, string password)
        {

            // check to see if the account already exists or if the author name is already taken

            // if name not taken send the account info to the database
            // Redirect to the login after account is created

            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(_logger, SessionID);

            bool isCreated = await database_Services.Create_Account_Data_By_Passing_Values_Async(username, password, authorname);

            if (isCreated)
            {
                AccountSecured = false;
                ViewBag.ErrorMessage = "Account creation successful! Please login to create/edit post.";
                if (_logger != null)
                    _logger.LogInformation("Account creation successful! Please login to create/edit post.");
                return RedirectToAction("Login");
            }
            else
            {
                AccountSecured = false;
                ViewBag.ErrorMessage = "Account creation failed.Your Account already be Created!";
                if (_logger != null)
                    _logger.LogInformation("Account creation failed.Duplicated username");
                return View("CreateAccount");
            }
            //if name is taken redirect to the create account page so they can try again
            // return RedirectToAction("CreateAccount");
        }

        public IActionResult Login()
        {
            if (AccountSecured == true)
            {
                return RedirectToAction("Index", new { filter = "date" });
            }
            else
            {
                if (ViewBag.ErrorMessage == null)
                {
                    ViewBag.ErrorMessage = string.Empty;
                }
                else
                {
                    ViewBag.ErrorMessage = ViewBag.ErrorMessage;
                }
            }
            return View();
        }

        public async Task<IActionResult> LoginValidate(string username, string password)
        {
            WebSecurityServices webSecurityServices = new WebSecurityServices();
            bool isLoginSuccessful = await webSecurityServices.LoginAsync(username, password);

            if (isLoginSuccessful)
            {
                // Save the account name for use elsewhere
                UserName = username;

                // Change AccountSecured to true
                AccountSecured = true;

                // Assign the new username to ViewBag
                ViewBag.UserName = UserName;

                return RedirectToAction("Index", new { filter = "date" });
            }
            else
            {
                ViewBag.ErrorMessage = "Login failed. Please check your username and password.";
                return View("Login");
            }
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

        internal static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

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
    }
}
