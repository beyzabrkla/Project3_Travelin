using MongoDB.Bson.Serialization.Attributes;

namespace DTOLayer.DTOs.TourDTOs
{
    public class UpdateTourDTO
    {
        public string TourId { get; set; }
        public string TourTitle { get; set; }
        public string TourCity { get; set; }
        public string TourCountry { get; set; }
        public string SubDescription { get; set; }
        public int TourCapacity { get; set; }
        public DateTime TourDate { get; set; }
        public string DayNight { get; set; }
        public string ImageUrl { get; set; }
        public decimal TourPrice { get; set; }
        public string FullDescription { get; set; }
        public bool IsDrafts { get; set; }
        public bool IsStatus { get; set; }
        public List<string> ImageAlbumUrls { get; set; }

        public string GuideId { get; set; }

        public string GuideName { get; set; }

        public string GuideTitle { get; set; }

        public string GuideImageUrl { get; set; }
        public string GuideDescription { get; set; }

    }
}
