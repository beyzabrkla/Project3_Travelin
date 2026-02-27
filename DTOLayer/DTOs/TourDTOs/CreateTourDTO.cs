using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

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

        // --- GÖRSEL YÖNETİMİ ---
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; } // Bilgisayardan ana görsel seçimi

        public string? MapImageUrl { get; set; }
        public IFormFile? MapImageFile { get; set; } // Bilgisayardan harita seçimi

        public string? VideoUrl { get; set; }  // var mı yok mu?
        public decimal TourPrice { get; set; }
        public string FullDescription { get; set; }
        public bool IsDrafts { get; set; }
        public bool IsStatus { get; set; }
        public bool IsVisaRequired { get; set; }

        public List<string> ImageAlbumUrls { get; set; } = new List<string>();
        public string GuideId { get; set; }
    }
}