using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EntityLayer
{
    public class Guide
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GuideId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; } // Örn: Profesyonel Dağcı, Tarihçi
        public string ImageUrl { get; set; }
        public string Description { get; set; } // Rehberin genel açıklaması

        [BsonIgnoreIfNull]
        public bool Status { get; set; }

        public string UserId { get; set; }
   
        public string Specialization { get; set; }
        public int? Experience { get; set; }

        public string Languages { get; set; }

        public string Phone { get; set; }

        public string AvailableHours { get; set; }

        public string About { get; set; }

        public decimal Rating { get; set; } = 5.0m;

        public int TotalTours { get; set; } = 0;
        public string AvailabilityStatus { get; set; } = "Available";

        public DateTime? ApprovedDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}