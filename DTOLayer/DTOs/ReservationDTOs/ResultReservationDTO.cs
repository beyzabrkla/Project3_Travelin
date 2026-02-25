using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOs.ReservationDTOs
{
    public class ResultReservationDTO
    {
        public string ReservationId { get; set; }
        public string TourId { get; set; }
        public string GuideId { get; set; }
        public string GuideName { get; set; }
        public string GuideTitle { get; set; }
        public string GuideImageUrl { get; set; }
        public string TourTitle { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int PersonCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; }
        public string AdminNote { get; set; }
    }
}
