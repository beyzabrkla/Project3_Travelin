using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.GuideDTOs;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;

        public AdminUserController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IGuideService guideService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _guideService = guideService;
            _mapper = mapper;
        }

        #region Kullanıcı Listeleme ve Detay
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Role = roles.FirstOrDefault() ?? "Customer";
            }
            return View(users);
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
        public async Task<IActionResult> UserDetails(string userId, int? guideId)
        {
            AppUser user = null;

            if (!string.IsNullOrEmpty(userId))
            {
                user = await _userManager.FindByIdAsync(userId);
            }
            else if (guideId.HasValue)
            {
                var guide = await _guideService.GetGuideByIdAsync(guideId.Value.ToString());

                if (guide != null)
                {
                    user = _userManager.Users.FirstOrDefault(x => x.FullName == guide.Name || x.UserName == guide.Name);
                }
            }

            if (user == null)
            {
                TempData["Error"] = "İlgili rehberin kullanıcı hesabı sistemde bulunamadı.";
                return RedirectToAction("GuideList");
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = roles;
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> GuideDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // 1. Rehberin MongoDB bilgilerini getir
            var guide = await _guideService.GetGuideByIdAsync(id);
            if (guide == null) return NotFound();

            // 2. Bu rehberle ilişkili Identity kullanıcısını (AppUser) bul
            // (İsim üzerinden eşleştirme yapıyoruz)
            var user = _userManager.Users.FirstOrDefault(x =>
                x.FullName == guide.Name || x.UserName == guide.Name);

            // 3. Rehber bilgilerini View'a taşımak için ViewBag kullanabiliriz
            ViewBag.GuideInfo = guide;

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                ViewBag.UserRoles = roles;
                return View(user); // View'da @model AppUser bekleyeceğiz
            }

            // Kullanıcı hesabı bulunamazsa sadece rehber ismiyle boş bir model gönder
            TempData["Warning"] = "Bu rehberin eşleşmiş bir kullanıcı hesabı bulunamadı.";
            return View(new AppUser { FullName = guide.Name });
        }

        [HttpGet]
        public async Task<IActionResult> RedirectToGuideUpdate(string fullName)
        {
            var allGuides = await _guideService.GetAllGuideAsync();
            var guide = allGuides.FirstOrDefault(x => x.Name == fullName);

            if (guide != null)
            {
                // Rehber bulunduysa UpdateGuide sayfasına git
                return RedirectToAction("UpdateGuide", new { id = guide.GuideId });
            }

            // Eğer rehber kaydı henüz MongoDB'de oluşmadıysa hata ver veya UserList'e dön
            TempData["Error"] = "Bu kullanıcının rehber profil kaydı bulunamadı.";
            return RedirectToAction("UserList");
        }

        #endregion


        #region Rol İşlemleri

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                user.Role = roleName;
                await _userManager.UpdateAsync(user);

                if (roleName == "Guide")
                {
                    await _guideService.CreateGuideAsync(new CreateGuideDTO
                    {
                        Name = user.FullName ?? user.UserName,
                        Status = true,
                        ImageUrl = "/images/default-guide.png",
                        CreatedAt = DateTime.Now.ToString("o")
                    });
                }
                TempData["Success"] = "Yetki başarıyla güncellendi.";
            }
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);

                user.Role = "Customer";
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("UserList");
        }
        #endregion


        #region Aktivasyon ve Silme İşlemleri
        [HttpPost]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) { user.IsActive = true; await _userManager.UpdateAsync(user); }
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) { user.IsActive = false; await _userManager.UpdateAsync(user); }
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id.ToString() == userId)
            {
                TempData["Error"] = "Kendi hesabınızı silemezsiniz!";
                return RedirectToAction("UserList");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) await _userManager.DeleteAsync(user);

            return RedirectToAction("UserList");
        }
        #endregion


        #region Rehber Yönetim İşlemleri

        [HttpGet]
        public async Task<IActionResult> UpdateGuide(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var guide = await _guideService.GetGuideByIdAsync(id);
            if (guide == null) return NotFound();

            var model = _mapper.Map<UpdateGuideDTO>(guide);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("GuideList");

            var guide = await _guideService.GetGuideByIdAsync(id);
            if (guide != null)
            {
                guide.Status = !guide.Status;

                var updateModel = _mapper.Map<UpdateGuideDTO>(guide);

                await _guideService.UpdateGuideAsync(updateModel);
            }
            return RedirectToAction("GuideList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGuide(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _guideService.DeleteGuideAsync(id);
            }
            return RedirectToAction("GuideList");
        }

        #endregion
    }
}