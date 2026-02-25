using BusinessLayer.Abstract;
using DTOLayer.DTOs.GuideDTOs;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly UserManager<AppUser> _userManager;

        public GuideController(IGuideService guideService, UserManager<AppUser> userManager)
        {
            _guideService = guideService;
            _userManager = userManager;
        }

        // ==================== REHBER LİSTESİ (PUBLIC) ====================
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GuideList()
        {
            try
            {
                var values = await _guideService.GetAllGuideAsync();
                return View(values);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Rehber listesi yüklenemedi: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // ==================== REHBER YAP (ADMIN) ====================
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
                    UserId = user.Id.ToString(), // Guid ise ToString() ekledik
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
        public async Task<IActionResult> GuideDashboard()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Index", "Home");

            var guides = await _guideService.GetAllGuideAsync();
            var myProfile = guides.FirstOrDefault(x => x.UserId == currentUser.Id.ToString());

            if (myProfile == null)
            {
                TempData["Error"] = "Rehber profiliniz henüz oluşturulmamış.";
                return RedirectToAction("Index", "Home");
            }

            return View(myProfile);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateGuideDTO updateDTO)
        {
            if (!ModelState.IsValid) return View("GuideDashboard", updateDTO);

            try
            {
                await _guideService.UpdateGuideAsync(updateDTO);
                TempData["Success"] = "Profiliniz başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Güncelleme hatası: " + ex.Message;
            }

            return RedirectToAction("GuideDashboard");
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