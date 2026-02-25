using BusinessLayer.Abstract;
using DTOLayer.DTOs.ReservationDTOs; 
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;
        private readonly IGuideService _guideService;
        private readonly UserManager<AppUser> _userManager;

        public CustomerController(IReservationService reservationService, UserManager<AppUser> userManager, ITourService tourService, IGuideService guideService)
        {
            _reservationService = reservationService;
            _userManager = userManager;
            _tourService = tourService;
            _guideService = guideService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction("SignIn", "Login");

            ViewBag.FullName = user.FullName;

            var allReservations = await _reservationService.GetAllReservationAsync();
            var userReservations = allReservations.Where(x => x.CustomerEmail == user.Email).ToList();

            ViewBag.ActiveCount = userReservations.Count(x => x.Status?.ToLower() == "approved");
            ViewBag.PendingCount = userReservations.Count(x => x.Status?.ToLower() == "pending");
            ViewBag.TotalCount = userReservations.Count;

            var latestReservations = userReservations
                .OrderByDescending(x => x.ReservationDate)
                .Take(5)
                .ToList();

            return View(latestReservations);
        }

        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allReservations = await _reservationService.GetAllReservationAsync();

            var activeReservations = allReservations
                .Where(x => x.CustomerEmail == user.Email && (x.Status?.ToLower() == "approved" || x.Status?.ToLower() == "pending"))
                .OrderByDescending(x => x.ReservationDate)
                .ToList();

            return View(activeReservations);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationDetail([FromQuery] string id)
        {
            var allReservations = await _reservationService.GetAllReservationAsync();
            var res = allReservations.FirstOrDefault(x => x.ReservationId == id);
            if (res == null) return NotFound();

            var tour = await _tourService.GetTourByIdAsync(res.TourId);

            // Rehberi kendi tablosundan ID ile çekiyoruz
            var guide = (tour != null && !string.IsNullOrEmpty(tour.GuideId))
                        ? await _guideService.GetGuideByIdAsync(tour.GuideId)
                        : null;

            var result = new
            {
                tourTitle = res.TourTitle,
                description = tour?.SubDescription ?? "Açıklama bulunmuyor.",
                date = res.ReservationDate.ToString("dd.MM.yyyy"),
                personCount = res.PersonCount,
                customerName = res.CustomerName,
                customerPhone = res.CustomerPhone,

                // Rehber bilgileri (guide nesnesinden geliyor)
                guideName = guide?.Name ?? "Rehber Atanmadı",
                guideTitle = guide?.Title ?? "Tur Rehberi",
                guideImage = guide?.ImageUrl ?? "/images/default-guide.png"
            };
            return Json(result);

        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

        public IActionResult MyReviews()
        {
            return View();
        }
    }
}