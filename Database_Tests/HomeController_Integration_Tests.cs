using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using WebProject_Project_VI.Controllers;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using WebProject_Project_VI.Services.Table_Services;

namespace Database_Tests
{
    [TestClass]
    public class HomeController_Integration_Tests
    {
        [TestMethod]
        public async Task Index_ReturnsViewWithFilteredPosts()
        {
            // Arrange
            var controller = HomeController.Get_Home_Instance;
            var expectedPosts = 6;

            // Act
            var result = await controller.Index("date");

            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            var model = ((ViewResult)result).Model;
            Assert.IsInstanceOfType(model, typeof(List<Post_Model>));
            Assert.AreEqual(expectedPosts, ((List<Post_Model>)model).Count);
        }

        [TestMethod]
        public async Task IncrementViews_IncrementsViewCount()
        {
            // Arrange
            string session_id = "TestHomeIncrementViews";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            var controller = HomeController.Get_Home_Instance;
            var postId = 1;
            var expectedViewsCount = 5;
            await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postId, "Number_Of_Visits", expectedViewsCount.ToString(), "int");
            // Act
            var result_index = await controller.Index("date");
            var result = await controller.IncrementViews(postId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            var data = ((JsonResult)result).Value;
            var data_string = data.ToString();
            string successPattern = @"success\s*=\s*(\w+)";
            string viewsCountPattern = @"viewsCount\s*=\s*(\d+)";

            System.Text.RegularExpressions.Match successMatch = Regex.Match(data_string, successPattern);
            System.Text.RegularExpressions.Match viewsCountMatch = Regex.Match(data_string, viewsCountPattern);
            Assert.AreEqual("True", successMatch.Groups[1].Value);
            expectedViewsCount++;
            Assert.AreEqual(expectedViewsCount.ToString(), viewsCountMatch.Groups[1].Value);
        }
        [TestMethod]
        public async Task IncrementLikes_IncrementsLikeCount()
        {
            // Arrange
            string session_id = "TestHomeIncrementLike";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            var controller = HomeController.Get_Home_Instance;
            var postId = 1;
            var expected = 5;
            await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postId, "Number_Of_Likes", expected.ToString(), "int");

            // Act
            var result_index = await controller.Index("date");
            var result = await controller.IncrementLikes(postId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            var data = ((JsonResult)result).Value;
            var data_string = data.ToString();
            string successPattern = @"success\s*=\s*(\w+)";
            string CountPattern = @"likesCount\s*=\s*(\d+)";

            System.Text.RegularExpressions.Match successMatch = Regex.Match(data_string, successPattern);
            System.Text.RegularExpressions.Match CountMatch = Regex.Match(data_string, CountPattern);
            Assert.AreEqual("True", successMatch.Groups[1].Value);
            expected++;
            Assert.AreEqual(expected.ToString(), CountMatch.Groups[1].Value);
        }
        [TestMethod]
        public async Task IncrementDislikes_IncrementsDislikeCount()
        {
            // Arrange
            string session_id = "TestHomeIncrementLike";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            var controller = HomeController.Get_Home_Instance;
            var postId = 1;
            var expected = 5;
            await database_Services.Update_Property_Data_Post_Data_By_Post_ID_And_Property_Async(postId, "Number_Of_DisLikes", expected.ToString(), "int");

            // Act
            var result_index = await controller.Index("date");
            var result = await controller.IncrementDislikes(postId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            var data = ((JsonResult)result).Value;
            var data_string = data.ToString();
            string successPattern = @"success\s*=\s*(\w+)";
            string CountPattern = @"dislikesCount\s*=\s*(\d+)";

            System.Text.RegularExpressions.Match successMatch = Regex.Match(data_string, successPattern);
            System.Text.RegularExpressions.Match CountMatch = Regex.Match(data_string, CountPattern);
            Assert.AreEqual("True", successMatch.Groups[1].Value);
            expected++;
            Assert.AreEqual(expected.ToString(), CountMatch.Groups[1].Value);
        }
        [TestMethod]
        public async Task DeletePost_RemovesPost()
        {
            // Arrange
            string session_id = "TestHomeIncrementLike";
            var postId = 1;
            var controller = HomeController.Get_Home_Instance;

            Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            Post_Table_Services Post_table_Services = new Post_Table_Services();
            Database_Services database_Services = new Database_Services();

            Post_table_Services.Set_Up_Post_Table_Services(session_id, configuration);
            database_Services.Set_Up_Database_Services(null, session_id);

            Post_Model? post_id_1_data = await Post_table_Services.Read_Post_Data_By_Post_ID_Async(postId);

            // Act
            var result_index = await controller.Index("date");
            var result = await controller.DeletePost(postId);

            bool recreate_post_data_1 = await database_Services.Create_Post_Data_By_Model_Async_(post_id_1_data); 
            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            var data = ((JsonResult)result).Value;
            var data_string = data.ToString();
            string successPattern = @"success\s*=\s*(\w+)";

            System.Text.RegularExpressions.Match successMatch = Regex.Match(data_string, successPattern);
            Assert.AreEqual("True", successMatch.Groups[1].Value);
        }
        [TestMethod]
        public async Task CreatePost_AddsNewPost()
        {
            // Arrange
            string session_id = "TestHomeIncrementLike";
            var controller = HomeController.Get_Home_Instance;
            string authorName = "TestAuthor_Create_Post";
            string title = "TestTitle_Create_Post";
            string Content = "TestContent";

            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            // Act
            var result_index = await controller.Index("date");
            var result = await controller.CreatePost(authorName, title, Content);

            bool delete_temp_post = await database_Services.Delete_Post_By_Title_Async(title);
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
        [TestMethod]
        public async Task LoginServiceTest()
        {
            // Arrange
            string session_id = "TestHomeLogin";
            var controller = HomeController.Get_Home_Instance;
            string username = "Liam";
            string password = "bob";

            var results = await controller.LoginValidate(username, password);
            Assert.IsInstanceOfType(results, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)results;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task LogOutServiceTest()
        {
            // Arrange
            string session_id = "TestHomeLogin";
            var controller = HomeController.Get_Home_Instance;
            string username = "Liam";
            string password = "bob";

            var results = await controller.LoginValidate(username, password);
            Assert.IsInstanceOfType(results, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)results;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);

            var results2 = controller.Logout();
            Assert.IsInstanceOfType(results2, typeof(RedirectToActionResult));
            var redirectToActionResult2 = (RedirectToActionResult)results2;
            Assert.AreEqual("Index", redirectToActionResult2.ActionName);

            bool account_signout = controller.GetAccountSecure();
            Assert.AreEqual(false, account_signout);
        }
    }
}
