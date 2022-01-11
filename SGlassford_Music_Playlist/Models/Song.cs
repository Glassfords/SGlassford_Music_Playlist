using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGlassford_Music_Playlist.Models
{
    public class Song
    {
        //Properties
        [Key]
        public int SongId { get; set; }
        [Display(Name="Song Name")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Artist Name")]
        [Required]
        public string Artist { get; set; }
        [DataType(DataType.Url)]
        [Required]
        public string FilePath { get; set; }

        //Navigational Properties
        public List<Playlist> Playlists { get; set; }

    }
}