using MongoDB.Bson.Serialization.Attributes;

namespace DTOLayer.DTOs.TourDTOs
{
    public class CreateTourDTO
    {
        public string TourTitle { get; set; }
        public string TourCity { get; set; }
        public string TourCountry { get; set; }
        public string SubDescription { get; set; }
        public int TourCapacity { get; set; }
        public DateTime TourDate { get; set; }
        public string DayNight { get; set; }
        public string ImageUrl { get; set; }
        public decimal TourPrice { get; set; } // Tur fiyatı
        public string FullDescription { get; set; }
        public bool IsDrafts { get; set; }
        public bool IsStatus { get; set; }
        public List<string> ImageAlbumUrls { get; set; } = new List<string>();

        public string GuideId { get; set; } 
    }
}
