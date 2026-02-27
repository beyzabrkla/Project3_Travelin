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
            string[] ignoredFields = new[] { "GuideName", "GuideTitle", "GuideImageUrl", "ImageAlbumUrls", "GuideDescription" };
            foreach (var field in ignoredFields) ModelState.Remove(field);

            if (!ModelState.IsValid)
            {
                await SetCountryViewBag();
                await SetGuideViewBag();
                return View(updateTourDTO);
            }

            var existingTour = await _tourService.GetTourByIdAsync(updateTourDTO.TourId);
            _mapper.Map(updateTourDTO, existingTour);
            var finalDto = _mapper.Map<UpdateTourDTO>(existingTour);
            await _tourService.UpdateTourAsync(finalDto);

            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourService.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }

        // ══════════════════════════════════════════════════════════
        // GROQ AI - DESCRIPTION GENERATOR
        // ══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateDescription([FromBody] GenerateDescriptionRequest request)
        {
            try
            {
                var apiKey = _configuration["Groq:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    return Json(new { success = false, error = "Groq API key is not configured in appsettings.json." });

                // Tone is passed in Turkish from UI but we map to English for a language-agnostic prompt
                var toneDesc = request.Tone switch
                {
                    "heyecanli" => "exciting, adventurous and energetic",
                    "romantik" => "romantic, poetic and emotional",
                    "luks" => "luxurious, sophisticated and premium",
                    _ => "professional, informative and trustworthy"
                };

                // Prompt is intentionally in English so Google Translate on the frontend
                // does not interfere with the AI instructions — only the OUTPUT is Turkish.
                var systemMessage = "You are a professional travel content writer. " +
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
                    "{\"fullDescription\":\"250-350 word detailed description including places to visit, activities, accommodation, meals and motivation\",\"subDescription\":\"Max 150 character catchy summary for the tour card\"}";

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

                // Extract text from Groq response
                using var doc = JsonDocument.Parse(responseBody);
                var rawContent = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "";

                // Strip markdown fences if model wraps in ```json ... ```
                var cleaned = rawContent
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                using var resultDoc = JsonDocument.Parse(cleaned);
                var fullDesc = resultDoc.RootElement.GetProperty("fullDescription").GetString();
                var subDesc = resultDoc.RootElement.GetProperty("subDescription").GetString();

                return Json(new
                {
                    success = true,
                    fullDescription = fullDesc,
                    subDescription = subDesc
                });
            }
            catch (JsonException)
            {
                return Json(new { success = false, error = "AI response was not in the expected format. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Unexpected error: " + ex.Message });
            }
        }
    }
}