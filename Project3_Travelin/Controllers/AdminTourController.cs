using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.AIDTOS;
using DTOLayer.DTOs.TourDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AdminTourController(ITourService tourService, IMapper mapper, IGuideService guideService, IConfiguration configuration)
        {
            _tourService = tourService;
            _mapper = mapper;
            _guideService = guideService;
            _configuration = configuration;
        }

        // ── YARDIMCI METOTLAR ─────────────────────────────────────────

        private async Task SetCountryViewBag()
        {
            var allTours = await _tourService.GetAllTourAsync();
            ViewBag.Countries = allTours
                .Where(x => !string.IsNullOrEmpty(x.TourCountry))
                .Select(x => x.TourCountry)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            if (((List<string>)ViewBag.Countries).Count == 0)
                ViewBag.Countries = new List<string> { "Turkiye", "Italy", "France", "Spain", "Greece" };
        }

        private async Task SetGuideViewBag()
        {
            var guides = await _guideService.GetAllGuideAsync();

            if (guides == null || !guides.Any())
            {
                ViewBag.Guides = new List<SelectListItem> { new SelectListItem { Text = "No Guide Found", Value = "" } };
                return;
            }

            var activeGuides = guides
                .Where(x => x.Status == true)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.GuideId.ToString()
                }).ToList();

            ViewBag.Guides = activeGuides.Any()
                ? activeGuides
                : new List<SelectListItem> { new SelectListItem { Text = "No Guide Found", Value = "" } };
        }

        private void RemoveOptionalModelStateFields()
        {
            ModelState.Remove("ImageFile");
            ModelState.Remove("MapImageFile");
            ModelState.Remove("GuideImages");
            ModelState.Remove("ImageAlbumUrls");
            ModelState.Remove("GuideAlbumUrls");
            ModelState.Remove("GuideName");
            ModelState.Remove("GuideTitle");
            ModelState.Remove("GuideImageUrl");
            ModelState.Remove("GuideDescription");
            ModelState.Remove("VideoUrl");      
        }


        public async Task<IActionResult> TourList(string q, string country, string status, DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var values = await _tourService.GetAllTourAsync();

            ViewBag.Countries = values
                .Where(x => !string.IsNullOrEmpty(x.TourCountry))
                .Select(x => x.TourCountry)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

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

            if (!string.IsNullOrEmpty(q))
                values = values.Where(x =>
                    (x.TourTitle != null && x.TourTitle.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (x.TourCity != null && x.TourCity.Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();

            if (!string.IsNullOrEmpty(country))
                values = values.Where(x => x.TourCountry != null && x.TourCountry == country).ToList();

            if (fromDate.HasValue) values = values.Where(x => x.TourDate >= fromDate.Value).ToList();
            if (toDate.HasValue) values = values.Where(x => x.TourDate <= toDate.Value).ToList();

            int pageSize = 6;
            int totalCount = values.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = page < 1 ? 1 : page;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.ActiveStatus = status;

            return View(values.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public async Task<IActionResult> DraftTours()
        {
            var values = await _tourService.GetAllTourAsync();
            return View("TourList", values.Where(x => x.IsDrafts == true).ToList());
        }

        public async Task<IActionResult> ActiveTours()
        {
            var values = await _tourService.GetAllTourAsync();
            return View("TourList", values.Where(x => x.IsStatus == true && x.IsDrafts == false).ToList());
        }

        public async Task<IActionResult> PassiveTours()
        {
            var values = await _tourService.GetAllTourAsync();
            return View("TourList", values.Where(x => x.IsStatus == false).ToList());
        }


        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            await SetGuideViewBag();
            return View(new CreateTourDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTour(CreateTourDTO model)
        {
            RemoveOptionalModelStateFields();

            if (!ModelState.IsValid)
            {
                await SetGuideViewBag();
                return View(model);
            }

            if (model.ImageFile != null && model.ImageFile.Length > 0)
                model.ImageUrl = await SaveImage(model.ImageFile);

            if (model.MapImageFile != null && model.MapImageFile.Length > 0)
                model.MapImageUrl = await SaveImage(model.MapImageFile);

            var publishStatus = Request.Form["PublishStatus"].ToString();
            model.IsStatus = publishStatus == "active";
            model.IsDrafts = publishStatus == "draft";

            await _tourService.CreateTourAsync(model);
            return RedirectToAction("TourList");
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTour(UpdateTourDTO updateTourDTO)
        {
            RemoveOptionalModelStateFields();

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();

                await SetCountryViewBag();
                await SetGuideViewBag();
                return View(updateTourDTO);
            }

            try
            {
                if (updateTourDTO.ImageFile != null && updateTourDTO.ImageFile.Length > 0)
                    updateTourDTO.ImageUrl = await SaveImage(updateTourDTO.ImageFile);

                if (updateTourDTO.MapImageFile != null && updateTourDTO.MapImageFile.Length > 0)
                    updateTourDTO.MapImageUrl = await SaveImage(updateTourDTO.MapImageFile);

                await _tourService.UpdateTourAsync(updateTourDTO);
                return RedirectToAction("TourList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sistem Hatası: " + ex.Message);
                await SetCountryViewBag();
                await SetGuideViewBag();
                return View(updateTourDTO);
            }
        }


        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourService.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }


        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var resource = Directory.GetCurrentDirectory();
            var extension = Path.GetExtension(file.FileName);
            var newImageName = Guid.NewGuid() + extension;
            var saveLocation = Path.Combine(resource, "wwwroot/userimages", newImageName);

            var directoryPath = Path.GetDirectoryName(saveLocation);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var stream = new FileStream(saveLocation, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newImageName;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateDescription([FromBody] GenerateDescriptionRequest request)
        {
            try
            {
                var apiKey = _configuration["Groq:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    return Json(new { success = false, error = "Groq API key is not configured." });

                var toneDesc = request.Tone switch
                {
                    "heyecanli" => "exciting, adventurous and energetic",
                    "romantik" => "romantic, poetic and emotional",
                    "luks" => "luxurious, sophisticated and premium",
                    _ => "professional, informative and trustworthy"
                };

                var systemMessage =
                    "You are a professional travel content writer. " +
                    "Always write tour descriptions in Turkish. " +
                    "Return ONLY valid JSON with no markdown fences, no extra text.";

                var userMessage =
                    $"Write a tour description in a {toneDesc} style.\n\n" +
                    $"Tour Title: {request.Title}\n" +
                    $"Country: {request.Country}\n" +
                    $"City: {request.City}\n" +
                    $"Duration: {request.Duration}\n" +
                    $"Price: {(string.IsNullOrEmpty(request.Price) ? "not specified" : request.Price + " TL")}\n\n" +
                    "Respond ONLY with this JSON structure (values must be in Turkish):\n" +
                    "{\"fullDescription\":\"250-350 word detailed description\",\"subDescription\":\"Max 150 character catchy summary\"}";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
                    {
                        new { role = "system", content = systemMessage },
                        new { role = "user",   content = userMessage   }
                    },
                    max_tokens = 1200,
                    temperature = 0.7
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", jsonContent);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, error = "Groq API error: " + responseBody });

                using var doc = JsonDocument.Parse(responseBody);
                var rawContent = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "";

                var cleaned = rawContent.Replace("```json", "").Replace("```", "").Trim();

                using var resultDoc = JsonDocument.Parse(cleaned);
                var fullDesc = resultDoc.RootElement.GetProperty("fullDescription").GetString();
                var subDesc = resultDoc.RootElement.GetProperty("subDescription").GetString();

                return Json(new { success = true, fullDescription = fullDesc, subDescription = subDesc });
            }
            catch (JsonException)
            {
                return Json(new { success = false, error = "AI yanıtı beklenen formatta değildi. Lütfen tekrar deneyin." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Beklenmeyen hata: " + ex.Message });
            }
        }
    }
}