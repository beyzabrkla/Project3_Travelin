using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    public class AdminTourController : Controller
    {
        public IActionResult TourList()
        {
            return View();
        }
    }
}
