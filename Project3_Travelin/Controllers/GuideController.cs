using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.GuideDTOs;
using DTOLayer.DTOs.TourDTOs;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly ITourService _tourService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GuideController(IGuideService guideService, UserManager<AppUser> userManager, ITourService tourService, IMapper mapper)
        {
            _guideService = guideService;
            _userManager = userManager;
            _tourService = tourService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignGuideRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.AddToRoleAsync(user, "Guide");
            if (result.Succeeded)
            {
                var createGuideDTO = new CreateGuideDTO
                {
                    UserId = user.Id.ToString(),
                    Name = user.FullName,
                    Status = true,
                };
                await _guideService.CreateGuideAsync(createGuideDTO);
                TempData["Success"] = $"{user.FullName} artık bir rehber!";
            }

            return RedirectToAction("UserList", "Admin");
        }

        [Authorize(Roles = "Guide")]
        [HttpGet]
        public async Task<IActionResult> MyTours()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Dashboard", "Customer");

            var allGuides = await _guideService.GetAllGuideAsync();
            var myGuide = allGuides.FirstOrDefault(g => g.UserId == currentUser.Id.ToString());

            if (myGuide == null)
            {
                ViewBag.Error = "Rehber profiliniz henüz oluşturulmamış.";
                return View(new List<ResultTourDTO>());
            }

            var tours = await _tourService.GetToursByGuideIdAsync(myGuide.GuideId);

            ViewBag.GuideName = myGuide.Name;
            ViewBag.TourCount = tours.Count;
            ViewBag.ActiveTourCount = tours.Count(x => x.IsStatus && !x.IsDrafts);

            return View(tours);
        }


        [Authorize(Roles = "Guide")]
        [HttpGet]
        public async Task<IActionResult> GuideTourLog(string id)
        {
            var tourValue = await _tourService.GetTourByIdAsync(id); 
            if (tourValue == null) return NotFound();

            var model = _mapper.Map<ResultTourDTO>(tourValue);

            return View(model);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        public async Task<IActionResult> GuideTourLog(UpdateTourDTO model)
        {
            try
            {
                var existingTourData = await _tourService.GetTourByIdAsync(model.TourId);

                if (existingTourData != null)
                {
                    var updateTour = _mapper.Map<UpdateTourDTO>(existingTourData);

                    updateTour.GuideDescription = model.GuideDescription;

                    if (model.GuideImages != null && model.GuideImages.Any())
                    {
                        if (updateTour.GuideAlbumUrls == null) updateTour.GuideAlbumUrls = new List<string>();

                        foreach (var item in model.GuideImages)
                        {
                            var extension = Path.GetExtension(item.FileName);
                            var newImageName = Guid.NewGuid() + extension;
                            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimages/", newImageName);

                            using var stream = new FileStream(location, FileMode.Create);
                            await item.CopyToAsync(stream);

                            updateTour.GuideAlbumUrls.Add("/userimages/" + newImageName);
                        }
                    }

                    await _tourService.UpdateTourAsync(updateTour);
                    TempData["Success"] = "Gezi günlüğünüz başarıyla güncellendi.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata oluştu: " + ex.Message;
            }

            return RedirectToAction("MyTours");
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateGuideDTO updateDTO)
        {
            if (!ModelState.IsValid) return RedirectToAction("MyTours");

            await _guideService.UpdateGuideAsync(updateDTO);
            TempData["Success"] = "Profiliniz başarıyla güncellendi.";

            return RedirectToAction("MyTours");
        }

        [HttpGet]
        public async Task<IActionResult> GuideDetail(string id)
        {
            var guide = await _guideService.GetGuideByIdAsync(id);
            if (guide == null) return NotFound();
            return View(guide);
        }
    }
}