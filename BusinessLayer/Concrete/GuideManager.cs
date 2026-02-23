using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DTOLayer.DTOs.GuideDTOs;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class GuideManager : IGuideService
    {
        private readonly IGuideDal _guideDal;
        private readonly IMapper _mapper;

        public GuideManager(IGuideDal guideDal, IMapper mapper)
        {
            _guideDal = guideDal;
            _mapper = mapper;
        }

        public async Task<List<ResultGuideDTO>> GetAllGuideAsync()
        {
            var values = await _guideDal.GetAllAsync();
            return _mapper.Map<List<ResultGuideDTO>>(values);
        }

        public async Task CreateGuideAsync(CreateGuideDTO createGuideDTO)
        {
            var value = _mapper.Map<Guide>(createGuideDTO);
            await _guideDal.InsertAsync(value);
        }

        public async Task UpdateGuideAsync(UpdateGuideDTO updateGuideDTO)
        {
            var value = _mapper.Map<Guide>(updateGuideDTO);
            await _guideDal.UpdateAsync(value);
        }

        public async Task<GetGuideByIdDTO> GetGuideByIdAsync(string id)
        {
            var value = await _guideDal.GetByIdAsync(id);
            return _mapper.Map<GetGuideByIdDTO>(value);
        }

        public async Task ChangeGuideStatusAsync(string id)
        {
            var guide = await _guideDal.GetByIdAsync(id);
            if (guide != null)
            {
                // Durumu tersine çevir (True ise False, False ise True yap)
                guide.Status = !guide.Status;
                await _guideDal.UpdateAsync(guide);
            }
        }
    }
}
