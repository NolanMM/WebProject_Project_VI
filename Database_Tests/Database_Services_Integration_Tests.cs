using WebProject_Project_VI.Models;
using WebProject_Project_VI.Services;

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
    }
}