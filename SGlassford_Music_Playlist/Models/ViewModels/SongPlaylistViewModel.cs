using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGlassford_Music_Playlist.Models.ViewModels
{
    public class SongPlaylistViewModel
    {
        public List<Song> Songs { get; set; }
        public Playlist Playlist { get; set; }
    }
}