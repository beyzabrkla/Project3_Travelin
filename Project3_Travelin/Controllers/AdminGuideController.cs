using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.GuideDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminGuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;

        public AdminGuideController(IGuideService guideService, IMapper mapper)
        {
            _guideService = guideService;
            _mapper = mapper;
        }

        public async Task<IActionResult> GuideList(string q, int page = 1)
        {
            var values = await _guideService.GetAllGuideAsync();

            if (!string.IsNullOrEmpty(q))
            {
                values = values.Where(x =>
                    (x.Name != null && x.Name.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (x.Title != null && x.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            int pageSize = 6;
            int totalCount = values.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = page < 1 ? 1 : page;

            var pagedData = values.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchQuery = q;

            return View(pagedData);
        }

        [HttpGet]
        public IActionResult CreateGuide()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuide(CreateGuideDTO createGuideDTO)
        {
            if (ModelState.IsValid)
            {
                await _guideService.CreateGuideAsync(createGuideDTO);
                return RedirectToAction("GuideList");
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            ViewBag.Errors = errors;
            return View(createGuideDTO);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateGuide(string id)
        {
            var value = await _guideService.GetGuideByIdAsync(id);
            var model = _mapper.Map<UpdateGuideDTO>(value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGuide(UpdateGuideDTO updateGuideDTO)
        {
            if (ModelState.IsValid)
            {
                await _guideService.UpdateGuideAsync(updateGuideDTO);
                return RedirectToAction("GuideList");
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            ViewBag.Errors = errors;
            return View(updateGuideDTO);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var guide = await _guideService.GetGuideByIdAsync(id);
            var model = _mapper.Map<UpdateGuideDTO>(guide);
            model.Status = !model.Status;
            await _guideService.UpdateGuideAsync(model);
            return RedirectToAction("GuideList");
        }
    }
}
