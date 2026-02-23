namespace DTOLayer.DTOs.CategoryDTOs
{
    public class GetCategoryByIdDTO
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string IconUrl { get; set; }
        public bool IsStatus { get; set; }
    }
}
