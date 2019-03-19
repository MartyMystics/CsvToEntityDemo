using Autofac;
using CsvToEntityDemo.Database;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.Models;

namespace CsvToEntityDemo.IoC
{
    public abstract class ApplicationIoC
    {
        /// <summary>
        /// Register all the required types for the application's dependency injection.
        /// </summary>
        /// <returns>The ApplicationIoC container.</returns>
        public static IContainer RegisterDependencies(MusicContext musicContext)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CsvLoader>().As<ICsvLoader>();
            //builder.Register(f => new RepositoryFactory(musicContext)).As<IRepositoryFactory>();
            builder.Register(r => new MusicTrackRepository(musicContext)).As<IRepository<MusicTrack>>();
            builder.Register(r => new ArtistRepository(musicContext)).As<IRepository<Artist>>();

            return builder.Build();
        }
    }
}
