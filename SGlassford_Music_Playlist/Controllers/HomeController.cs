using Microsoft.AspNet.Identity;
using SGlassford_Music_Playlist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGlassford_Music_Playlist.Controllers
{
    public class HomeController : Controller
    {
        //DbContext
        private MusicPlaylistDbContext context = new MusicPlaylistDbContext();


        //Homepage
        public ActionResult Index()
        {
            return View();
        }

        //List all songs
        public ActionResult Songs()
        {
            var songs = context.Songs.ToList();

            return View(songs);
        }


        //Play selected song
        public ActionResult SongDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Song song = context.Songs.Find(id);

            if (song == null)
            {
                return HttpNotFound();
            }

            return View(song);
        }


    }
}