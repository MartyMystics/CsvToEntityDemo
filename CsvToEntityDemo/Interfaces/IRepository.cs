namespace CsvToEntityDemo.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds an entity to the repository and returns this ID of the entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>This ID of the entity</returns>
        int Add(T entity);

        /// <summary>
        /// Gets an entity from the repository by ID. 
        /// </summary>
        /// <param name="id">The ID of the entity to get.</param>
        /// <returns>The matching entity or null.</returns>
        T Get(int id);
    }
}
