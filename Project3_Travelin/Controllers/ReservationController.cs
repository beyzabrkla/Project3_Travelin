using BusinessLayer.Abstract;
using DTOLayer.DTOs.ReservationDTOs;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IReservationService _reservationService;
        private readonly IGuideService _guideService;
        private readonly UserManager<AppUser> _userManager;

        public ReservationController(ITourService tourService, IReservationService reservationService, UserManager<AppUser> userManager, IGuideService guideService)
        {
            _tourService = tourService;
            _reservationService = reservationService;
            _userManager = userManager;
            _guideService = guideService;
        }

        [HttpGet]
        public async Task<IActionResult> NewReservation(string id = null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allTours = await _tourService.GetAllTourAsync();
            var activeTours = allTours.Where(x => x.IsStatus == true && x.IsDrafts == false).ToList();

            ViewBag.TourList = activeTours.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.TourTitle,
                Value = x.TourId,
                Selected = x.TourId == id
            }).ToList();

            ViewBag.TourPrices = activeTours.ToDictionary(x => x.TourId, x => x.TourPrice);

            var model = new CreateReservationDTO
            {
                CustomerName = user?.FullName,
                CustomerEmail = user?.Email,
                CustomerPhone = user?.PhoneNumber,
                PersonCount = 1,
                TourId = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewReservation(CreateReservationDTO createReservationDTO)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Login");

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (!ModelState.IsValid)
            {
                if (isAjax) return Json(new { success = false, message = "Lütfen tüm alanları doğru doldurunuz." });

                var allTours = await _tourService.GetAllTourAsync();
                var activeTours = allTours.Where(x => x.IsStatus == true && x.IsDrafts == false).ToList();

                ViewBag.TourList = activeTours.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.TourTitle,
                    Value = x.TourId,
                    Selected = x.TourId == createReservationDTO.TourId
                }).ToList();

                return View(createReservationDTO);
            }

            await _reservationService.CreateReservationAsync(createReservationDTO);

            if (isAjax)
            {
                return Json(new { success = true, message = "Rezervasyonunuz başarıyla oluşturuldu ve mail adresinize iletildi!" });
            }

            TempData["SuccessMessage"] = "Rezervasyonunuz başarıyla oluşturuldu!";
            return RedirectToAction("MyReservations");
        }


        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction("SignIn", "Login");

            var allReservations = await _reservationService.GetAllReservationAsync();
            var userReservations = allReservations.Where(x => x.CustomerEmail == user.Email).ToList();

            return View(userReservations);
        }

        // --- DİJİTAL BİLET VERİSİ ---
        [HttpGet]
        public async Task<IActionResult> GetReservationDetails(string id)
        {
            var allReservations = await _reservationService.GetAllReservationAsync();
            var res = allReservations.FirstOrDefault(x => x.ReservationId == id);

            if (res == null) return NotFound();

            var allTours = await _tourService.GetAllTourAsync();
            var tour = allTours.FirstOrDefault(x => x.TourId == res.TourId);

            // REHBER BİLGİSİNİ ÇEKME MANTIĞI
            string guideName = "Rehber Atanmadı";
            string guideImage = "/images/default-guide.png";

            if (tour != null && !string.IsNullOrEmpty(tour.GuideId))
            {
                var guide = await _guideService.GetGuideByIdAsync(tour.GuideId);
                if (guide != null)
                {
                    guideName = guide.Name;
                    guideImage = !string.IsNullOrEmpty(guide.ImageUrl) ? guide.ImageUrl : guideImage;
                }
            }

            decimal unitPrice = tour?.TourPrice ?? 0;
            decimal totalPrice = res.PersonCount * unitPrice;

            return Json(new
            {
                title = res.TourTitle,
                date = res.ReservationDate.ToString("dd MMMM yyyy"),
                person = res.PersonCount,
                customer = res.CustomerName,
                status = res.Status,
                guide = guideName, // Rehber tablosundan gelen isim
                guideImage = guideImage, // Rehber tablosundan gelen görsel
                unitPrice = unitPrice.ToString("C0"),
                totalPrice = totalPrice.ToString("C0")
            });
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(string id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction("SignIn", "Login");

            var allReservations = await _reservationService.GetAllReservationAsync();
            var reservation = allReservations.FirstOrDefault(x => x.ReservationId == id);

            if (reservation != null && reservation.CustomerEmail == user.Email)
            {
                if (reservation.Status?.ToLower() == "approved")
                {
                    TempData["ErrorMessage"] = "Onaylanmış rezervasyonlar silinemez, lütfen destekle iletişime geçin.";
                    return RedirectToAction("MyReservations");
                }

                // VERİTABANINDAN TAMAMEN SİLME
                await _reservationService.DeleteReservationAsync(id);

                TempData["SuccessMessage"] = "Rezervasyon talebiniz başarıyla silindi.";
            }

            return RedirectToAction("MyReservations");
        }
    }
}