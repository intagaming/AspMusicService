using System.Collections.Generic;
namespace MusicService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<Song> LikedSong { get; set; }

    }
}
