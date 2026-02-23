using System.Diagnostics;
using BusinessLayer.Services.TourServices;
using Microsoft.AspNetCore.Mvc;
using Project3_Travelin.Models;

namespace Project3_Travelin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITourService _tourService;
        public HomeController(ILogger<HomeController> logger, ITourService tourService)
        {
            _logger = logger;
            _tourService = tourService;
        }

        public async Task<IActionResult> Index()
        {
            var allTours = await _tourService.GetAllTourAsync();

            // Veritabanýndaki turlardan benzersiz Ülke listesini çekiyoruz
            ViewBag.Destinations = allTours.Select(x => x.TourCountry).Distinct().ToList();

            // Veritabanýndaki turlardan benzersiz Süre listesini çekiyoruz
            ViewBag.Durations = allTours.Select(x => x.DayNight).Distinct().ToList();

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
