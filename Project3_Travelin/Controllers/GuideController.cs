using Microsoft.AspNetCore.Mvc;
using Project3_Travelin.DTOs.GuideDTOs;
using Project3_Travelin.Services.GuideServices;

namespace Project3_Travelin.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public async Task<IActionResult> GuideList()
        {
            var values = await _guideService.GetAllGuideAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateGuide()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuide(CreateGuideDTO createGuideDTO)
        {
            await _guideService.CreateGuideAsync(createGuideDTO);
            return RedirectToAction("GuideList");
        }

        public async Task<IActionResult> ChangeStatus(string id)
        {
            await _guideService.ChangeGuideStatusAsync(id);
            return RedirectToAction("GuideList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateGuide(string id)
        {
            var value = await _guideService.GetGuideByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGuide(UpdateGuideDTO updateGuideDTO)
        {
            await _guideService.UpdateGuideAsync(updateGuideDTO);
            return RedirectToAction("GuideList");
        }
    }
}
