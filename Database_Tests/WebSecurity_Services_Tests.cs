using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
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
            var username = "validUser";
            var password = "validPassword";
            var mockAccount = new Account_Model { Username = username, Password = password };

            _mockAccountTableServices.Setup(s => s.Read_Account_Data_By_Username_Async(It.IsAny<string>()))
                .ReturnsAsync(mockAccount);

            var result = await _webSecurityServices.LoginAsync(username, password);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task LoginAsync_InvalidCredentials_ShouldReturnFalse()
        {
            var username = "invalidUser";
            var password = "invalidPassword";

            _mockAccountTableServices.Setup(s => s.Read_Account_Data_By_Username_Async(It.IsAny<string>()))
                .ReturnsAsync((Account_Model)null);

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
            await _webSecurityServices.LoginAsync("validUser", "validPassword"); // Ensure this is awaited

            // Diagnostic line: check if AccountSecured is true
            Assert.IsTrue(_webSecurityServices.AccountSecured, "Account should be secured after login.");

            bool isAuthenticated = _webSecurityServices.IsAuthenticated();

            Assert.IsTrue(isAuthenticated, "IsAuthenticated should return true after successful login.");
        }


        [TestMethod]
        public async Task GetUserName_WhenLoggedIn_ReturnsUserName()
        {
            string expectedUserName = "validUser";
            await _webSecurityServices.LoginAsync(expectedUserName, "validPassword"); // Await the login

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
