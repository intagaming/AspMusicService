using System.Collections.Generic;

namespace MusicService.Models
{
    public class Album
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
