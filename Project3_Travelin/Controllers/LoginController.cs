using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ==================== KULLANICI GİRİŞ ====================
        [HttpPost]
        public async Task<IActionResult> Login(string user_name, string password_name)
        {
            var user = await _userManager.FindByNameAsync(user_name);
            if (user == null)
                user = await _userManager.FindByEmailAsync(user_name);
            if (user == null)
            {
                TempData["LoginError"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index", "Home");
            }
            if (!user.IsActive)
            {
                TempData["LoginError"] = "Hesabınız pasif durumdadır.";
                return RedirectToAction("Index", "Home");
            }
            var result = await _signInManager.PasswordSignInAsync(user, password_name, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Role göre yönlendirme mantığı
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("TourList", "AdminTour");

                if (await _userManager.IsInRoleAsync(user, "Guide"))
                {
                    // Guide Dashboard'a yönlendir
                    return RedirectToAction("Dashboard", "GuideDashboard");
                }

                // Customer ise Customer Dashboard'a gider
                return RedirectToAction("Dashboard", "Customer");
            }
            TempData["LoginError"] = "Kullanıcı adı veya şifre hatalı.";
            return RedirectToAction("Index", "Home");
        }

        // ==================== KULLANICI KAYIT ====================
        [HttpPost]
        public async Task<IActionResult> Register(string user_name, string user_email, string password_name, string repassword_name)
        {
            if (password_name != repassword_name)
            {
                TempData["RegisterError"] = "Şifreler eşleşmiyor.";
                return RedirectToAction("Index", "Home");
            }
            var existingUser = await _userManager.FindByNameAsync(user_name);
            if (existingUser != null)
            {
                TempData["RegisterError"] = "Bu kullanıcı adı zaten kullanılıyor.";
                return RedirectToAction("Index", "Home");
            }
            var user = new AppUser
            {
                UserName = user_name,
                Email = user_email,
                FullName = user_name,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, password_name);
            if (result.Succeeded)
            {
                // Yeni kayıt olanı Customer rolüne ata
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, isPersistent: true);
                TempData["RegisterSuccess"] = "Kayıt başarılı! Rezervasyon yapabilirsiniz.";
                // Kayıttan sonra direkt dashboard'a gönderelim
                return RedirectToAction("Dashboard", "Customer");
            }
            TempData["RegisterError"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToAction("Index", "Home");
        }

        // ==================== ADMIN GİRİŞ ====================
        [HttpPost]
        public async Task<IActionResult> AdminLogin(string user_name, string password_name)
        {
            var user = await _userManager.FindByNameAsync(user_name);
            if (user == null)
                user = await _userManager.FindByEmailAsync(user_name);

            if (user == null)
            {
                TempData["AdminLoginError"] = "Admin bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            // Eğer senin kullanıcı adın buysa, veritabanında Admin rolü yoksa bile ekle.
            if (user.UserName == "bbeym") // Buraya kendi kullanıcı adını yaz!
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Rol kontrolünü MongoDB'deki olası hatalara karşı biraz daha esnetelim
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                TempData["AdminLoginError"] = "Bu hesap admin değildir. (Veritabanı rolü: " +
                    string.Join(",", await _userManager.GetRolesAsync(user)) + ")";
                return RedirectToAction("Index", "Home");
            }

            if (!user.IsActive)
            {
                TempData["AdminLoginError"] = "Hesabınız pasif durumdadır.";
                return RedirectToAction("Index", "Home");
            }

            // lockoutOnFailure: false yaparak çok fazla denemede hesabın kilitlenmesini önlüyoruz
            var result = await _signInManager.PasswordSignInAsync(user, password_name, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("TourList", "AdminTour");
            }

            // Eğer şifre doğru ama yine de girmiyorsa (MongoDB SecurityStamp hatası olabilir)
            if (result.IsNotAllowed)
            {
                TempData["AdminLoginError"] = "Giriş izni verilmedi (Email onayı gerekebilir veya hesap kilitli).";
            }
            else
            {
                TempData["AdminLoginError"] = "Admin adı veya şifre hatalı.";
            }

            return RedirectToAction("Index", "Home");
        }

        // ==================== ADMIN KAYIT ====================
        [HttpPost]
        public async Task<IActionResult> AdminRegister(string user_name, string user_email, string password_name, string repassword_name)
        {
            if (password_name != repassword_name)
            {
                TempData["AdminRegisterError"] = "Şifreler eşleşmiyor.";
                return RedirectToAction("Index", "Home");
            }

            var existingUser = await _userManager.FindByNameAsync(user_name);
            if (existingUser != null)
            {
                TempData["AdminRegisterError"] = "Bu kullanıcı adı zaten kullanılıyor.";
                return RedirectToAction("Index", "Home");
            }

            var user = new AppUser
            {
                UserName = user_name,
                Email = user_email,
                FullName = user_name,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, password_name);
            if (result.Succeeded)
            {
                // Yeni admin'i Admin rolüne ata
                await _userManager.AddToRoleAsync(user, "Admin");
                await _signInManager.SignInAsync(user, isPersistent: true);
                TempData["AdminRegisterSuccess"] = "Admin kayıt başarılı! Panele yönlendiriliyorsunuz.";
                return RedirectToAction("TourList", "AdminTour");
            }

            TempData["AdminRegisterError"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToAction("Index", "Home");
        }

        // ==================== LOGOUT ====================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
