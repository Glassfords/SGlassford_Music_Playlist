using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGlassford_Music_Playlist.Models.ViewModels
{
    public class ComparePlaylistViewModel
    {
        public Playlist SelectedPlaylist { get; set; }
        public Playlist SecondPlaylist { get; set; }
        public IEnumerable<Song> SelectedPlaylistUniqueSongs { get; set; }
        public IEnumerable<Song> SecondPlaylistUniqueSongs { get; set; }
        public IEnumerable<Song> SharedSongs { get; set; }

    }
}