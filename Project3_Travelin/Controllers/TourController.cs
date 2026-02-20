using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<IActionResult> TourList(int page = 1)
        {
            int pageSize = 3;
            var allValues = await _tourService.GetAllTourAsync();

            var totalCount = allValues.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedValues = allValues
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedValues);
        }
    }
}
