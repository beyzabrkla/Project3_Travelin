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
    [Authorize(Roles = "Admin")]
    public class AdminGuideController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGuideService _guideService;

        public AdminGuideController(UserManager<AppUser> userManager, IGuideService guideService)
        {
            _userManager = userManager;
            _guideService = guideService;
        }

        [HttpGet]
        public async Task<IActionResult> GuideList(string q, int page = 1)
        {
            var guides = await _guideService.GetAllGuideAsync();

            if (!string.IsNullOrEmpty(q))
            {
                guides = guides.Where(x => x.Name.Contains(q) || x.Title.Contains(q)).ToList();
            }

            ViewBag.TotalCount = guides.Count;
            ViewBag.SearchQuery = q;
            ViewBag.TotalPages = 1;
            ViewBag.CurrentPage = 1;

            return View(guides); 
        }

        [HttpGet]
        public IActionResult CreateGuide()
        {
            return View(new CreateGuideDTO { Status = true});
        }

        // ==================== YENİ REHBER EKLEME (POST) ====================
        [HttpPost]
        public async Task<IActionResult> CreateGuide(CreateGuideDTO createGuideDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(createGuideDTO);
            }

            createGuideDTO.CreatedAt = DateTime.Now.ToString("o");

            await _guideService.CreateGuideAsync(createGuideDTO);
            return RedirectToAction("GuideList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateGuide(string id)
        {
            var guide = await _guideService.GetGuideByIdAsync(id);
            if (guide == null) return NotFound();

            // Senin UpdateGuideDTO yapına göre mapping yapıyoruz
            var updateDto = new UpdateGuideDTO
            {
                GuideId = guide.GuideId,
                Name = guide.Name,
                Title = guide.Title,
                Description = guide.Description, 
                ImageUrl = guide.ImageUrl,
                Status = guide.Status,
                UserId = guide.UserId,
                Specialization = guide.Specialization,
                Experience = guide.Experience,
                Languages = guide.Languages,
                About = guide.About,
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGuide(UpdateGuideDTO updateGuideDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(updateGuideDTO);
            }

            updateGuideDTO.UpdatedAt = DateTime.Now;
            await _guideService.UpdateGuideAsync(updateGuideDTO);

            return RedirectToAction("GuideList");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var guide = await _guideService.GetGuideByIdAsync(id);
            if (guide != null)
            {
                guide.Status = !guide.Status;
                var updateDto = new UpdateGuideDTO
                {
                    GuideId = guide.GuideId,
                    Name = guide.Name,
                    Title = guide.Title,
                    Description = guide.Description,
                    ImageUrl = guide.ImageUrl,
                    Status = guide.Status
                };
                await _guideService.UpdateGuideAsync(updateDto);
            }
            return RedirectToAction("GuideList");
        }

        [HttpPost]
        public async Task<IActionResult> MakeGuide(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["GuideError"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("GuideList");
            }

            try
            {
                if (!await _userManager.IsInRoleAsync(user, "Guide"))
                {
                    await _userManager.AddToRoleAsync(user, "Guide");
                }

                var createGuideDTO = new CreateGuideDTO
                {
                    UserId = user.Id.ToString(), 
                    Name = user.FullName,
                    Status = true,
                    About = "Yeni rehber profili.",
                    Specialization = "Belirtilmedi"
                };

                await _guideService.CreateGuideAsync(createGuideDTO);

                TempData["GuideSuccess"] = $"{user.FullName} başarıyla rehber yapıldı.";
            }
            catch (Exception ex)
            {
                TempData["GuideError"] = "İşlem sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("GuideList");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveGuideRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("GuideList");

            try
            {
                await _userManager.RemoveFromRoleAsync(user, "Guide");

                TempData["GuideSuccess"] = "Rehberlik yetkisi kaldırıldı.";
            }
            catch (Exception ex)
            {
                TempData["GuideError"] = "Hata: " + ex.Message;
            }

            return RedirectToAction("GuideList");
        }
    }
}