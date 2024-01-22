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
            post_services.Set_Up_Post_Table_Services(session_id, configuration);
            var result = await post_services.Create_Post_Data_By_Passing_Values_Async("Title", "Content", "Author", true);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Delete_Post_By_Title_Async_Success()
        {
            // Arrange
            var mock = new Mock<YourClass>();
            mock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<MySqlParameter[]>()))
                .ReturnsAsync(1);

            var yourClassInstance = mock.Object;

            // Act
            var result = await yourClassInstance.Delete_Post_By_Title_Async("title");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Read_Post_Data_By_Title_Async_Success()
        {
            // Arrange
            var mock = new Mock<YourClass>();
            mock.Setup(x => x.ExecuteQueryAsync<Post_Model>(It.IsAny<string>(), It.IsAny<MySqlParameter[]>()))
                .ReturnsAsync(new List<Post_Model> { new Post_Model { Title = "title", Content = "content", Author = "author" } });

            var yourClassInstance = mock.Object;

            // Act
            var result = await yourClassInstance.Read_Post_Data_By_Title_Async("title");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("title", result.Title);
        }

        [TestMethod]
        public async Task Read_Post_Data_By_Author_Async_Success()
        {
            // Arrange
            var mock = new Mock<YourClass>();
            mock.Setup(x => x.ExecuteQueryAsync<Post_Model>(It.IsAny<string>(), It.IsAny<MySqlParameter[]>()))
                .ReturnsAsync(new List<Post_Model> { new Post_Model { Title = "title", Content = "content", Author = "author" } });

            var yourClassInstance = mock.Object;

            // Act
            var result = await yourClassInstance.Read_Post_Data_By_Author_Async("author");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("title", result[0].Title);
        }

        [TestMethod]
        public async Task Update_Post_Data_String_By_Title_Async_Success()
        {
            // Arrange
            var mock = new Mock<YourClass>();
            mock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<MySqlParameter[]>()))
                .ReturnsAsync(1);

            var yourClassInstance = mock.Object;

            // Act
            var result = await yourClassInstance.Update_Post_Data_String_By_Title_Async("title", "property", "newValue", typeof(string));

            // Assert
            Assert.IsTrue(result);
        }
    }
}
