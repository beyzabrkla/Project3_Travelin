using DTOLayer.DTOs.GuideDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGuideService
    {
        Task<List<ResultGuideDTO>> GetAllGuideAsync();
        Task CreateGuideAsync(CreateGuideDTO createGuideDTO);
        Task UpdateGuideAsync(UpdateGuideDTO updateGuideDTO);
        Task<GetGuideByIdDTO> GetGuideByIdAsync(string id);
        Task ChangeGuideStatusAsync(string id);
        Task DeleteGuideAsync(string id);
    }
}
