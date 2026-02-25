using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.TourDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Metrics;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;

        public AdminTourController(ITourService tourService, IMapper mapper, IGuideService guideService)
        {
            _tourService = tourService;
            _mapper = mapper;
            _guideService = guideService;
        }
        private async Task SetCountryViewBag()
        {
            var allTours = await _tourService.GetAllTourAsync();
            ViewBag.Countries = allTours
                .Where(x => !string.IsNullOrEmpty(x.TourCountry))
                .Select(x => x.TourCountry)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            // Eğer hiç tur yoksa dropdown boş kalmasın diye varsayılan bir kaç ülke
            if (ViewBag.Countries.Count == 0)
            {
                ViewBag.Countries = new List<string> { "Türkiye", "İtalya", "Fransa", "İspanya", "Yunanistan" };
            }
        }
        private async Task SetGuideViewBag()
        {
            var guides = await _guideService.GetAllGuideAsync();

            if (guides == null || !guides.Any())
            {
                ViewBag.v = new List<SelectListItem> { new SelectListItem { Text = "Rehber Bulunamadı", Value = "" } };
                return;
            }

            var activeGuides = guides
                .Where(x => x.Status ==true) 
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.GuideId.ToString()
                }).ToList();

            ViewBag.Guides = activeGuides.Any() ? activeGuides : new List<SelectListItem>();
        }

        public async Task<IActionResult> TourList(string q, string country, string status, DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var values = await _tourService.GetAllTourAsync();

            // ÜLKE LİSTESİ
            ViewBag.Countries = values
                .Where(x => !string.IsNullOrEmpty(x.TourCountry))
                .Select(x => x.TourCountry)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            // DURUM FİLTRESİ (sidebar linklerinden gelir)
            if (!string.IsNullOrEmpty(status))
            {
                values = status switch
                {
                    "active" => values.Where(x => x.IsStatus == true && x.IsDrafts == false).ToList(),
                    "draft" => values.Where(x => x.IsDrafts == true).ToList(),
                    "passive" => values.Where(x => x.IsStatus == false && x.IsDrafts == false).ToList(),
                    _ => values
                };
            }

            // ARAMA FİLTRESİ
            if (!string.IsNullOrEmpty(q))
            {
                values = values.Where(x =>
                    (x.TourTitle != null && x.TourTitle.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (x.TourCity != null && x.TourCity.Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // ÜLKE FİLTRESİ
            if (!string.IsNullOrEmpty(country))
                values = values.Where(x => x.TourCountry != null && x.TourCountry == country).ToList();

            // TARİH FİLTRELERİ
            if (fromDate.HasValue)
                values = values.Where(x => x.TourDate >= fromDate.Value).ToList();
            if (toDate.HasValue)
                values = values.Where(x => x.TourDate <= toDate.Value).ToList();

            // SAYFALAMA
            int pageSize = 6;
            int totalCount = values.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = page < 1 ? 1 : page;

            var pagedData = values.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.ActiveStatus = status; // View'da hangi linkin aktif olduğunu vurgulamak için

            return View(pagedData);
        }

        public async Task<IActionResult> DraftTours()
        {
            var values = await _tourService.GetAllTourAsync();
            var drafts = values.Where(x => x.IsDrafts == true).ToList();
            return View("TourList", drafts);
        }

        public async Task<IActionResult> ActiveTours()
        {
            var values = await _tourService.GetAllTourAsync();
            var actives = values.Where(x => x.IsStatus == true && x.IsDrafts == false).ToList();
            return View("TourList", actives);
        }

        public async Task<IActionResult> PassiveTours()
        {
            var values = await _tourService.GetAllTourAsync();
            var passives = values.Where(x => x.IsStatus == false).ToList();
            return View("TourList", passives);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            await SetGuideViewBag(); 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTour(CreateTourDTO model)
        {
            ModelState.Remove("ImageAlbumUrls");

            if (ModelState.IsValid)
            {
                var status = Request.Form["PublishStatus"];

                model.IsStatus = (status == "active" || status == "passive");
                model.IsDrafts = (status == "draft");

                await _tourService.CreateTourAsync(model);
                return RedirectToAction("TourList");
            }

            await SetGuideViewBag();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);
            var model = _mapper.Map<UpdateTourDTO>(value);

            await SetCountryViewBag();
            await SetGuideViewBag();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDTO updateTourDTO)
        {
            // Formda hiç olmayan (hidden olarak bile taşımadığımız) alanları validasyondan muaf tut
            string[] ignoredFields = new[] { "GuideName", "GuideTitle", "GuideImageUrl", "ImageAlbumUrls", "SubDescription", "GuideDescription" };
            foreach (var field in ignoredFields) ModelState.Remove(field);

            if (!ModelState.IsValid)
            {
                await SetCountryViewBag();
                await SetGuideViewBag(); // Rehber listesini tekrar doldur
                return View(updateTourDTO);
            }

            // 1. Veritabanındaki orijinal kaydı çek (Entity olarak)
            var existingTour = await _tourService.GetTourByIdAsync(updateTourDTO.TourId);

            // 2. AutoMapper Sihri: 
            // updateTourDTO içindeki dolu olan her şeyi existingTour üzerine yazar.
            // DTO'da olmayan alanlar (Örn: ImageAlbumUrls) existingTour içinde korunur.
            _mapper.Map(updateTourDTO, existingTour);

            // 3. Güncellenmiş Entity'yi tekrar DTO'ya çevirip servise gönder (Eğer servis DTO istiyorsa)
            var finalDto = _mapper.Map<UpdateTourDTO>(existingTour);
            await _tourService.UpdateTourAsync(finalDto);

            return RedirectToAction("TourList");
        }
        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourService.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }
    }
}
