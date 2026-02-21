using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3_Travelin.Entities
{
    public class Tour
    { 
        //bu komutlar mongo db deki primary key ve onun veri türüne karşılık geliyor
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
        public List<string> ImageAlbumUrls { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TourPrice { get; set; } // Tur fiyatı

        [BsonRepresentation(BsonType.ObjectId)]
        public string GuideId { get; set; }
    }
}
