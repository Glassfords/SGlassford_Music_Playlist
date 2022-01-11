using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGlassford_Music_Playlist.Models.ViewModels
{
    public class EditPlaylistViewModel
    {
        public int? PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public string Description { get; set; }
    }
}