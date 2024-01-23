using Microsoft.Extensions.Configuration;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using WebProject_Project_VI.Services.Table_Services;

namespace Database_Tests.Database_Services_Tests.Table_Services_Tests
{
    [TestClass]
    public class Account_Table_Service_Tests
    {
        [TestMethod]
        public void TestGetType()
        {
            // Arrange
            Account_Table_Services account_services = new Account_Table_Services();

            // Act
            Type expectedProperties = typeof(Account_Table_Services);
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
            Account_Table_Services account_services = new Account_Table_Services();
            string Expected_Return_Value = "test_session_id";

            // Act
            ITableServices? tableServices = account_services.SetUp(session_id, configuration);
            string? actual_Return_Value = account_services.Get_Session_Id();
            // Assert
            Assert.AreEqual(Expected_Return_Value, actual_Return_Value);
        }
        [TestMethod]
        public void TestSet_Up_Account_Table_Services()
        {
            // Arrange
            string session_id = "test_session_id_Set_Up";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Account_Table_Services account_services = new Account_Table_Services();
            string Expected_Return_Value = "test_session_id_Set_Up";

            // Act
            account_services.Set_Up_Account_Table_Services(session_id, configuration);
            string? actual_Return_Value = account_services.Get_Session_Id();
            // Assert
            Assert.AreEqual(Expected_Return_Value, actual_Return_Value);
        }
        [TestMethod]
        public async Task TestRead_All_Data_Async()
        {
            // Arrange
            string Username_Authorized = "historyfellow@Account";
            string Password_Authorized = "HistoryFellow@Password";
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);
            
            // Act
            List<IData>? datas = await account_services.Read_All_Data_Async(Username_Authorized, Password_Authorized);

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
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);

            // Act
            List<IData>? datas = await account_services.Read_All_Data_Async(Username_Authorized, Password_Authorized);

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
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);

            // Act
            List<IData>? datas = await account_services.Read_All_Data_Async(Username_Authorized, Password_Authorized);

            // Assert
            Assert.IsNull(datas);
        }
        [TestMethod]
        public async Task Create_Account_Data_By_Passing_Values_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);

            // Act
            bool result = await account_services.Create_Account_Data_By_Passing_Values_Async("usernameTest", "password", "author");
            var result_read = await account_services.Read_Account_Data_By_Username_Async("usernameTest") as Account_Model;
            bool result_delete = await account_services.Delete_Account_Data_By_Username_Async("usernameTest");

            // Assert
            Assert.AreEqual("password", result_read.Password);
        }

        [TestMethod]
        public async Task Delete_Account_Data_By_Username_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);

            // Act
            bool result = await account_services.Create_Account_Data_By_Passing_Values_Async("usernameTest", "password", "author");
            bool result_del = await account_services.Delete_Account_Data_By_Username_Async("usernameTest");

            // Assert
            Assert.IsTrue(result_del);
        }

        [TestMethod]
        public async Task Read_Account_Data_By_Username_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);
            bool result_write = await account_services.Create_Account_Data_By_Passing_Values_Async("UsernameCreate", "password_Created", "author");

            // Act
            var result = await account_services.Read_Account_Data_By_Username_Async("UsernameCreate") as Account_Model;
            bool result_delete = await account_services.Delete_Account_Data_By_Username_Async("UsernameCreate");

            // Assert
            //Assert.IsNotNull(result);
            Assert.AreEqual("password_Created", result.Password);
        }

        [TestMethod]
        public async Task Update_Account_Data_By_Username_Async_Success()
        {
            // Arrange
            string session_id = "test_session_id_Read_All_Data_Async";
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Account_Table_Services account_services = new Account_Table_Services();
            account_services.Set_Up_Account_Table_Services(session_id, configuration);
            bool result_write = await account_services.Create_Account_Data_By_Passing_Values_Async("UsernameUpdate", "password_Created", "author");

            // Act
            var result = await account_services.Update_Account_Data_By_Username_Async("UsernameUpdate", "Password", "password_Updated");
            var result_Read_After_Updated = await account_services.Read_Account_Data_By_Username_Async("UsernameUpdate") as Account_Model;
            bool result_delete = await account_services.Delete_Account_Data_By_Username_Async("UsernameUpdate");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("password_Updated", result_Read_After_Updated.Password);
        }
    }
}
