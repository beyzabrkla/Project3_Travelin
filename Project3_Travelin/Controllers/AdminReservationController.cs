using BusinessLayer.Abstract;
using DTOLayer.DTOs.ReservationDTOs; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public AdminReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> ReservationList(string status)
        {
            var values = await _reservationService.GetAllReservationAsync();

            if (!string.IsNullOrEmpty(status))
            {
                values = values.Where(x => x.Status != null && x.Status.ToLower() == status.ToLower()).ToList();
            }

            ViewBag.Status = status;
            return View(values);
        }

        public async Task<IActionResult> ApproveReservation(string id)
        {
            var updateDto = new UpdateReservationStatusDTO
            {
                ReservationId = id,
                Status = "Approved",
                AdminNote = "Admin tarafından onaylandı."
            };

            await _reservationService.UpdateReservationStatusAsync(updateDto);
            return RedirectToAction("ReservationList");
        }

        public async Task<IActionResult> RejectReservation(string id)
        {
            var updateDto = new UpdateReservationStatusDTO
            {
                ReservationId = id,
                Status = "Rejected",
                AdminNote = "Üzgünüz, kontenjan doluluğu veya başka bir sebeple reddedildi."
            };

            await _reservationService.UpdateReservationStatusAsync(updateDto);
            return RedirectToAction("ReservationList");
        }
    }
}