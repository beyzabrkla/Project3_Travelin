using AutoMapper;
using MongoDB.Driver;
using EntityLayer;
using DTOLayer.DTOs.CategoryDTOs;
using BusinessLayer.Settings;

namespace BusinessLayer.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection <Category> _categoryCollection; //field örneklemesi 

        public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString); //veri tabanı adresine ulaş
            var database = client.GetDatabase(_databaseSettings.DatabaseName); //veri tabanı adına ulaş
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName); //veri tabanı tablosuna ulaş
            _mapper = mapper;

        }

        public async Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            var values = _mapper.Map<Category>(createCategoryDTO); //
            await _categoryCollection.InsertOneAsync(values); //mongodb de ekleme işlemi 
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryCollection.DeleteOneAsync(x=>x.CategoryId==id); //mongodb silme işlemi
        }

        public async Task<List<ResultCategoryDTO>> GetAllCategoryAsync()
        {
            var values = await _categoryCollection.Find(x=>true).ToListAsync(); //filtreleme olmadan tüm kayıtları getirme
            return _mapper.Map<List<ResultCategoryDTO>>(values); //dtodaki değerleri valuesdeki değerlere eşitledik
        }

        public async Task<GetCategoryByIdDTO> GetCategoryByIdAsync(string id)
        {
            var value = await _categoryCollection.Find(x=>x.CategoryId ==id).FirstOrDefaultAsync(); //
            return _mapper.Map<GetCategoryByIdDTO>(value);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO)
        {
            var values = _mapper.Map<Category>(updateCategoryDTO);
            await _categoryCollection.FindOneAndReplaceAsync(x => x.CategoryId == updateCategoryDTO.CategoryId, values);
        }
    }
}
