using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.v1 = "Hoş Geldiniz!";
            return View();
        }

        // Aktif Rezervasyonlar
        public IActionResult MyReservations()
        {
            return View();
        }

        // Geçmiş Rezervasyonlar
        public IActionResult OldReservations()
        {
            return View();
        }

        // Profil Bilgileri ve Güncelleme
        public IActionResult Profile()
        {
            return View();
        }

        // Kullanıcının yaptığı yorumlar
        public IActionResult MyReviews()
        {
            return View();
        }
    }
}