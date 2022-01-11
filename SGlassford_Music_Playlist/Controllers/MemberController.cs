using SGlassford_Music_Playlist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using SGlassford_Music_Playlist.Models.Users;
using SGlassford_Music_Playlist.Models.ViewModels;

namespace SGlassford_Music_Playlist.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        //Create instance of the database
        private MusicPlaylistDbContext context = new MusicPlaylistDbContext();

        //Action that displays all Users playlists
        public ActionResult Playlists()
        {
            //Select all playlists
            //Include foreign key User
            var playlists = context.Playlists.Include(p => p.User);

            //Get the id of the logged in user
            var userId = User.Identity.GetUserId();

            //From the list of playlists select only the current users
            playlists = playlists.Where(p => p.User.Id == userId);

            //Send playlists to view
            return View(playlists.ToList());
        }

        [HttpGet]
        //Members can create new playlists
        public ActionResult CreatePlaylist()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreatePlaylist([Bind(Include = "PlaylistName, Description")] Playlist playlist)
        {

            if (ModelState.IsValid)
            {
                //Get logged in Users id
                var id = User.Identity.GetUserId();

                //Find User
                var user = context.Users.Find(id);

                playlist.UserId = id;
                playlist.DateCreated = DateTime.Now;
                playlist.Songs = new HashSet<Song>();


                //Check if user is free or premium and limit song capacity
                if (user.AccountType == AccountType.Free)
                {
                    playlist.Capacity = 100;
                }
                else
                {
                    playlist.Capacity = 900000;
                }

                //Add new playlist to the database and save changes
                context.Playlists.Add(playlist);
                context.SaveChanges();

                return RedirectToAction("Playlists");
            }

            //If something goes wrong return to view
            return View();
        }

        //This action edits playlist name and desctiption
        public ActionResult EditPlaylist(int? id)
        {
            //Check if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find post by id
            Playlist playlist = context.Playlists.Find(id);

            //Check if playlist exists
            if (playlist == null)
            {
                return HttpNotFound();
            }

            //ViewModel that holds Songs and a Playlist
            EditPlaylistViewModel model = new EditPlaylistViewModel
            {
                //Assign playlist attributes to model
                Description = playlist.Description,
                PlaylistName = playlist.PlaylistName,
                PlaylistId = id
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlaylist([Bind(Include = "PlaylistName,Description,PlaylistId")] EditPlaylistViewModel model)
        {
            //Update database
            if (ModelState.IsValid)
            {
                Playlist playlist = context.Playlists.Find(model.PlaylistId);

                playlist.PlaylistName = model.PlaylistName;
                playlist.Description = model.Description;

                context.Entry(playlist).State = EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Playlists");
            }
            return View();
        }

        //Action that plays songs in selected playlist
        public ActionResult PlayPlaylist(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find playlist by id
            Playlist playlist = context.Playlists.Find(id);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            //Get the id of the logged in user
            var userId = User.Identity.GetUserId();

            //From the list of playlists select only the current users
            List<Playlist>playlists = context.Playlists.Where(p => p.User.Id == userId).ToList();

            playlists.Remove(playlist);

            //Add all playlists to ViewBag
            ViewBag.data = playlists;

            return View(playlist);
        }

        //Action that takes user to AddSong view
        [HttpGet]
        public ActionResult AddSong(int? id)
        {
            SongPlaylistViewModel model = new SongPlaylistViewModel
            {
                Playlist = context.Playlists.Find(id),
                Songs = context.Songs.ToList()
            };

            ViewBag.Message = Convert.ToString(TempData["Message"]);
            return View(model);
        }


        
        //Action that adds song to playlist
        [ActionName("AddSongPost")]
        [HttpGet]
        public ActionResult AddSong(int? id,int? secondId)
        {
            string message;

            //Check if song or playlist id are null
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (secondId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find song using id
            Song song = context.Songs.Find(id);

            //Find playlist using id
            Playlist playlist = context.Playlists.Find(secondId);

            //Check song and playlist exists in database
            if (song == null)
            {
                return HttpNotFound();
            }
            if (playlist == null)
            {
                return HttpNotFound();
            }
            if (playlist.Capacity < playlist.Songs.Count)
            {
                //TempData to send error message
                message = "Playlist is full";
                TempData["Message"] = message;

                return RedirectToAction("AddSong", new { id = secondId });
            }
            if(!playlist.Songs.Add(song))
            {
                //TempData to send error message
                message = "This song is already in playlist";
                TempData["Message"] = message;

                return RedirectToAction("AddSong", new { id = secondId });
            }

            //Update playlist in database
            context.Entry(playlist).State = EntityState.Modified;
            //Save changes
            context.SaveChanges();

            //TempData to send success message
            message = "Song successfully added to playlist";
            TempData["Message"] = message;

            return RedirectToAction("AddSong",new { id = secondId});
        }

        //Action that deletes playlist from database
        public ActionResult DeletePlaylist(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Playlist playlist = context.Playlists.Find(id);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            context.Playlists.Remove(playlist);
            context.SaveChanges();

            return RedirectToAction("Playlists");
        }

        //Action that compares features of the current playlist
        public ActionResult ComparePlaylists(int? id, int? secondId)
        {
            //Check if id's are null
            if (id == null)
            {
                return HttpNotFound();
            }
            if (secondId == null)
            {
                return HttpNotFound();
            }

            ComparePlaylistViewModel model = new ComparePlaylistViewModel();

            string subsetMessage;

            //Find both playlists in database
            Playlist selectedPlaylist = context.Playlists.Find(id);
            Playlist comparePlaylist = context.Playlists.Find(secondId);

            //Add both playlists to model
            model.SelectedPlaylist =selectedPlaylist;
            model.SecondPlaylist = comparePlaylist;

            //Store unique songs in model
            model.SelectedPlaylistUniqueSongs = new HashSet<Song>(selectedPlaylist.Songs.Except(comparePlaylist.Songs));
            model.SecondPlaylistUniqueSongs = new HashSet<Song>(comparePlaylist.Songs.Except(selectedPlaylist.Songs));

            //Store shared songs in model
            model.SharedSongs = new HashSet<Song>(selectedPlaylist.Songs.Intersect(comparePlaylist.Songs));

            //Check if playlist is a subset and send message to view through ViewBag
            if (selectedPlaylist.Songs.IsSubsetOf(comparePlaylist.Songs))
            {
                subsetMessage = selectedPlaylist.PlaylistName + " is a subset of " + comparePlaylist.PlaylistName;
                ViewBag.Message = subsetMessage;
            }
            else
            {
                subsetMessage = selectedPlaylist.PlaylistName + " is not a subset of " + comparePlaylist.PlaylistName;
                ViewBag.Message = subsetMessage;
            }

            //Send playlists to the View
            return View(model);
        }

        public ActionResult MergePlaylist(int? id, int? secondId)
        {
            //Check if id's are null
            if (id == null)
            {
                return HttpNotFound();
            }
            if (secondId == null)
            {
                return HttpNotFound();
            }

            //Find playlist using id
            Playlist playlist = context.Playlists.Find(id);
            Playlist playlist2 = context.Playlists.Find(secondId);

            //Check if playlist is null
            if (playlist == null)
            {
                return HttpNotFound();
            }
            if (playlist2 == null)
            {
                return HttpNotFound();
            }

            //Merge HashSet of songs
            playlist.Songs.UnionWith(playlist2.Songs);

            //Update playlist in database
            context.Entry(playlist).State = EntityState.Modified;
            //Delete other playlist
            context.Playlists.Remove(playlist2);
            //Save changes
            context.SaveChanges();

            return RedirectToAction("Playlists","Member");
        }
    }

}