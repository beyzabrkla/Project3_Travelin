using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project3_Travelin.DTOs.TourDTOs;
using Project3_Travelin.Services.GuideServices;
using Project3_Travelin.Services.TourServices;

namespace Project3_Travelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;


        public TourController(ITourService tourService, IMapper mapper, IGuideService guideService)
        {
            _tourService = tourService;
            _mapper = mapper;
            _guideService = guideService;
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
        public async Task<IActionResult> TourDetail(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);

            return View(value);
        }

        public async Task<IActionResult> TourRoutes(string guideId)
        {
            // Tüm aktif rehberleri çekiyoruz
            var allGuides = await _guideService.GetAllGuideAsync();
            ViewBag.Guides = allGuides.Where(x => x.Status).ToList();

            var tours = string.IsNullOrEmpty(guideId)
                        ? await _tourService.GetAllTourAsync()
                        : await _tourService.GetToursByGuideIdAsync(guideId);

            return View(tours);
        }
        public async Task<IActionResult> GuideTourLog(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);
            return View(value);
        }

    }
}
