using SGlassford_Music_Playlist.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGlassford_Music_Playlist.Models
{
    public class Playlist
    {
        //Properties
        [Key]
        public int PlaylistId { get; set; }
        [Display(Name ="Playlist Name")]
        [Required]
        public string PlaylistName { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] // formats as short data time
        public DateTime DateCreated { get; set; }
        [Display(Name="Playlist Description")]
        public string Description { get; set; }

        //Constructor
        public Playlist()
        {
            Songs = new HashSet<Song>();
        }
            
        //Navigational properties
        public virtual HashSet<Song> Songs { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}