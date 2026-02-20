using MongoDB.Driver;
using MovementGroupStorage.Domain.Entities;
using MovementGroupStorage.Domain.Repositories;
using MovementGroupStorage.Infrastructure.Domain.Repositories.Extensions;

namespace MovementGroupStorage.Infrastructure.Domain.Repositories
{
    /// <summary>
    /// Generic MongoDB <see cref="IRepository{TKey, TEntity}"/> implementation.
    /// </summary>
    /// <typeparam name="TKey">The type of entities' key</typeparam>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public class MongoDbRepository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>, new()
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public MongoDbRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TEntity>();
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity?> FindAsync(TKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(kv => kv.Id, id);
            var entity = await _collection.Find(filter).FirstOrDefaultAsync();
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }
    }
}
