using Microsoft.Extensions.Configuration;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using WebProject_Project_VI.Services.Table_Services;

namespace Database_Tests.Database_Services_Tests.Table_Services_Tests
{
    [TestClass]
    public class Post_Table_Service_Tests
    {
        [TestMethod]
        public void TestGetType()
        {
            // Arrange
            Post_Table_Services account_services = new Post_Table_Services();

            // Act
            Type expectedProperties = typeof(Post_Table_Services);
            Type actualProperties = account_services.Get_Type();

            // Assert
            Assert.AreEqual(expectedProperties, actualProperties);
        }

        [TestMethod]
        public void TestSetUp()
        {
            // Arrange
            string session_id = "test_session_id";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            string Expected_Return_Value = "test_session_id";

            // Act
            ITableServices? tableServices = post_services.SetUp(session_id, configuration);
            string? actual_Return_Value = post_services.Get_Session_Id();
            // Assert
            Assert.AreEqual(Expected_Return_Value, actual_Return_Value);
        }
        [TestMethod]
        public void TestSet_Up_Post_Table_Services()
        {
            // Arrange
            string session_id = "test_session_id_Set_Up";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            string Expected_Return_Value = "test_session_id_Set_Up";

            // Act
            post_services.Set_Up_Post_Table_Services(session_id, configuration);
            string? actual_Return_Value = post_services.Get_Session_Id();
            // Assert
            Assert.AreEqual(Expected_Return_Value, actual_Return_Value);
        }
        [TestMethod]
        public async Task TestRead_All_Data_Async()
        {
            // Arrange
            string Username_Authorized = "historyfellow@Post";
            string Password_Authorized = "HistoryFellow@Password";
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            // Act
            List<IData>? datas = await post_services.Read_All_Data_Async(Username_Authorized, Password_Authorized);

            // Assert
            Assert.IsNotNull(datas);
        }

        [TestMethod]
        public async Task TestRead_All_Data_Missing_Username_Async()
        {
            // Arrange
            string Username_Authorized = String.Empty;
            string Password_Authorized = "HistoryFellow@Password";
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            // Act
            List<IData>? datas = await post_services.Read_All_Data_Async(Username_Authorized, Password_Authorized);

            // Assert
            Assert.IsNull(datas);
        }
        [TestMethod]
        public async Task TestRead_All_Data_Missing_Password_Async()
        {
            // Arrange
            string Username_Authorized = "historyfellow@Account";
            string Password_Authorized = String.Empty;
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            // Act
            List<IData>? datas = await post_services.Read_All_Data_Async(Username_Authorized, Password_Authorized);

            // Assert
            Assert.IsNull(datas);
        }

        [TestMethod]
        public async Task Create_Post_Data_By_Passing_Values_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Set_Up";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();

            // Act
            bool result_del = await post_services.Delete_Post_By_Post_ID_Async(10);
            post_services.Set_Up_Post_Table_Services(session_id, configuration);
            var result = await post_services.Create_Post_Data_By_Passing_Values_Async(10, "TitleTestCreate", "ContentTestCreate", "AuthorTestCreate", true);
            result_del = await post_services.Delete_Post_By_Post_ID_Async(10);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(result_del);
        }

        [TestMethod]
        public async Task Delete_Post_By_Title_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            // Act
            var result = await post_services.Create_Post_Data_By_Passing_Values_Async(11, "TitleTestDelete", "ContentTestDelete", "AuthorTestDelete", true);
            bool result_del = await post_services.Delete_Post_By_Title_Async("TitleTestDelete");

            // Assert
            Assert.IsTrue(result_del);
        }

        [TestMethod]
        public async Task Read_Post_Data_By_Title_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(12, "TitleTestRead", "ContentTestRead", "AuthorTestRead", true);

            // Act
            var result = await post_services.Read_Post_Data_By_Title_Async("TitleTestRead") as Post_Model;
            bool result_delete = await post_services.Delete_Post_By_Title_Async("TitleTestRead");

            // Assert
            Assert.AreEqual("ContentTestRead", result.Content);
            Assert.AreEqual("AuthorTestRead", result.Author);
        }

        [TestMethod]
        public async Task Read_Post_Data_By_Author_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);
            int number_of_posts = 3;

            // Act
            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(13, "TitleTestRead", "ContentTestRead", "AuthorTestRead", true);
            var result_write_1 = await post_services.Create_Post_Data_By_Passing_Values_Async(14, "TitleTestRead1", "ContentTestRead", "AuthorTestRead", true);
            var result_write_2 = await post_services.Create_Post_Data_By_Passing_Values_Async(15, "TitleTestRead2", "ContentTestRead", "AuthorTestRead", true);

            var result = await post_services.Read_Post_Data_By_Author_Async("AuthorTestRead");
            List<Post_Model> result_list = result.ConvertAll(post => (Post_Model)post);

            bool result_delete = await post_services.Delete_Post_By_Title_Async("TitleTestRead");
            bool result_delete_1 = await post_services.Delete_Post_By_Title_Async("TitleTestRead1");
            bool result_delete_2 = await post_services.Delete_Post_By_Title_Async("TitleTestRead2");

            // Assert
            Assert.AreEqual(number_of_posts, result_list.Count);
        }

        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_StringType_Content_Success()
        {
            // Arrange
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitle";
            string propertyUpdated = "Content";
            string newValue = "ContentTestUpdateSuccess";
            Type propertyType = typeof(string);

            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(16, "SampleTitle", "ContentTestUpdate", "AuthorTestRead", true);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("ContentTestUpdateSuccess", result_read.Content);
        }
        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_StringType_Author_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitleTestAuthor";
            string propertyUpdated = "Author";
            string newValue = "AuthorTestUpdateSuccess";
            Type propertyType = typeof(string);

            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(17, "SampleTitleTestAuthor", "ContentTestUpdate", "AuthorTestUpdate", true);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("AuthorTestUpdateSuccess", result_read.Author);
        }
        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_StringType_Title_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitleTestTitle";
            string propertyUpdated = "Title";
            string newValue = "TitleTestUpdateSuccess";
            Type propertyType = typeof(string);

            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(18, "SampleTitleTestTitle", "ContentTestUpdate", "AuthorTestUpdate", true);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(newValue);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(newValue);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("TitleTestUpdateSuccess", result_read.Title);
        }
        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_IntType_Number_Of_Likes_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitle_Int";
            string propertyUpdated = "Number_Of_Likes";
            string newValue = "123";
            Type propertyType = typeof(int);
            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(19, "SampleTitle_Int", "ContentTestUpdate", "AuthorTestUpdate", true);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(123, result_read.Number_Of_Likes);
        }
        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_IntType_Number_Of_DisLikes_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitle_IntDis";
            string propertyUpdated = "Number_Of_DisLikes";
            string newValue = "1203";
            Type propertyType = typeof(int);
            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(20, "SampleTitle_IntDis", "ContentTestUpdate", "AuthorTestUpdate", true);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1203, result_read.Number_Of_DisLikes);
        }
        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_IntType_Number_Of_Visits_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitle_IntVis";
            string propertyUpdated = "Number_Of_Visits";
            string newValue = "12340";
            Type propertyType = typeof(int);
            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(21, "SampleTitle_IntVis", "ContentTestUpdate", "AuthorTestUpdate", true);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(12340, result_read.Number_Of_Visits);
        }

        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_BoolType_Is_Public_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitleBoolean";
            string propertyUpdated = "Is_Public";
            string newValue = "true"; // A valid boolean string
            Type propertyType = typeof(bool);
            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(22, "SampleTitleBoolean", "ContentTestUpdate", "AuthorTestUpdate", false);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result_read.Is_Public);
        }

        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_DateTimeType_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            string title = "SampleTitleDate";
            string propertyUpdated = "Date";
            string newValue = "2024-01-21 11:38:07"; // A valid date string
            Type propertyType = typeof(DateTime);
            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(23, "SampleTitleDate", "ContentTestUpdate", "AuthorTestUpdate", false);

            // Act
            bool result = await post_services.Update_Post_Data_String_By_Title_Async(null, title, propertyUpdated, newValue, propertyType);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Title_Async(title);
            bool result_delete = await post_services.Delete_Post_By_Title_Async(title);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(DateTime.Parse(newValue), result_read.Date);
        }

        [TestMethod]
        public async Task Update_Post_Data_By_Post_Model_Async_Should_Update_Data()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Post_Table_Services post_services = new Post_Table_Services();
            post_services.Set_Up_Post_Table_Services(session_id, configuration);

            var result_write = await post_services.Create_Post_Data_By_Passing_Values_Async(29, "SampleTitleBoolean", "ContentTestUpdate", "AuthorTestUpdate", false);

            var postIdToUpdate = 29;
            var newPostData = new Post_Model
            {
                PostId = postIdToUpdate,
                Title = "Updated Title",
                Content = "Updated Content",
                Number_Of_Likes = 10,
                Number_Of_DisLikes = 5,
                Is_Public = true,
                Number_Of_Visits = 20,
                Date = DateTime.Now
            };

            // Act
            bool result = await post_services.Update_Post_Data_By_Post_Model_Async(newPostData);
            Post_Model? result_read = await post_services.Read_Post_Data_By_Post_ID_Async(postIdToUpdate);
            bool result_delete = await post_services.Delete_Post_By_Post_ID_Async(postIdToUpdate);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newPostData.Title, result_read.Title);
            Assert.AreEqual(newPostData.Content, result_read.Content);
        }
    }
}
