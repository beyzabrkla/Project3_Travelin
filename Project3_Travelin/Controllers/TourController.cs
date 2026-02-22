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

        public async Task<IActionResult> TourList(string destination, string duration, int page = 1)
        {
            int pageSize = 3;
            var allValues = await _tourService.GetAllTourAsync();

            // Eğer destination boş değilse hem ülke hem şehirde ara
            if (!string.IsNullOrEmpty(destination))
            {
                allValues = allValues.Where(x =>
                    x.TourCountry.Contains(destination, StringComparison.OrdinalIgnoreCase) ||
                    x.TourCity.Contains(destination, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Süre seçildiyse (Örn: "5" gün içerenleri getir)
            if (!string.IsNullOrEmpty(duration))
            {
                allValues = allValues.Where(x => x.DayNight.Contains(duration)).ToList();
            }

            //Sayfalama
            var totalCount = allValues.Count();
            var pagedValues = allValues.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.TotalTours = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.Destination = destination;
            ViewBag.Duration = duration;

            return View(pagedValues);
        }
        public async Task<IActionResult> TourDetail(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);

            return View(value);
        }

        public async Task<IActionResult> TourRoutes(string guideId, int page = 1)
        {
            int pageSize = 6;

            //Tüm aktif rehberleri çek (Üst bar için)
            var allGuides = await _guideService.GetAllGuideAsync();
            ViewBag.Guides = allGuides.Where(x => x.Status).ToList();

            //Turları rehber filtresine göre al
            var tours = string.IsNullOrEmpty(guideId)
                        ? await _tourService.GetAllTourAsync()
                        : await _tourService.GetToursByGuideIdAsync(guideId);

            //Filtrelenmiş listenin toplam sayısını al
            var totalCount = tours.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            //Sayfalama işlemini uygula
            var pagedValues = tours
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            //ViewBag değerlerini gönder
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalTours = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.SelectedGuideId = guideId; // Filtrenin kaybolmaması için

            return View(pagedValues);
        }
        public async Task<IActionResult> GuideTourLog(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);
            return View(value);
        }

    }
}
