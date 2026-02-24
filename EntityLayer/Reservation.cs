using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReservationId { get; set; }
        public string TourId { get; set; }
        public string TourTitle { get; set; }  
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int PersonCount { get; set; }

        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "pending"; // pending | approved | rejected

        public string AdminNote { get; set; }      // admin reddettiğinde not yazabilir
    }
}
