using System;
using System.Collections.Generic;
using CsvToEntityDemo.Exceptions;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.Models;
using NUnit.Framework;

namespace CsvToEntityDemo.Tests
{
    [TestFixture]
    public class CsvLoaderTests
    {
        [Test]
        public void GetColumnNamesFromHeaderOk()
        {
            string header = "ArtistId,SongName, Remix,ReleaseDate ,Rating";

            var expectedColumnNames = new List<string>
            {
                "ArtistId", "SongName", "Remix", "ReleaseDate", "Rating"
            };

            ICsvLoader csvLoader = new CsvLoader();

            IList<string> actualColumnNames = csvLoader.GetColumnNamesFromHeader(header);

            for (int i = 0; i < expectedColumnNames.Count; i++)
            {
                string expectedColumnName = expectedColumnNames[i];
                string actualColumnName = actualColumnNames[i];

                Assert.That(expectedColumnName == actualColumnName);
            }
        }

        [Test]
        public void CanGetEntityFromRowOk()
        {
            string rowLine = "4,If You Should Go,John O'Callaghan Remix,2007-07-14,5.0";

            var fieldNames = new List<string>
            {
                "ArtistId", "SongName", "Remix", "ReleaseDate", "Rating"
            };

            var expectedEntity = new MusicTrack
            {
                ArtistId = 4,
                SongName = "If You Should Go",
                Remix = "John O'Callaghan Remix",
                ReleaseDate = new DateTime(2007, 7, 14),
                Rating = 5.0M
            };

            ICsvLoader csvLoader = new CsvLoader();

            MusicTrack actualEntity = csvLoader.GetEntityFromRow<MusicTrack>(rowLine, fieldNames);

            AssertMusicTrack(expectedEntity, actualEntity);
        }

        [Test]
        public void CanGetEntityTypeFromFilename()
        {
            ICsvLoader csvLoader = new CsvLoader();

            Type entityType = csvLoader.GetEntityTypeFromFilename("MusicTrack.csv");

            Assert.That(entityType.Name == "MusicTrack");
        }

        [Test]
        public void ThrowsIfFieldsDoNotMatchEntity()
        {
            string rowLine = "4,If You Should Go,John O'Callaghan Remix,2007-07-14,Armada";

            var fieldNames = new List<string>
            {
                "ArtistId", "SongName", "Remix", "ReleaseDate", "Label"
            };

            ICsvLoader csvLoader = new CsvLoader();

            FieldException ex = Assert.Throws<FieldException>(() =>
            {
                MusicTrack actualEntity = csvLoader.GetEntityFromRow<MusicTrack>(rowLine, fieldNames);
            });

            Assert.That(ex.FieldName == "Label");
            Assert.That(ex.Message.Contains("Unknown field"));
        }

        [Test]
        public void ThrowsIfDataDoesNotMatchFieldsType()
        {
            // The last 2 fields are in the wrong order.
            string rowLine = "4,If You Should Go,John O'Callaghan Remix,5.0, 2007-07-14";

            var fieldNames = new List<string>
            {
                "ArtistId", "SongName", "Remix", "ReleaseDate", "Rating"
            };

            ICsvLoader csvLoader = new CsvLoader();

            FieldException ex = Assert.Throws<FieldException>(() =>
            {
                MusicTrack actualEntity = csvLoader.GetEntityFromRow<MusicTrack>(rowLine, fieldNames);
            });

            Assert.That(ex.FieldName == "ReleaseDate");
            Assert.That(ex.Message.Contains("Incompatible value"));
        }

        private class TestClassWithReadOnlyField
        {
            public int Id { get; set; }

            public string ReadOnlyField { get; }
        }

        [Test]
        public void ThrowsIfFieldIsReadOnly()
        {
            string rowLine = "4,Test";

            var fieldNames = new List<string>
            {
                "Id", "ReadOnlyField"
            };

            ICsvLoader csvLoader = new CsvLoader();

            FieldException ex = Assert.Throws<FieldException>(() =>
            {
                TestClassWithReadOnlyField actualEntity = csvLoader
                    .GetEntityFromRow<TestClassWithReadOnlyField>(rowLine, fieldNames);
            });

            Assert.That(ex.FieldName == "ReadOnlyField");
            Assert.That(ex.Message.Contains("Field is readonly"));
        }

        [Test]
        public void ThrowsIfUnknownEntityType()
        {
            ICsvLoader csvLoader = new CsvLoader();

            TypeException ex = Assert.Throws<TypeException>(() =>
            {
                Type entityType = csvLoader.GetEntityTypeFromFilename("Label.csv");
            });

            Assert.That(ex.TypeName == "CsvToEntityDemo.Models.Label");
            Assert.That(ex.Message.Contains("Unknown type"));
        }

        private void AssertMusicTrack(MusicTrack expected, MusicTrack actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }

            Assert.That(expected.ArtistId == actual.ArtistId);
            Assert.That(expected.SongName == actual.SongName);
            Assert.That(expected.Remix == actual.Remix);
            Assert.That(expected.ReleaseDate == actual.ReleaseDate);
            Assert.That(expected.Rating == actual.Rating);
        }
    }
}
