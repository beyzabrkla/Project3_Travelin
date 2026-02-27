using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

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

        // --- GÖRSEL YÖNETİMİ ---
        public string? ImageUrl { get; set; } // Veritabanındaki dosya adı
        public IFormFile? ImageFile { get; set; } // Bilgisayardan seçilen yeni ana görsel

        public string? MapImageUrl { get; set; } // Veritabanındaki harita dosya adı
        public IFormFile? MapImageFile { get; set; } // Bilgisayardan seçilen yeni harita görseli
        public string? VideoUrl { get; set; }  // var mı yok mu?
        public decimal TourPrice { get; set; }
        public string FullDescription { get; set; }
        public bool IsDrafts { get; set; }
        public bool IsStatus { get; set; }
        public bool IsVisaRequired { get; set; }
        public List<string> ImageAlbumUrls { get; set; }
        public List<IFormFile>? GuideImages { get; set; }
        public string GuideId { get; set; }
        public string GuideName { get; set; }
        public string GuideTitle { get; set; }
        public string GuideImageUrl { get; set; }
        public List<string> GuideAlbumUrls { get; set; }
        public string GuideDescription { get; set; }
    }
}