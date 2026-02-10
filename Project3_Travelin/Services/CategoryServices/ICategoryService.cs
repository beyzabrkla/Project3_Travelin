using Project3_Travelin.DTOs.CategoryDTOs;

namespace Project3_Travelin.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDTO>> GetAllCategoryAsync();
        Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO);
        Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO);
        Task DeleteCategoryAsync(string id);
        Task<GetCategoryByIdDTO> GetCategoryByIdAsync(string id);
    }
}
