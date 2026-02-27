using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.CommentDTOs;
using DTOLayer.DTOs.TourDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IGuideService _guideService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public TourController(ITourService tourService, IMapper mapper, IGuideService guideService, ICommentService commentService)
        {
            _tourService = tourService;
            _mapper = mapper;
            _guideService = guideService;
            _commentService = commentService;
        }

        public async Task<IActionResult> TourList(string destination, string duration, bool? visaStatus, int page = 1)
        {
            int pageSize = 3;
            var allValues = await _tourService.GetAllTourAsync();

            allValues = allValues.Where(x => x.IsStatus == true && x.IsDrafts == false).ToList();

            if (visaStatus.HasValue)
                allValues = allValues.Where(x => x.IsVisaRequired == visaStatus.Value).ToList();

            if (!string.IsNullOrEmpty(destination))
                allValues = allValues.Where(x =>
                    x.TourCountry.Contains(destination, StringComparison.OrdinalIgnoreCase) ||
                    x.TourCity.Contains(destination, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(duration))
                allValues = allValues.Where(x => x.DayNight.Contains(duration)).ToList();

            foreach (var tour in allValues)
            {
                var comments = await _commentService.GetCommentsByTourIdAsync(tour.TourId);
                var approvedComments = comments.Where(c => c.IsStatus).ToList();
                tour.ReviewCount = approvedComments.Count;
                tour.Rating = approvedComments.Any() ? (int)Math.Round(approvedComments.Average(c => c.Score)) : 0;
            }

            var totalCount = allValues.Count();
            var pagedValues = allValues.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.TotalTours = totalCount;
            ViewBag.VisaStatus = visaStatus;
            ViewBag.Destination = destination;
            ViewBag.Duration = duration;

            return View(pagedValues);
        }

        public async Task<IActionResult> TourDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("TourList");

            var value = await _tourService.GetTourByIdAsync(id);
            if (value == null) return NotFound();

            var comments = await _commentService.GetCommentsByTourIdAsync(id);
            var approvedComments = comments
                .Where(c => c.IsStatus)
                .OrderByDescending(c => c.CommentDate)
                .ToList();

            value.Comments = _mapper.Map<List<ResultCommentListByTourIdDTO>>(approvedComments);
            value.ReviewCount = value.Comments.Count;
            value.Rating = value.Comments.Any()
                ? (int)Math.Round(value.Comments.Average(c => c.Score))
                : 0;

            return View(value);
        }

        public async Task<IActionResult> TourRoutes(string guideId, int page = 1)
        {
            int pageSize = 6;

            var allGuides = await _guideService.GetAllGuideAsync();
            ViewBag.Guides = allGuides.Where(x => x.Status).ToList();

            var tours = string.IsNullOrEmpty(guideId)
                ? await _tourService.GetAllTourAsync()
                : await _tourService.GetToursByGuideIdAsync(guideId);

            // ✅ DÜZELTİLDİ: Sadece aktif, taslak olmayan turlar gösterilsin
            tours = tours
                .Where(x => x.IsStatus == true && x.IsDrafts == false)
                .ToList();

            var totalCount = tours.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedValues = tours
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalTours = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.SelectedGuideId = guideId;

            return View(pagedValues);
        }

        public async Task<IActionResult> GuideTourLog(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("TourList");

            var tourValue = await _tourService.GetTourByIdAsync(id);
            if (tourValue == null) return NotFound();

            if (!string.IsNullOrEmpty(tourValue.GuideId))
            {
                var guideValue = await _guideService.GetGuideByIdAsync(tourValue.GuideId);
                if (guideValue != null)
                {
                    tourValue.GuideName = guideValue.Name;
                    tourValue.GuideTitle = guideValue.Title;
                    tourValue.GuideImageUrl = guideValue.ImageUrl;
                }
            }

            var model = _mapper.Map<GetTourByIdDTO>(tourValue);
            return View(model);
        }
    }
}