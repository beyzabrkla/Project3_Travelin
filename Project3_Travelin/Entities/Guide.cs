using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3_Travelin.Entities
{
    public class Guide
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GuideId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; } // Örn: Profesyonel Dağcı, Tarihçi
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
