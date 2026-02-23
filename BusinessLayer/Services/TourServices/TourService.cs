using AutoMapper;
using BusinessLayer.Settings;
using DTOLayer.DTOs.TourDTOs;
using EntityLayer;
using MongoDB.Driver;

namespace BusinessLayer.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Guide> _guideCollection;
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings) // guideCollection'ı parametreden çıkardık
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _guideCollection = database.GetCollection<Guide>(_databaseSettings.GuideCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDTO createTourDTO)
        {
            var values = _mapper.Map<Tour>(createTourDTO);
            await _tourCollection.InsertOneAsync(values);
        }

        public async Task DeleteTourAsync(string id)
        {
           await _tourCollection.DeleteOneAsync(x=>x.TourId ==id);
        }

        public async Task<List<ResultTourDTO>> GetAllTourAsync()
        {
            var values = await _tourCollection.Find(x=>true).ToListAsync();
            return _mapper.Map<List<ResultTourDTO>>(values);
        }

        public async Task<GetTourByIdDTO> GetTourByIdAsync(string id)
        {
            var tour = await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            var tourDto = _mapper.Map<GetTourByIdDTO>(tour);

            if (tour != null && !string.IsNullOrEmpty(tour.GuideId))
            {
                var guide = await _guideCollection.Find(x => x.GuideId == tour.GuideId).FirstOrDefaultAsync();
                if (guide != null)
                {
                    tourDto.GuideName = guide.Name;
                    tourDto.GuideImageUrl = guide.ImageUrl;
                    tourDto.GuideTitle = guide.Title;
                }
            }
            return tourDto;
        }

        public async Task<List<ResultTourDTO>> GetToursByGuideIdAsync(string guideId)
        {
            var values = await _tourCollection.Find(x => x.GuideId == guideId).ToListAsync();
            return _mapper.Map<List<ResultTourDTO>>(values);
        }

        public async Task UpdateTourAsync(UpdateTourDTO updateTourDTO)
        {
            var values = _mapper.Map<Tour>(updateTourDTO);
            await _tourCollection.FindOneAndReplaceAsync(x=>x.TourId ==updateTourDTO.TourId, values);
        }
    }
}
