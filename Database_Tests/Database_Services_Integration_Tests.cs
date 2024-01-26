using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using WebProject_Project_VI;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using WebProject_Project_VI.Services.Table_Services;

namespace Database_Tests
{
    [TestClass]
    public class Database_Services_Integration_Tests
    {
        [TestMethod]
        public async Task TestReturnAllPostDatabaseServicesAsync()
        {
            // Arrange
            string session_id = "TestIntegrate1";
            string table_name = "post";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
        // Act
        List<IData>? posts = await database_Services.Read_All_Data_By_Table_Name_Async(table_name,Username_Authorized, Password_Authorized);
        List<Post_Model>? posts_ = posts.ConvertAll(post => (Post_Model)post);

        // Assert
        Assert.IsTrue(posts_.Count > 0,posts_.Count.ToString());
        }

        [TestMethod]
        public async Task TestReturnAllAccountDatabaseServicesAsync()
        {
            // Arrange
            string session_id = "TestIntegrate2";
            string table_name = "account";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "historyfellow@Account";
            string Password_Authorized = "HistoryFellow@Password";

            // Act
            List<IData>? accounts = await database_Services.Read_All_Data_By_Table_Name_Async(table_name, Username_Authorized, Password_Authorized);
            List<Account_Model>? accounts_ = accounts.ConvertAll(account => (Account_Model)account);

            // Assert
            Assert.IsTrue(accounts_.Count > 0, accounts_.Count.ToString());
        }

        [TestMethod]
        public async Task GetSessionID_Database_Services()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            string? actual_session_id = database_Services.Get_Session_Id();

            Assert.AreEqual(session_id, actual_session_id);
        }

        [TestMethod]
        public async Task ReadAccountByUsernameAsyncTest()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            string Username_Authorized = "Liam";

            Account_Model? account = await database_Services.Read_Account_Data_By_Username_Async(Username_Authorized);

            Assert.AreEqual(Username_Authorized, account.Username);
        }
        [TestMethod]
        public async Task CreateAccountByUsernameAsyncTest()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            string username = "TestIntegrate3";
            string password = "Hello";
            string author_name = "TestIntegrate3";

            bool is_Account_Created = await database_Services.Create_Account_Data_By_Passing_Values_Async(username, password, author_name);
            bool is_Account_Deleted = await database_Services.Delete_Account_Data_By_Username_Async(username);
            Assert.IsTrue(is_Account_Created);
        }
        [TestMethod]
        public async Task DeleteAccountByUsernameAsyncTest()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            string username = "TestIntegrate3";
            string password = "Hello";
            string author_name = "TestIntegrate3";

            bool is_Account_Created = await database_Services.Create_Account_Data_By_Passing_Values_Async(username, password, author_name);
            bool is_Account_Deleted = await database_Services.Delete_Account_Data_By_Username_Async(username);
            
            Assert.IsTrue(is_Account_Deleted);
        }
        [TestMethod]
        public async Task ReadPostByAuthorNameAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            string author_name = "Liam";

            var newPost = new Post_Model
            {
                PostId = 40,
                Author = author_name,
                Title = "Testing",
                Date = DateTime.Now,
                Content = "TestingContent",
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 1,
                Number_Of_Visits = 2,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Model_Async_(newPost);

            List<Post_Model>? posts = await database_Services.Read_Posts_By_Authors_Name_Async(author_name);

            bool isDeleted = await database_Services.Delete_Post_By_Title_Async(newPost.Title);
            Assert.IsTrue(posts.Count > 0);
        }
        [TestMethod]
        public async Task ReadPostByTitledAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            var newPost = new Post_Model
            {
                PostId = 40,
                Author = Username_Authorized,
                Title = "Testing",
                Date = DateTime.Now,
                Content = "TestingContent",
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 1,
                Number_Of_Visits = 2,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Model_Async_(newPost);

            Post_Model? posts = await database_Services.Read_Post_Data_By_Title_Async(newPost.Title);

            bool isDeleted = await database_Services.Delete_Post_By_Title_Async(newPost.Title);

            Assert.IsNotNull(posts);
        }
        [TestMethod]
        public async Task CreatePostByDataModelAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            var newPost = new Post_Model
            {
                PostId = 40,
                Author = Username_Authorized,
                Title = "Testing",
                Date = DateTime.Now,
                Content = "TestingContent",
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 1,
                Number_Of_Visits = 2,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Model_Async_(newPost);

            Post_Model? posts = await database_Services.Read_Post_Data_By_Title_Async(newPost.Title);

            bool isDeleted = await database_Services.Delete_Post_By_Title_Async(newPost.Title);

            Assert.IsTrue(isCreated);
            Assert.IsNotNull(posts);
        }
        [TestMethod]
        public async Task CreatePostByPassingValueAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            var newPost = new Post_Model
            {
                PostId = 40,
                Author = Username_Authorized,
                Title = "Testing",
                Date = DateTime.Now,
                Content = "TestingContent",
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 1,
                Number_Of_Visits = 2,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Passing_Values_Async(newPost.PostId, newPost.Title, newPost.Content, newPost.Author, newPost.Is_Public);

            Post_Model? posts = await database_Services.Read_Post_Data_By_Title_Async(newPost.Title);

            bool isDeleted = await database_Services.Delete_Post_By_Title_Async(newPost.Title);

            Assert.IsTrue(isCreated);
            Assert.IsNotNull(posts);
        }
        [TestMethod]
        public async Task UpdatePasswordAccountDataByUsernameAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            string username = "TestIntegrate3";
            string password = "Hello";
            string author_name = "TestIntegrate3";

            bool is_Account_Created = await database_Services.Create_Account_Data_By_Passing_Values_Async(username, password, author_name);
            bool is_Account_Updated = await database_Services.Update_Password_Account_Data_By_Username_Async(username, "Hello2");
            Account_Model? account_Model = await database_Services.Read_Account_Data_By_Username_Async(username);
            bool is_Account_Deleted = await database_Services.Delete_Account_Data_By_Username_Async(username);

            Assert.IsTrue(is_Account_Created);
            Assert.IsTrue(is_Account_Updated);
            Assert.AreEqual("Hello2", account_Model.Password);
        }
        [TestMethod]
        public async Task UpdateAuthorAccountDataByUsernameAsyncTest()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);

            string username = "TestIntegrate3";
            string password = "Hello";
            string author_name = "TestIntegrate3";

            bool is_Account_Created = await database_Services.Create_Account_Data_By_Passing_Values_Async(username, password, author_name);
            bool is_Account_Updated = await database_Services.Update_Author_Name_Account_Data_By_Username_Async(username, "TestIntegrateUpdate");
            Account_Model? account_Model = await database_Services.Read_Account_Data_By_Username_Async(username);
            bool is_Account_Deleted = await database_Services.Delete_Account_Data_By_Username_Async(username);

            Assert.IsTrue(is_Account_Created);
            Assert.IsTrue(is_Account_Updated);
            Assert.AreEqual("TestIntegrateUpdate", account_Model.AuthorName);
        }

        [TestMethod]
        public async Task UpdatePublicModePostDataByTitleAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            var newPost = new Post_Model
            {
                PostId = 40,
                Author = Username_Authorized,
                Title = "Testing",
                Date = DateTime.Now,
                Content = "TestingContent",
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 1,
                Number_Of_Visits = 2,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Passing_Values_Async(newPost.PostId, newPost.Title, newPost.Content, newPost.Author, newPost.Is_Public);
            bool isUpdated = await database_Services.Update_Public_Mode_Post_Data_By_Title_Async(newPost.Title, false);
            Post_Model? posts = await database_Services.Read_Post_Data_By_Title_Async(newPost.Title);

            bool isDeleted = await database_Services.Delete_Post_By_Title_Async(newPost.Title);

            Assert.IsTrue(isCreated);
            Assert.IsNotNull(posts);
            Assert.AreEqual(false, posts.Is_Public);
        }
        [TestMethod]
        public async Task UpdatePublicModePostDataByPostIDAsync()
        {
            string session_id = "TestIntegrate3";
            Database_Services database_Services = new Database_Services();
            database_Services.Set_Up_Database_Services(null, session_id);
            string Username_Authorized = "Liam";

            var newPost = new Post_Model
            {
                PostId = 40,
                Author = Username_Authorized,
                Title = "Testing",
                Date = DateTime.Now,
                Content = "TestingContent",
                Number_Of_Likes = 0,
                Number_Of_DisLikes = 1,
                Number_Of_Visits = 2,
                Is_Public = true
            };

            // Add the new post to the list
            bool isCreated = await database_Services.Create_Post_Data_By_Passing_Values_Async(newPost.PostId, newPost.Title, newPost.Content, newPost.Author, newPost.Is_Public);
            bool isUpdated = await database_Services.Update_Public_Mode_Post_Data_By_Post_ID_Async(newPost.PostId, false);
            Post_Model? posts = await database_Services.Read_Post_Data_By_Title_Async(newPost.Title);

            bool isDeleted = await database_Services.Delete_Post_By_Title_Async(newPost.Title);

            Assert.IsTrue(isCreated);
            Assert.IsNotNull(posts);
            Assert.AreEqual(false, posts.Is_Public);
        }
    }
}