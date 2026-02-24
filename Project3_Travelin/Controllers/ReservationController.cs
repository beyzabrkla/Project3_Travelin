using BusinessLayer.Abstract;
using DTOLayer.DTOs.ReservationDTOs;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Project3_Travelin.Controllers
{
    //[Authorize(Roles = "Customer")]
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IReservationService _reservationService;
        private readonly UserManager<AppUser> _userManager;

        public ReservationController(ITourService tourService, IReservationService reservationService, UserManager<AppUser> userManager)
        {
            _tourService = tourService;
            _reservationService = reservationService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> NewReservation(string id = null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allTours = await _tourService.GetAllTourAsync(); // Tüm turları çekiyoruz

            ViewBag.TourList = allTours.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.TourTitle,
                Value = x.TourId,
                Selected = x.TourId == id // Eğer ID ile gelmişse o turu seçili getir
            }).ToList();

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
            if (!ModelState.IsValid)
            {
                return View(createReservationDTO);
            }

            await _reservationService.CreateReservationAsync(createReservationDTO);

            return RedirectToAction("MyReservations", "Customer");
        }

        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var allReservations = await _reservationService.GetAllReservationAsync();
            var userReservations = allReservations.Where(x => x.CustomerEmail == user.Email).ToList();

            return View(userReservations);
        }
    }
}