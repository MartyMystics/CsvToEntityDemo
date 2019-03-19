using System.Data.Entity;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.Models;

namespace CsvToEntityDemo.Database
{
    public class MusicContext: DbContext
    {
        public MusicContext() : base()
        {
        }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<MusicTrack> MusicTracks { get; set; }
    }
}
