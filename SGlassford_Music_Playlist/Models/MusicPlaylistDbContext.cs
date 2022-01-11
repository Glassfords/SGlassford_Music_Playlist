using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SGlassford_Music_Playlist.Models
{
    public class MusicPlaylistDbContext : IdentityDbContext<User>
    {

        //DbSets
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }

        //Default constructor
        public MusicPlaylistDbContext()
            : base("MusicPlaylistConnectionV2", throwIfV1Schema: false)
        {
            //Set the database initiliser
            Database.SetInitializer(new DatabaseInitialiser());
        }

        public static MusicPlaylistDbContext Create()
        {
            return new MusicPlaylistDbContext();
        }
    }
}