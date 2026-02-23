using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DTOLayer.DTOs.TourDTOs;
using EntityLayer;

namespace BusinessLayer.Concrete
{
    public class TourManager : ITourService
    {
        private readonly IMapper _mapper;
        private readonly ITourDal _tourDal;

        public TourManager(IMapper mapper, ITourDal tourDal)
        {
            _mapper = mapper;
            _tourDal = tourDal;
        }

        public async Task CreateTourAsync(CreateTourDTO createTourDTO)
        {
            var value = _mapper.Map<Tour>(createTourDTO);
            await _tourDal.InsertAsync(value);
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourDal.DeleteAsync(id);
        }

        public async Task<List<ResultTourDTO>> GetAllTourAsync()
        {
            var values = await _tourDal.GetAllAsync();
            var mappedValues= _mapper.Map<List<ResultTourDTO>>(values);
            return mappedValues;
        }

        public async Task<GetTourByIdDTO> GetTourByIdAsync(string id)
        {
           var value = await _tourDal.GetByIdAsync(id);
           return _mapper.Map<GetTourByIdDTO>(value);
        }

        public async Task<List<ResultTourDTO>> GetToursByGuideIdAsync(string guideId)
        {
            var values = await _tourDal.GetByFilterAsync(x => x.GuideId == guideId);

            return _mapper.Map<List<ResultTourDTO>>(values);
        }

        public async Task UpdateTourAsync(UpdateTourDTO updateTourDTO)
        {
           var value = _mapper.Map<Tour>(updateTourDTO);
           await _tourDal.UpdateAsync(value);
        }
    }
}
