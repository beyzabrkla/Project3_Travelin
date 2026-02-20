using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {
            return RedirectToAction("Index","Home");
        }
    }
}
