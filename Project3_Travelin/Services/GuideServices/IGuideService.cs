using Project3_Travelin.DTOs.GuideDTOs;

namespace Project3_Travelin.Services.GuideServices
{
    public interface IGuideService
    {
        Task<List<ResultGuideDTO>> GetAllGuideAsync();
        Task CreateGuideAsync(CreateGuideDTO createGuideDTO);
        Task UpdateGuideAsync(UpdateGuideDTO updateGuideDTO);
        Task<GetGuideByIdDTO> GetGuideByIdAsync(string id);
        Task ChangeGuideStatusAsync(string id);
    }
}
