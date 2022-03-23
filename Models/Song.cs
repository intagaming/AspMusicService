
using System.Collections.Generic;

namespace MusicService.Models
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public ICollection<Artist> Artists { get; set; }
        public Album Album { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<User> LikedUsers { get; set; }
    }
}
