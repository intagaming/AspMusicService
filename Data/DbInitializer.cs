using MusicService.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MusicContext context)
        {
            context.Database.EnsureCreated();

            if (context.Songs.Any())
            {
                return;
            }


            var artists = new Artist[]
            {
                new Artist{Name="Lil Wuyn"},
                new Artist{Name="Twenty One Pilots"}
            };
            foreach (Artist artist in artists)
            {
                context.Artists.Add(artist);
            }
            context.SaveChanges();

            var albums = new Album[]
            {
                new Album{Name="Scaled And Icy"}
            };
            foreach (Album album in albums)
            {
                context.Albums.Add(album);
            }
            context.SaveChanges();

            var songs = new Song[]
            {
                new Song{Name="Diamond", Artists=new HashSet<Artist>{ artists[0] }},
                new Song{Name="Choker", Artists=new HashSet<Artist>{ artists[1] }, Album=albums[0]},
                new Song{Name="The Outside", Artists=new HashSet<Artist>{ artists[1] }, Album=albums[0]},
            };
            foreach (Song song in songs)
            {
                context.Songs.Add(song);
            }
            context.SaveChanges();

            var playlists = new Playlist[]
            {
                new Playlist{Name="My Playlist", Songs=new HashSet<Song> { songs[0], songs[2] } }
            };
            foreach (Playlist playlist in playlists)
            {
                context.Playlists.Add(playlist);
            }
            context.SaveChanges();
        }
    }
}
