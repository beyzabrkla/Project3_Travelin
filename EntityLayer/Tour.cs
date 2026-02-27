using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EntityLayer
{
    [BsonIgnoreExtraElements]
    public class Tour
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }
        public string TourTitle { get; set; }
        public string TourCity { get; set; }
        public string TourCountry { get; set; }
        public string SubDescription { get; set; }
        public string FullDescription { get; set; }
        public int TourCapacity { get; set; }
        public DateTime TourDate { get; set; }
        public string DayNight { get; set; }
        public string ImageUrl { get; set; }
        public string MapImageUrl { get; set; } // Harita görseli URL'i
        public bool IsDrafts { get; set; }
        public bool IsStatus { get; set; }
        public List<string> ImageAlbumUrls { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TourPrice { get; set; }
        public string GuideId { get; set; }
        public string GuideDescription { get; set; }
        public string GuideImage { get; set; }
        public List<string> GuideAlbumUrls { get; set; }
        public string VideoUrl { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsVisaRequired { get; set; }
    }
}
