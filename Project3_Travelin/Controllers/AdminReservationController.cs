using BusinessLayer.Abstract;
using DTOLayer.DTOs.ReservationDTOs;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

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

            // Mail Gönderme İşlemi
            var reservation = (await _reservationService.GetAllReservationAsync()).FirstOrDefault(x => x.ReservationId == id);
            if (reservation != null)
            {
                SendStatusMail(reservation.CustomerEmail, "Rezervasyonunuz Onaylandı! ✈️",
                    $"Sayın {reservation.CustomerName}, {reservation.TourTitle} turu için yaptığınız rezervasyon onaylanmıştır. İyi yolculuklar dileriz!");
            }

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

            // Mail Gönderme İşlemi
            var reservation = (await _reservationService.GetAllReservationAsync()).FirstOrDefault(x => x.ReservationId == id);
            if (reservation != null)
            {
                SendStatusMail(reservation.CustomerEmail, "Rezervasyon Durumu Hakkında Bilgilendirme",
                    $"Sayın {reservation.CustomerName}, maalesef {reservation.TourTitle} turu için yaptığınız rezervasyon reddedilmiştir.");
            }

            return RedirectToAction("ReservationList");
        }

        private void SendStatusMail(string receiverMail, string subject, string body)
        {
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Travelin Rezervasyon", "beyzailetisimapp@gmail.com"));
            mimeMessage.To.Add(new MailboxAddress("Gezgin", receiverMail));

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = subject;

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("beyzailetisimapp@gmail.com", "qrmoackfzratkiky");
                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }
    }
}