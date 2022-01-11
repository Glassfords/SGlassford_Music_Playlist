using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SGlassford_Music_Playlist.Models
{
    public abstract class User : IdentityUser
    {
        //Extended User properties
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name="Date Registered")]
        public DateTime RegisteredAt { get; set; }
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        public User():base()
        {
            Playlists = new HashSet<Playlist>();
        }

        //Navigational properties
        public virtual HashSet<Playlist> Playlists { get; set; }

        //Users current Role property
        private ApplicationUserManager userManager;
        [NotMapped]
        public string CurrentRole
        {
            get
            {
                if (userManager == null)
                {
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return userManager.GetRoles(Id).Single();
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //User can either be a premium or free account
    public enum AccountType { Premium, Free }
}