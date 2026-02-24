using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOs.ReservationDTOs
{
    public class CreateReservationDTO
    {
        public string TourId { get; set; }
        public string TourTitle { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int PersonCount { get; set; }
    }
}
