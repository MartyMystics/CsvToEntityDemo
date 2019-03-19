using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsvToEntityDemo.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public decimal Rating { get; set; }

        public ICollection<MusicTrack> MusicTracks { get; set; }
    }
}
