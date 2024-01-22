using WebProject_Project_VI.Models;

namespace Database_Tests.Data_Model_Database_Tests
{
    [TestClass]
    public class Account_Model_Tests
    {
        [TestMethod]
        public void TestToString()
        {
            // Arrange
            Account_Model account = new Account_Model
            {
                Username = "testuser",
                Password = "testpassword",
                AuthorName = "Test Author"
            };

            // Assert
            string actualOutput = account.toString();
            string expectedOutput = $"Username: {account.Username}\nPassword: {account.Password}\nAuthor Name: {account.AuthorName}";
            Assert.AreEqual(expectedOutput, actualOutput, $"Expected: {expectedOutput}, Actual: {actualOutput}");
        }

        [TestMethod]
        public void TestGetProperties()
        {
            // Arrange
            Account_Model account = new Account_Model();

            // Act
            string[] expectedProperties = { "Username", "Password", "AuthorName" };
            string[] actualProperties = account.Get_Property();

            // Assert
            CollectionAssert.AreEqual(expectedProperties, actualProperties);
        }
        [TestMethod]
        public void TestGetType()
        {
            // Arrange
            Account_Model account = new Account_Model();

            // Act
            Type expectedProperties = typeof(Account_Model);
            Type actualProperties = account.Get_Type();

            // Assert
            Assert.AreEqual(expectedProperties, actualProperties);
        }
        [TestMethod]
        public void TestKeyValuePair()
        {
            // Arrange
            Account_Model account = new Account_Model();

            // Act
            var expectedKeyValuePair = new System.Collections.Generic.Dictionary<string, Type>
            {
                { "Username", typeof(string) },
                { "Password", typeof(string) },
                { "AuthorName", typeof(string) }
            };
            var actualKeyValuePair = Account_Model.keyValuePairs();

            // Assert
            CollectionAssert.AreEqual(expectedKeyValuePair, actualKeyValuePair);
        }
    }
}
