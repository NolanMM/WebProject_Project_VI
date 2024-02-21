using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using System.Net.Http;
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

        public static HomeController Get_Home_Instance
        {
            get
            {
                return new HomeController(new Logger<HomeController>(new LoggerFactory()), new ConfigurationBuilder().Build());
            }
        }

        private static Dictionary<string,(string, bool)> AccountSecured = new Dictionary<string, (string, bool)>();

        private static List<Post_Model>? backup_data;

        private static List<Post_Model>? posts;

        private static int IdIncrement;

        [HttpGet, HttpOptions]
        public async Task<IActionResult> Index(string filter)
        {
            string table_name = "post";
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
            List<IData>? posts__ = new List<IData>();

            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);
                posts__ = await database_Services.Read_All_Data_By_Table_Name_Async(table_name, Username_Authorized, Password_Authorized);
                List<Post_Model> filteredPosts = posts__.ConvertAll(post => (Post_Model)post);

                ViewBag.SessionId = SessionId;
                ViewBag.UserName = String.Empty;
                ViewData["Filter"] = filter; // Pass the selected filter to the view
                ViewBag.IsLoggedIn = false;

                AccountSecured.Add(SessionId, (String.Empty, false));

                if(filteredPosts != null)
                {
                    backup_data = filteredPosts;
                }
                else
                {
                    filteredPosts = backup_data;
                }

                if (posts == null)
                {
                    posts = new List<Post_Model>();
                }
                else
                {
                    posts.Clear();
                    posts = new List<Post_Model>();
                }

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
                return View(filteredPosts);
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];

                if(AccountSecured.ContainsKey(SessionId))
                {
                    ViewBag.UserName = AccountSecured[SessionId].Item1;
                    ViewBag.IsLoggedIn = AccountSecured[SessionId].Item2;
                }
                else
                {
                    ViewBag.UserName = String.Empty;
                    ViewBag.IsLoggedIn = false;
                }

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);
                posts__ = await database_Services.Read_All_Data_By_Table_Name_Async(table_name, Username_Authorized, Password_Authorized);
                List<Post_Model> filteredPosts = posts__.ConvertAll(post => (Post_Model)post);

                ViewData["Filter"] = filter; // Pass the selected filter to the view
                ViewBag.SessionId = SessionId;

                if (filteredPosts != null)
                {
                    backup_data = filteredPosts;
                }
                else
                {
                    filteredPosts = backup_data;
                }

                if (posts == null)
                {
                    posts = new List<Post_Model>();
                }
                else
                {
                    posts.Clear();
                    posts = new List<Post_Model>();
                }

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
                return View(filteredPosts);
            }
        }
        [HttpPatch]
        public async Task<IActionResult> IncrementViews(int postId)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);

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
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                // Find the post in the list based on the postId
                var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);

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
        }
        [HttpPost]
        public async Task<IActionResult> IncrementLikes(int postId)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));

                // Find the post in the list based on the postId
                var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);

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
            else
            {
                SessionId = this.Request.Cookies["search_key"];

                // Find the post in the list based on the postId
                var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);

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
        }
        [HttpPut]
        public async Task<IActionResult> IncrementDislikes(int postId)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));

                // Find the post in the list based on the postId
                var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);

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
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                // Find the post in the list based on the postId
                var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);

                Database_Services database_Services = new Database_Services();
                database_Services.Set_Up_Database_Services(_logger, SessionId);

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
                
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int postId)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];

                if(AccountSecured.ContainsKey(SessionId))
                {
                    if (AccountSecured[SessionId].Item2 == false)
                    {
                        return Json(new { success = false, error = "Error deleting the post" });
                    }
                    else
                    {
                        // Lambda expression to find the post to delete
                        var postToDelete = posts.FirstOrDefault(p => p.PostId == postId);

                        Database_Services database_Services = new Database_Services();
                        database_Services.Set_Up_Database_Services(_logger, SessionId);

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
                    }
                }
                else
                {
                    return Json(new { success = false, error = "Error deleting the post" });
                }
            }
            // Return error if can't delete the post.
            return Json(new { success = false, error = "Error deleting the post" });
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(string authorName, string title, string content)
        {

            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                if (posts == null)
                {
                    posts = new List<Post_Model>();
                }
                else
                {
                    posts.Clear();
                    posts = new List<Post_Model>();
                }

                if(AccountSecured.ContainsKey(SessionId))
                {
                    if (AccountSecured[SessionId].Item2 == false)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        string Username = AccountSecured[SessionId].Item1;
                        Database_Services database_Services = new Database_Services();
                        database_Services.Set_Up_Database_Services(_logger, SessionId);
                        Post_Table_Services Post_Table_Services = new Post_Table_Services();
                        Post_Table_Services.Set_Up_Post_Table_Services(SessionId, _configuration);

                        IdIncrement = await Post_Table_Services.Get_Currently_Greatest_Post_ID_In_Post_Table_Async();
                        IdIncrement++;

                        authorName = Username;

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
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

        public IActionResult EditPost(int postId)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                if(AccountSecured.ContainsKey(SessionId))
                {
                    if (AccountSecured[SessionId].Item2 == false)
                    {
                        return RedirectToAction("Login");
                    }
                    var postToEdit = posts.FirstOrDefault(p => p.PostId == postId);
                    if (postToEdit != null)
                    {
                        return View(postToEdit);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

        //  This action is needed to update an existing post after editing. 
        [HttpPost]
        public async Task<IActionResult> UpdatePost(int postId, string content)
        {
            string SessionId = Guid.NewGuid().ToString();
            if(!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                if(AccountSecured.ContainsKey(SessionId))
                {
                    if (AccountSecured[SessionId].Item2 == false)
                    {
                        return RedirectToAction("Login");
                    }else
                    {
                        Database_Services database_Services = new Database_Services();
                        database_Services.Set_Up_Database_Services(_logger, SessionId);
                        var postToUpdate = posts.FirstOrDefault(p => p.PostId == postId);
                        if (postToUpdate != null)
                        {
                            postToUpdate.Content = content;
                            bool isUpdated = await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postToUpdate.PostId, "Content", postToUpdate.Content, "string");
                            return RedirectToAction("Index");
                        }
                        return View("EditPost", new Post { PostId = postId, Content = content });
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

        public async Task<IActionResult> FetchPost(int postId, string authorName, string content, string title, int likecount, int dislikecount, int viewcount, string date)
        {
            string SessionId = Guid.NewGuid().ToString();
            string table_name = "post";
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                if(AccountSecured.ContainsKey(SessionId))
                {
                    if (AccountSecured[SessionId].Item2 == false)
                    {
                        return RedirectToAction("Login");
                    }
                    else
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
                        Database_Services database_Services = new Database_Services();
                        database_Services.Set_Up_Database_Services(_logger, SessionId);

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
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

        public IActionResult NewPost()
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                if(AccountSecured.ContainsKey(SessionId))
                {
                    bool flag = AccountSecured[SessionId].Item2;
                    if (flag == true)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }else
                {
                    return RedirectToAction("Login");
                }
            }
        }


        public IActionResult CreateAccount()
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));
                if (ViewBag.ErrorMessage == null)
                {
                    ViewBag.ErrorMessage = string.Empty;
                }
                else
                {
                    ViewBag.ErrorMessage = ViewBag.ErrorMessage;
                }
                return View();
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];

                if(AccountSecured.ContainsKey(SessionId))
                {
                    bool flag = AccountSecured[SessionId].Item2;
                    if (flag == true)
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
                        return View();
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> CreateAccountValidate(string username, string authorname, string password)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                ViewBag.ErrorMessage = "Account creation failed.";
                if (_logger != null)
                    _logger.LogInformation("No Session ID when create account");
                return View("CreateAccount");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];

                // check to see if the account already exists or if the author name is already taken

                // if name not taken send the account info to the database
                // Redirect to the login after account is created
                if(AccountSecured.ContainsKey(SessionId))
                {

                    Database_Services database_Services = new Database_Services();
                    database_Services.Set_Up_Database_Services(_logger, SessionId);

                    bool isCreated = await database_Services.Create_Account_Data_By_Passing_Values_Async(username, password, authorname);

                    if (isCreated)
                    {
                        AccountSecured[SessionId] = (username, false);
                        ViewBag.ErrorMessage = "Account creation successful! Please login to create/edit post.";
                        if (_logger != null)
                            _logger.LogInformation("Account creation successful! Please login to create/edit post.");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        AccountSecured.Remove(SessionId);
                        ViewBag.ErrorMessage = "Account creation failed.Your Account already be Created!";
                        if (_logger != null)
                            _logger.LogInformation("Account creation failed.Duplicated username");
                        return View("CreateAccount");
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Account creation failed.";
                    if (_logger != null)
                        _logger.LogInformation("No Session ID when create account");
                    return View("CreateAccount");
                }
            }
        }

        public IActionResult Login()
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                this.Response.Cookies.Append("search_key", SessionId);
                AccountSecured.Add(SessionId, (String.Empty, false));

                if (ViewBag.ErrorMessage == null)
                {
                    ViewBag.ErrorMessage = string.Empty;
                }
                else
                {
                    ViewBag.ErrorMessage = ViewBag.ErrorMessage;
                }
                
                return View();
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                bool flag = false;

                if(AccountSecured.ContainsKey(SessionId))
                {
                    flag = AccountSecured[SessionId].Item2;
                }
                if (flag == true)
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
        }

        public async Task<IActionResult> LoginValidate(string username, string password)
        {
            string SessionId = Guid.NewGuid().ToString();
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                return RedirectToAction("Login");
            }
            else
            {
                SessionId = this.Request.Cookies["search_key"];
                WebSecurityServices webSecurityServices = new WebSecurityServices();
                bool isLoginSuccessful = await webSecurityServices.LoginAsync(username, password);
                if (isLoginSuccessful)
                {
                    if(AccountSecured.ContainsKey(SessionId))
                    {
                        AccountSecured[SessionId] = (username, true);
                    }
                    // Assign the new username to ViewBag
                    ViewBag.UserName = username;

                    return RedirectToAction("Index", new { filter = "date" });
                }
                else
                {
                    ViewBag.ErrorMessage = "Login failed. Please check your username and password.";
                    return View("Login");
                }
            }
        }

        public IActionResult Logout()
        {
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                return RedirectToAction("Index", new { filter = "date" });
            }else
            {
                string SessionId = this.Request.Cookies["search_key"];
                if (AccountSecured.ContainsKey(SessionId))
                {
                    AccountSecured.Remove(SessionId);
                }
                return RedirectToAction("Index", new { filter = "date" });
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

        public bool GetAccountSecure()
        {
            if (!this.Request.Cookies.ContainsKey("search_key"))
            {
                return false;
            }
            else
            {
                string SessionId = this.Request.Cookies["search_key"];
                if(AccountSecured.ContainsKey(SessionId))
                {
                    return AccountSecured[SessionId].Item2;
                }
                else
                {
                    return false;
                }
            }
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
