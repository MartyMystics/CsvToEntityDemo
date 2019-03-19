using System;
using System.Collections.Generic;
using System.IO;
//using System.Reflection;
using Autofac;
using CsvToEntityDemo.Database;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.IoC;
using CsvToEntityDemo.Models;

namespace CsvToEntityDemo
{
    class Program
    {
        const string UsageMessage = "Usage: CsvToEntityDemo <csv-filename>";

        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            // There are a couple of data files in the TestData folder that I have been 
            // using for testing: Artist.csv and MusicTrack.csv
            // 
            // Try the following to import them:
            //    CsvToEntityDemo TestData/Artist.csv
            //    CsvToEntityDemo TestData/MusicTrack.csv
            //
            // This program assumes that the Table (entity name) is the same as the filename 
            // (minus any path / .csv details). The first row of each CSV file must contain
            // the names of the table columns and these must match the entity field names.
            //
            // The program will then use reflection to create an instance of the entity and 
            // populate that instance with data from the row (line) it has just read from the 
            // CSV data file. It will then write that row to the database using Entity Framework 
            // Code First.
            // 
            // I've added some NUnit unit tests for testing the positive and negative flows for
            // the CsvLoader class, and the program itself should probably have better exception
            // handling, i.e. maybe rollback on error (?), checks for duplicate rows and then 
            // update instead of insert, etc, but these didn't appear to be in the requirements.

            try
            {
                EnsureArgsAreValid(args);

                string csvFilename = args[0];
                if (!File.Exists(csvFilename))
                {
                    Console.WriteLine($"CSV File: '{csvFilename}' could not be found.");
                }

                using (var musicContext = new MusicContext())
                {
                    Container = ApplicationIoC.RegisterDependencies(musicContext);

                    using (var scope = Container.BeginLifetimeScope())
                    {
                        using (var reader = new StreamReader(csvFilename))
                        {
                            var csvLoader = scope.Resolve<ICsvLoader>();

                            Type entityType = csvLoader.GetEntityTypeFromFilename(csvFilename);

                            string header = reader.ReadLine();
                            if (string.IsNullOrEmpty(header))
                            {
                                throw new ApplicationException(
                                    "CsvToEntityDemo Error: The first line of this file should be a row of comma-separated field names.");
                            }

                            List<string> columnNames = csvLoader.GetColumnNamesFromHeader(header);

                            while (!reader.EndOfStream)
                            {
                                string rowLine = reader.ReadLine();

                                if (string.IsNullOrEmpty(rowLine))
                                {
                                    continue;
                                }

                                var entity = typeof(ICsvLoader)
                                    .GetMethod("GetEntityFromRow")
                                    .MakeGenericMethod(entityType)
                                    .Invoke(csvLoader, new object[] { rowLine, columnNames });

                                // TODO: There must be a way better (more generic) way of doing this bit below, 
                                // but I wasn't having much luck at 1am when I was doing this (see commented code 
                                // further below), so I settled for this ... at least it works! :)

                                if (entity is MusicTrack)
                                {
                                    var repository = scope.Resolve<IRepository<MusicTrack>>();
                                    int musicTrackId = repository.Add(entity as MusicTrack);
                                    Console.WriteLine($"Added a new MusicTrack with ID: {musicTrackId}");

                                    // NOTE: Uncomment the below code to verify that the data was written successfully.
                                    //MusicTrack musicTrackCheck = repository.Get(musicTrackId); 
                                    //if (musicTrackCheck == null)
                                    //{
                                    //    throw new ApplicationException($"MusicTrack: ID={musicTrackId} does not exist.");
                                    //}
                                }
                                else if (entity is Artist)
                                {
                                    var repository = scope.Resolve<IRepository<Artist>>();
                                    int artistId = repository.Add(entity as Artist);
                                    Console.WriteLine($"Added a new Artist with ID: {artistId}");

                                    // NOTE: Uncomment the below code to verify that the data was written successfully.
                                    //Artist artistCheck = repository.Get(artistId);
                                    //if (artistCheck == null)
                                    //{
                                    //    throw new ApplicationException($"Artist: ID={artistId} does not exist.");
                                    //}
                                }
                                else
                                {
                                    throw new ApplicationException($"Entity type: {entity.GetType()} is not supported");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private static IRepository<T> GetRepository<T>(ILifetimeScope scope, T entity) where T : class
        //{
        //    Type repoType = typeof(IRepository<T>);
        //
        //    var type = typeof(ResolutionExtensions);
        //    var method = type.GetMethod("Resolve", new[] { typeof(Type) });
        //    var genericMethod = method.MakeGenericMethod(repoType);
        //    var repository = method.Invoke(scope, new object[] { });
        //    return repository;
        //
        //    var repository = scope.Resolve<IRepository<T>>();
        //    return repository;
        //}

        private static void EnsureArgsAreValid(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ApplicationException(UsageMessage);
            }

            string csvFilename = args[0];
            if (!File.Exists(csvFilename))
            {
                Console.WriteLine($"Error: CSV File: '{csvFilename}' could not be found.");
            }
        }
    }
}
