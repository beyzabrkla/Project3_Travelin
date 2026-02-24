using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOs.ReservationDTOs
{
    public class UpdateReservationStatusDTO
    {
        public string ReservationId { get; set; }
        public string Status { get; set; }
        public string AdminNote { get; set; }
    }
}
