
using DTOLayer.DTOs.GuideDTOs;

namespace BusinessLayer.Services.GuideServices
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
