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
        public string Description { get; set; } //rehberin genel açıklaması
        public bool Status { get; set; }
    }
}
