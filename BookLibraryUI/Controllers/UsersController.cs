using Microsoft.AspNetCore.Mvc;

namespace BookLibraryUI.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
