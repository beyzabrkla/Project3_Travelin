using System.Diagnostics;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project3_Travelin.Models;

namespace Project3_Travelin.Controllers
{
    [AllowAnonymous]
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

            ViewBag.Destinations = allTours.Select(x => x.TourCountry).Distinct().ToList();

            ViewBag.Durations = allTours.Select(x => x.DayNight).Distinct().ToList();

            // Footer için: tüm turlarýn ImageAlbumUrls'lerini topla, karýþtýr, ilk 9'u al
            var footerPhotos = allTours
                .Where(x => x.ImageAlbumUrls != null && x.ImageAlbumUrls.Any())
                .SelectMany(x => x.ImageAlbumUrls)
                .Where(url => !string.IsNullOrEmpty(url))
                .OrderBy(_ => Guid.NewGuid()) // rastgele karýþtýr
                .Take(9)
                .ToList();

            // Yeterli album fotoðrafý yoksa tur kapak fotoðraflarýný da ekle
            if (footerPhotos.Count < 9)
            {
                var coverPhotos = allTours
                    .Where(x => !string.IsNullOrEmpty(x.ImageUrl))
                    .Select(x => x.ImageUrl)
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(9 - footerPhotos.Count)
                    .ToList();

                footerPhotos.AddRange(coverPhotos);
            }

            ViewBag.FooterPhotos = footerPhotos;

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