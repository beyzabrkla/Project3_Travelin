using AutoMapper;
using BusinessLayer.Services.GuideServices;
using BusinessLayer.Settings;
using DTOLayer.DTOs.GuideDTOs;
using EntityLayer;
using MongoDB.Driver;

public class GuideService : IGuideService
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Guide> _guideCollection;

    public GuideService(IMapper mapper, IDatabaseSettings _databaseSettings)
    {
        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);
        _guideCollection = database.GetCollection<Guide>(_databaseSettings.GuideCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultGuideDTO>> GetAllGuideAsync()
    {
        var values = await _guideCollection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultGuideDTO>>(values);
    }

    public async Task CreateGuideAsync(CreateGuideDTO createGuideDTO)
    {
        var value = _mapper.Map<Guide>(createGuideDTO);
        await _guideCollection.InsertOneAsync(value);
    }

    public async Task UpdateGuideAsync(UpdateGuideDTO updateGuideDTO)
    {
        var value = _mapper.Map<Guide>(updateGuideDTO);
        await _guideCollection.FindOneAndReplaceAsync(x => x.GuideId == updateGuideDTO.GuideId, value);
    }

    public async Task ChangeGuideStatusAsync(string id)
    {
        var value = await _guideCollection.Find(x => x.GuideId == id).FirstOrDefaultAsync();
        if (value != null)
        {
            // Durum true ise false, false ise true yap
            value.Status = !value.Status;
            await _guideCollection.FindOneAndReplaceAsync(x => x.GuideId == id, value);
        }
    }

    public async Task<GetGuideByIdDTO> GetGuideByIdAsync(string id)
    {
        var value = await _guideCollection.Find(x => x.GuideId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetGuideByIdDTO>(value);
    }
}