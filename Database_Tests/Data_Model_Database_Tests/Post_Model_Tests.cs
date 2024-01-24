using WebProject_Project_VI.Models;

namespace Database_Tests.Data_Model_Database_Tests
{
    [TestClass]
    public class Post_Model_Tests
    {
        [TestMethod]
        public void TestToString()
        {
            // Arrange
            Post_Model post = new Post_Model
            {
                Title = "Test Title",
                Content = "Test Content",
                Author = "Test Author",
                Number_Of_Likes = 5,
                Number_Of_DisLikes = 2,
                Is_Public = true,
                Number_Of_Visits = 100,
                Date = DateTime.Now
            };

            // Assert
            string actualOutput = post.toString();
            string expectedOutput = $"Title: {post.Title}\nContent: {post.Content}\nAuthor: {post.Author}\n" +
                $"Likes: {post.Number_Of_Likes}\nDislikes: {post.Number_Of_DisLikes}\n" +
                $"Public: {post.Is_Public}\nVisits: {post.Number_Of_Visits}\nDate: {post.Date}";
            Assert.AreEqual(expectedOutput, actualOutput, $"Expected: {expectedOutput}, Actual: {actualOutput}");
        }

        [TestMethod]
        public void TestGetProperties()
        {
            // Arrange
            Post_Model post = new Post_Model();

            // Act
            string[] expectedProperties = {"PostId", "Title", "Content", "Author", "Number_Of_Likes", "Number_Of_DisLikes", "Is_Public", "Number_Of_Visits", "Date" };
            string[] actualProperties = post.Get_Property();

            // Assert
            CollectionAssert.AreEqual(expectedProperties, actualProperties);
        }
        [TestMethod]
        public void TestGetType()
        {
            // Arrange
            Post_Model post = new Post_Model();

            // Act
            Type expectedProperties = typeof(Post_Model);
            Type actualProperties = post.Get_Type();

            // Assert
            Assert.AreEqual(expectedProperties, actualProperties);
        }
        [TestMethod]
        public void TestKeyValuePair()
        {
            // Act
            var expectedKeyValuePair = new System.Collections.Generic.Dictionary<string, Type>
            {
                { "PostId", typeof(int) },
                { "Title", typeof(string) },
                { "Content", typeof(string) },
                { "Author", typeof(string) },
                { "Number_Of_Likes", typeof(int?) },
                { "Number_Of_DisLikes", typeof(int?) },
                { "Is_Public", typeof(bool?) },
                { "Number_Of_Visits", typeof(int?) },
                { "Date", typeof(DateTime?) }
            };
            var actualKeyValuePair = Post_Model.keyValuePairs();

            // Assert
            CollectionAssert.AreEqual(expectedKeyValuePair, actualKeyValuePair);
        }
    }
}
