using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string NotificationId { get; set; }

        public string UserId { get; set; }         // hangi kullanıcıya
        public string Title { get; set; }          // "Rezervasyonunuz Onaylandı"
        public string Message { get; set; }        // detay mesaj
        public string Type { get; set; }           // "reservation_approved" | "reservation_rejected"
        public string RelatedId { get; set; }      // ReservationId
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
