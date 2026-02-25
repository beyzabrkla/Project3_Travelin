namespace DTOLayer.DTOs.GuideDTOs
{
    public class ResultGuideDTO
    {
        // ==================== TEMEL BİLGİLER ====================
        public string GuideId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }

        /// Rehberin UserId'si (AppUser.Id)
        public string UserId { get; set; }

        /// Uzmanlaştığı bölgeler
        public string Specialization { get; set; }

        /// Deneyim yılı
        public int? Experience { get; set; }

        /// Konuştuğu diller
        public string Languages { get; set; }

        /// Rehber puanı
        public decimal Rating { get; set; }

        /// Yaptığı toplam tur sayısı
        public int TotalTours { get; set; }

        /// Uygunluk durumu: Available, Busy, OnLeave
        public string AvailabilityStatus { get; set; }
    }
}