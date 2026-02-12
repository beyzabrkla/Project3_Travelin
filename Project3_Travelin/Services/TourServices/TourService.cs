using AutoMapper;
using MongoDB.Driver;
using Project3_Travelin.DTOs.TourDTOs;
using Project3_Travelin.Entities;
using Project3_Travelin.Settings;

namespace Project3_Travelin.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString); //mongo dbye bağlantı adresini çekiyoruz
            var database = client.GetDatabase(_databaseSettings.DatabaseName); //kullanılacak veritabanı adını alıyoruz
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName); //kullanılacak tablo
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
            var value = await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDTO>(value);
        }

        public async Task UpdateTourAsync(UpdateTourDTO updateTourDTO)
        {
            var values = _mapper.Map<Tour>(updateTourDTO);
            await _tourCollection.FindOneAndReplaceAsync(x=>x.TourId ==updateTourDTO.TourId, values);
        }
    }
}
