using DataAccessLayer.Abstract;
using EntityLayer.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IDatabaseSettings _databaseSettings, string collectionName)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<T>(collectionName);
        }
        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", ObjectId.Parse(id)));

        public async Task<List<T>> GetAllAsync() =>
            await _collection.Find(x => true).ToListAsync();

        public async Task<T> GetByIdAsync(string id)
        {
            // Eğer id null ise veya MongoDB formatına uygun değilse (24 karakterli hex) hata fırlatma, null dön.
            if (string.IsNullOrEmpty(id) || id.Length != 24)
            {
                return null;
            }

            return await _collection.Find(Builders<T>.Filter.Eq("_id", ObjectId.Parse(id))).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(T t) =>
            await _collection.InsertOneAsync(t);

        public async Task UpdateAsync(T t)
        {
            var idProperty = t.GetType().GetProperties().FirstOrDefault(p => p.Name.Contains("Id"));
            var idValue = idProperty?.GetValue(t)?.ToString();

            if (!string.IsNullOrEmpty(idValue))
            {
                var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(idValue));
                await _collection.ReplaceOneAsync(filter, t);
            }
        }

        public async Task<List<T>> GetByFilterAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter) =>
            await _collection.Find(filter).ToListAsync();
    }
}