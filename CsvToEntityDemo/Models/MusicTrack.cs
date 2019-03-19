using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsvToEntityDemo.Models
{
    public class MusicTrack
    {
        [Key]
        public int Id { get; set; }

        public int ArtistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SongName { get; set; }

        [MaxLength(100)]
        public string Remix { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public decimal Rating { get; set; }
    }
}
