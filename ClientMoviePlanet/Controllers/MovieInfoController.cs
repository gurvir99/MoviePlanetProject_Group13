using Microsoft.AspNetCore.Mvc;

namespace ClientMoviePlanet.Controllers
{
    public class MovieInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
