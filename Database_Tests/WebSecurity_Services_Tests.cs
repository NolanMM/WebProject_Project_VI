using Moq;
using Microsoft.Extensions.Configuration;
using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;
using WebProject_Project_VI.Services.Table_Services;

namespace Database_Tests
{
    [TestClass]
    public class WebSecurityServicesTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<Account_Table_Services> _mockAccountTableServices;
        private readonly WebSecurityServices _webSecurityServices;

        public WebSecurityServicesTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockAccountTableServices = new Mock<Account_Table_Services>();
            _webSecurityServices = new WebSecurityServices();
        }

        [TestMethod]
        public void GetUniqueKey_ShouldReturnStringOfGivenSize()
        {
            var key = WebSecurityServices.GetUniqueKey(10);
            Assert.AreEqual(10, key.Length);
        }

        [TestMethod]
        public async Task LoginAsync_ValidCredentials_ShouldReturnTrue()
        {
            var username = "Liam";
            var password = "bob";
            var mockAccount = new Account_Model { Username = username, Password = password };

            //_mockAccountTableServices.Setup(s => s.Read_Account_Data_By_Username_Async(It.IsAny<string>()))
            //    .ReturnsAsync(mockAccount);

            var result = await _webSecurityServices.LoginAsync(username, password);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task LoginAsync_InvalidCredentials_ShouldReturnFalse()
        {
            var username = "Someone";
            var password = "Hello";

            var result = await _webSecurityServices.LoginAsync(username, password);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Logout_ShouldSetAccountSecuredToFalse()
        {
            _webSecurityServices.Logout();

            Assert.IsFalse(_webSecurityServices.AccountSecured);
        }

        [TestMethod]
         public async Task IsAuthenticated_WhenLoggedIn_ReturnsTrue()
        {
            _webSecurityServices.LoginAsync("Liam", "bob").Wait();

            bool isAuthenticated = _webSecurityServices.IsAuthenticated();

            Assert.IsTrue(isAuthenticated, "IsAuthenticated should return true after successful login.");
        }


        [TestMethod]
        public async Task GetUserName_WhenLoggedIn_ReturnsUserName()
        {
            string expectedUserName = "Liam";
            _webSecurityServices.LoginAsync(expectedUserName, "bob").Wait();

            string? userName = _webSecurityServices.GetUserName();

            // Diagnostic lines: check if these properties are set as expected
            Assert.IsTrue(_webSecurityServices.AccountSecured, "AccountSecured should be true after login.");
            Assert.IsNotNull(userName, "UserName should not be null after successful login.");
            Assert.AreEqual(expectedUserName, userName);
        }


        [TestMethod]
        public void GetUserName_WhenNotLoggedIn_ReturnsNull()
        {
            string? userName = _webSecurityServices.GetUserName();

            Assert.IsNull(userName);
        }
    }
}
