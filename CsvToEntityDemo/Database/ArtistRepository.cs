using System.Linq;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.Models;

namespace CsvToEntityDemo.Database
{
    public class ArtistRepository : IRepository<Artist>
    {
        private readonly MusicContext context;

        public ArtistRepository(MusicContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds an entity to the repository and returns this ID of the entity.
        /// </summary>
        /// <param name="artist">The entity to add.</param>
        /// <returns>This ID of the entity</returns>
        public int Add(Artist artist)
        {
            context.Artists.Add(artist);
            return context.SaveChanges();
        }

        /// <summary>
        /// Gets an entity from the repository by ID. 
        /// </summary>
        /// <param name="id">The ID of the entity to get.</param>
        /// <returns>The matching entity or null.</returns>
        public Artist Get(int id)
        {
            Artist artist = context.Artists.SingleOrDefault(a => a.Id == id);
            return artist;
        }
    }
}
