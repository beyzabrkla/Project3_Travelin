using AutoMapper;
using BusinessLayer.Settings;
using DTOLayer.DTOs.CommentDTOs;
using EntityLayer;
using MongoDB.Driver;

namespace BusinessLayer.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Comment> _commentCollection; //field örneklemesi 

        public CommentService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _commentCollection = database.GetCollection<Comment>(_databaseSettings.CommentCollectionName);
            _mapper = mapper;
        }

        public async Task CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            var values= _mapper.Map<Comment>(createCommentDTO);
            await _commentCollection.InsertOneAsync(values);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentCollection.DeleteOneAsync(x=>x.CommentId ==id);
        }

        public async Task<List<ResultCommentDTO>> GetAllCommentAsync()
        {
            var values = await _commentCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCommentDTO>>(values);
        }

        public async Task<GetCommentByIdDTO> GetCommentByIdAsync(string id)
        {
            var value = await _commentCollection.Find(x => x.CommentId == id).FirstOrDefaultAsync(); 
            return _mapper.Map<GetCommentByIdDTO>(value);
        }

        public async Task UpdateCommentAsync(UpdateCommentDTO updateCommentDTO)
        {
            var value = _mapper.Map<Comment>(updateCommentDTO);
            await _commentCollection.FindOneAndReplaceAsync(x => x.CommentId == updateCommentDTO.CommentId, value);
        }
    }
}
