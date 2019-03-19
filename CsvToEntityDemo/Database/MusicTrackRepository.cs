using System.Linq;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.Models;

namespace CsvToEntityDemo.Database
{
    public class MusicTrackRepository : IRepository<MusicTrack>
    {
        private readonly MusicContext context;

        public MusicTrackRepository(MusicContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds an entity to the repository and returns this ID of the entity.
        /// </summary>
        /// <param name="musicTrack">The entity to add.</param>
        /// <returns>This ID of the entity</returns>
        public int Add(MusicTrack musicTrack)
        {
            context.MusicTracks.Add(musicTrack);
            context.SaveChanges();
            return musicTrack.Id;
        }

        /// <summary>
        /// Gets an entity from the repository by ID. 
        /// </summary>
        /// <param name="id">The ID of the entity to get.</param>
        /// <returns>The matching entity or null.</returns>
        public MusicTrack Get(int id)
        {
            MusicTrack musicTrack = context.MusicTracks.SingleOrDefault(a => a.Id == id);
            return musicTrack;
        }
    }
}
