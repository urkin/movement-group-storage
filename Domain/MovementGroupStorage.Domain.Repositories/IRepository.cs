using MovementGroupStorage.Domain.Entities;

namespace MovementGroupStorage.Domain.Repositories
{
    /// <summary>
    /// Defines a generic repository.
    /// </summary>
    /// <typeparam name="TKey">The type of entities key</typeparam>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IRepository<TKey, TEntity> where TEntity : IEntity<TKey>, new()
    {
        /// <summary>
        /// Finds the entity by it's identifier within the database.
        /// </summary>
        /// <param name="id">The identifier to find entity by.</param>
        /// <returns>Entity if found, otherwise null.</returns>
        Task<TEntity?> FindAsync(TKey id);

        /// <summary>
        /// Inserts an entity into the database.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>The inserted entity with the generated id.</returns>
        Task<TEntity> InsertAsync(TEntity entity);
    }
}
