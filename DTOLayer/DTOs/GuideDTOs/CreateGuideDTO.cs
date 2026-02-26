namespace DTOLayer.DTOs.GuideDTOs
{
    public class CreateGuideDTO
    {
        // ==================== TEMEL BİLGİLER ====================
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        // ==================== BAŞVURU İLİŞKİ ====================
        public string UserId { get; set; }

        // ==================== REHBERLİK BİLGİLERİ ====================
        public string Specialization { get; set; }
        public int? Experience { get; set; }
        public string Languages { get; set; }
        public string Phone { get; set; }

        /// Uygun çalışma saatleri
        public string? AvailableHours { get; set; }
        public string? AvailabilityStatus { get; set; }

        /// Hakkında bilgi (Başvuru formundan gelen)
        public string About { get; set; }
        public string? CreatedAt { get; set; }

    }
}