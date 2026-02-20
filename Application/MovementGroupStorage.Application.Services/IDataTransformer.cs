namespace MovementGroupStorage.Application.Services
{
    /// <summary>
    /// Defines service that transforms data from <see cref="TSource"/> to <see cref="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of source.</typeparam>
    /// <typeparam name="TDestination">The type of destination.</typeparam>
    public interface IDataTransformer<TSource, TDestination>
    {
        /// <summary>
        /// Transforms data from <see cref="TSource"/> to <see cref="TDestination"/>.
        /// </summary>
        /// <param name="source">An instance of <see cref="TSource"/> to transform data from.</param>
        /// <returns>An instance of <see cref="TDestination"/></returns>
        TDestination Transform(TSource source);

        /// <summary>
        /// Transforms data from <see cref="TSource"/> to <see cref="TDestination"/>.
        /// </summary>
        /// <param name="source">An instance of <see cref="TSource"/> to transform data from.</param>
        /// <returns>An asynchronous operation that returns an instance of <see cref="TDestination"/>.</returns>
        Task<TDestination> TransformAsync(TSource source);
    }
}
