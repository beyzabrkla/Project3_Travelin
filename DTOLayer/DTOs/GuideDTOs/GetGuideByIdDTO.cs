namespace DTOLayer.DTOs.GuideDTOs
{
    public class GetGuideByIdDTO
    {
        // ==================== TEMEL BİLGİLER ====================
        public string GuideId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        /// Rehberin UserId'si (AppUser.Id)
        public string UserId { get; set; }

        /// İlişkili GuideApplication ID'si
        public string GuideApplicationId { get; set; }

        // ==================== REHBERLİK BİLGİLERİ ====================
        /// Uzmanlaştığı bölgeler
        public string Specialization { get; set; }

        /// Deneyim yılı
        public int? Experience { get; set; }

        /// Konuştuğu diller
        public string Languages { get; set; }
        public string Phone { get; set; }

        /// Uygun çalışma saatleri
        public string AvailableHours { get; set; }

        /// Hakkında bilgi
        public string About { get; set; }

        // ==================== REHBER METRIKLERI ====================
        /// Rehber puanı
        public decimal Rating { get; set; }

        /// Yaptığı toplam tur sayısı
        public int TotalTours { get; set; }

        /// Uygunluk durumu: Available, Busy, OnLeave
        public string AvailabilityStatus { get; set; }

        /// Rehber onay tarihi
        public DateTime? ApprovedDate { get; set; }

        /// Oluşturma tarihi
        public DateTime CreatedAt { get; set; }

        /// Son güncelleme tarihi
        public DateTime? UpdatedAt { get; set; }

    }
}