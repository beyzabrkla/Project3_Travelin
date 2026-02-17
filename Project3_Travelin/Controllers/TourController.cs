using Microsoft.AspNetCore.Mvc;
using Project3_Travelin.DTOs.TourDTOs;
using Project3_Travelin.Services.TourServices;

namespace Project3_Travelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDTO createTourDTO)
        {
            await _tourService.CreateTourAsync(createTourDTO);
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> TourList()
        {
            var values= await _tourService.GetAllTourAsync();
            return View(values);
        }
    }
}
