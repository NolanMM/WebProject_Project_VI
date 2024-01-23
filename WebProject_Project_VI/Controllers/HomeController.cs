using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebProject_Project_VI.Models;

namespace WebProject_Project_VI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            // Simulated data - replace this with your actual data retrieval logic
            List<Post> posts = new List<Post>
            {
            new Post { AuthorName = "Author1", PostTitle = "Title1", Content = "Content1", LikeCount = 10, DislikeCount = 0, ViewCount = 100 },
            new Post { AuthorName = "Author2", PostTitle = "Title2", Content = "Content2", LikeCount = 0, DislikeCount = 0, ViewCount = 0 },
            new Post { AuthorName = "Author3", PostTitle = "Title3", Content = "Content3", LikeCount = 0, DislikeCount = 0, ViewCount = 0 },
            new Post { AuthorName = "Liam", PostTitle = "Trafalgar Law is best character", Content = "Trafalgar D. Water Law, more commonly known as just Trafalgar Law and by his epithet as the Surgeon of Death, is a pirate from the North Blue and the captain and doctor of the Heart Pirates. He is one of twelve pirates who are referred to as the Worst Generation. He became one of the Seven Warlords of the Sea during the timeskip,but his position was revoked for allying with the Straw Hat Pirates. Law, like many other pirates, dreams of finding the One Piece, while also desiring to know the purpose of the Will of D.", LikeCount = 99999, DislikeCount = 0, ViewCount = 999999999 }
            };

            return View(posts);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
