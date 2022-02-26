using System.Collections.Generic;

namespace MusicService.Models
{
    public class Artist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
