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
    [Authorize(Roles = "Admin")]  // Sadece Admin erişebilir
    public class AdminUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // ==================== TÜM KULLANICILARI LİSTELE ====================
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // ==================== ROL'E GÖRE KULLANICILARI FİLTRELE ====================
        [HttpGet]
        public async Task<IActionResult> UsersByRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            ViewBag.RoleName = roleName;
            return View("UserList", users);
        }

        // ==================== KULLANICI DETAYLARI ====================
        [HttpGet]
        public async Task<IActionResult> UserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = roles;
            return View(user);
        }

        // ==================== KULLANICI DÜZENLE ====================
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = roles;
            return View(user);
        }

        // ==================== KULLANICI GÜNCELLE ====================
        [HttpPost]
        public async Task<IActionResult> EditUser(string userId, string fullName, string email)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            user.FullName = fullName;
            user.Email = email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı bilgileri güncellendi.";
                return RedirectToAction("UserList");
            }

            TempData["Error"] = "Kullanıcı güncellenirken hata oluştu.";
            return View(user);
        }

        // ==================== KULLANICIYA ROL EKLE ====================
        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["Success"] = $"{roleName} rolü kullanıcıya eklendi.";
            }
            else
            {
                TempData["Error"] = "Rol eklenirken hata oluştu.";
            }

            return RedirectToAction("UserDetails", new { userId = userId });
        }

        // ==================== KULLANICIDAN ROL SİL ====================
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["Success"] = $"{roleName} rolü kullanıcıdan kaldırıldı.";
            }
            else
            {
                TempData["Error"] = "Rol kaldırılırken hata oluştu.";
            }

            return RedirectToAction("UserDetails", new { userId = userId });
        }

        // ==================== KULLANICILARI AKTİFLEŞTİR ====================
        [HttpPost]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı aktivleştirildi.";
            }
            else
            {
                TempData["Error"] = "Kullanıcı aktivleştirilemedi.";
            }

            return RedirectToAction("UserList");
        }

        // ==================== KULLANICILARI PASİFLEŞTİR ====================
        [HttpPost]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı pasifleştirildi.";
            }
            else
            {
                TempData["Error"] = "Kullanıcı pasifleştirilemedi.";
            }

            return RedirectToAction("UserList");
        }

        // ==================== KULLANICI SİL ====================
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            // Admin kendisini silemez
            if (await _userManager.IsInRoleAsync(user, "Admin") && user.Id == _userManager.GetUserAsync(User).Result?.Id)
            {
                TempData["Error"] = "Kendi hesabınızı silemezsiniz.";
                return RedirectToAction("UserList");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Kullanıcı silinirken hata oluştu.";
            }

            return RedirectToAction("UserList");
        }

        // ==================== KULLANICI İSTATİSTİKLERİ ====================
        [HttpGet]
        public async Task<IActionResult> UserStatistics()
        {
            var allUsers = _userManager.Users.ToList();
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var guides = await _userManager.GetUsersInRoleAsync("Guide");
            var customers = await _userManager.GetUsersInRoleAsync("Customer");

            var stats = new Dictionary<string, object>
            {
                { "TotalUsers", allUsers.Count },
                { "TotalAdmins", admins.Count },
                { "TotalGuides", guides.Count },
                { "TotalCustomers", customers.Count },
                { "ActiveUsers", allUsers.Count(u => u.IsActive) },
                { "InactiveUsers", allUsers.Count(u => !u.IsActive) }
            };

            return View(stats);
        }

        // ==================== ARAMA ====================
        [HttpPost]
        public async Task<IActionResult> SearchUser(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("UserList");
            }

            var users = _userManager.Users
                .Where(u => u.UserName.Contains(searchTerm) ||
                            u.Email.Contains(searchTerm) ||
                            u.FullName.Contains(searchTerm))
                .ToList();

            ViewBag.SearchTerm = searchTerm;
            return View("UserList", users);
        }

        // ==================== ŞİFRE RESETLE (ADMIN) ====================
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("UserList");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Şifre başarıyla sıfırlandı.";
            }
            else
            {
                TempData["Error"] = "Şifre sıfırlanırken hata oluştu.";
            }

            return RedirectToAction("UserDetails", new { userId = userId });
        }

        // ==================== TOPLU İŞLEMLER ====================
        [HttpPost]
        public async Task<IActionResult> BulkActivate(List<string> userIds)
        {
            if (userIds == null || userIds.Count == 0)
            {
                TempData["Error"] = "Lütfen en az bir kullanıcı seçin.";
                return RedirectToAction("UserList");
            }

            int successCount = 0;
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsActive = true;
                    await _userManager.UpdateAsync(user);
                    successCount++;
                }
            }

            TempData["Success"] = $"{successCount} kullanıcı aktivleştirildi.";
            return RedirectToAction("UserList");
        }

        // ==================== TOPLU İŞLEMLER - DEAKTİVASYON ====================
        [HttpPost]
        public async Task<IActionResult> BulkDeactivate(List<string> userIds)
        {
            if (userIds == null || userIds.Count == 0)
            {
                TempData["Error"] = "Lütfen en az bir kullanıcı seçin.";
                return RedirectToAction("UserList");
            }

            int successCount = 0;
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsActive = false;
                    await _userManager.UpdateAsync(user);
                    successCount++;
                }
            }

            TempData["Success"] = $"{successCount} kullanıcı pasifleştirildi.";
            return RedirectToAction("UserList");
        }
    }
}
