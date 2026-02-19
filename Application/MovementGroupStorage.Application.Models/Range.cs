namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents a possible capacity range.
    /// </summary>
    public class Range<T> where T : IComparable<T>
    {
        /// <summary>
        /// Minimum value.
        /// </summary>
        public T Min { get; set; }

        /// <summary>
        /// Maximum value.
        /// </summary>
        public T Max { get; set; }
    }
}
