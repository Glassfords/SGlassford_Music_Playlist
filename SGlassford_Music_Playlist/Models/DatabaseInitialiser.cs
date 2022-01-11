using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SGlassford_Music_Playlist.Models.Users;

namespace SGlassford_Music_Playlist.Models
{
    //Database is set to drop create always during develoment
    public class DatabaseInitialiser:DropCreateDatabaseAlways<MusicPlaylistDbContext>
    {
        protected override void Seed(MusicPlaylistDbContext context)
        {
            base.Seed(context);

            //Check if any records are stored in User table
            if (!context.Users.Any())
            {
                //--------------------------Seed Roles--------------------------------------

                //Create role manager
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Member"))
                {
                    //Create Member role
                    roleManager.Create(new IdentityRole("Member"));
                }
                if (!roleManager.RoleExists("Admin"))
                {
                    //Create PremiumMember role
                    roleManager.Create(new IdentityRole("Admin"));
                }
                context.SaveChanges();

                //--------------------------Seed Users--------------------------------------

                //Create user manager
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                userManager.PasswordValidator = new PasswordValidator
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = false,
                };

                //Create an Admin
                if (userManager.FindByName("admin@admin.com") == null)
                {
                    var admin = new Member
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        FirstName = "Ad",
                        LastName = "Min",
                        RegisteredAt = DateTime.Now,
                        EmailConfirmed = true,
                        AccountType = AccountType.Premium
                    };

                    //Add to Users table
                    userManager.Create(admin, "admin123");
                    //Assign to admin role
                    userManager.AddToRole(admin.Id, "Admin");
                }

                //Create premium user
                if (userManager.FindByName("member1@member.com") == null)
                {
                    var premUser = new Member
                    { 
                        UserName = "member1@member.com",
                        Email = "member1@member.com",
                        FirstName = "Mike",
                        LastName = "Mires",
                        RegisteredAt = DateTime.Now,
                        EmailConfirmed = true,
                        AccountType = AccountType.Premium
                    };

                    //Add to Users table
                    userManager.Create(premUser, "mike123");
                    //Assign to admin role
                    userManager.AddToRole(premUser.Id, "Member");
                }

                //Create free user
                if (userManager.FindByName("member2@member.com") == null)
                {
                    var freeUser = new Member
                    {
                        UserName = "member2@member.com",
                        Email = "member2@member.com",
                        FirstName = "Lisa",
                        LastName = "Simpson",
                        RegisteredAt = DateTime.Now,
                        EmailConfirmed = true,
                        AccountType = AccountType.Free
                    };
                    //Add to Users table
                    userManager.Create(freeUser, "lisa123");
                    //Assign to admin role
                    userManager.AddToRole(freeUser.Id, "Member");
                }

                

                context.SaveChanges();

                //--------------------------Seed Songs--------------------------------------

                //Azure Media Service to store and stream audio files

                //Start of link for Microsoft Azure Media (first half)
                string azureURL = "https://playlist-ukso1.streaming.media.azure.net/";

                //Add songs manually
                context.Songs.Add(new Song { Name = "At The Restaurant", Artist = "Monolog Rockstars", FilePath = azureURL + "366c63f4-0993-4558-a6a2-ee8056da8219/Monolog Rockstars - At The Resta.ism/manifest(format=m3u8-aapl)"});
                context.Songs.Add(new Song { Name = "Peril", Artist = "Xylo-Ziko", FilePath = azureURL + "857eebea-938a-4944-a986-dc933ced8c60/Xylo Ziko - peril.ism/manifest(format=m3u8-aapl)"}); 
                context.Songs.Add(new Song { Name = "Buckbreak", Artist = "Ken Hamm", FilePath = azureURL + "9407f320-9795-4583-b3dc-67e49ae1e65d/Ken Hamm - Buckbreak.ism/manifest(format=m3u8-aapl)"});
                context.Songs.Add(new Song { Name = "Hambug", Artist = "Crowander", FilePath = azureURL + "10a088fc-50be-48d6-b63a-156080ca98fb/Crowander - Humbug.ism/manifest(format=m3u8-aapl)"});
                context.Songs.Add(new Song { Name = "Algorithms", Artist = "Chad Crunch", FilePath = azureURL + "7bc720cf-c34f-4b20-98a0-240804128e8d/Chad Crouch - Algorithms.ism/manifest(format=m3u8-aapl)"});
                context.Songs.Add(new Song { Name = "Night Owl", Artist = "Broke For Free", FilePath = azureURL + "2b1ced1b-b9f6-4273-af9e-c43f793bee9e/Broke For Free - Night Owl.ism/manifest(format=m3u8-aapl)" });
                context.Songs.Add(new Song { Name = "Running Waters", Artist = "Jason Shaw", FilePath = azureURL + "31434d99-6da4-49a2-8702-0b8b453b4762/Jason Shaw - RUNNING WATERS.ism/manifest(format=m3u8-aapl)" });
                context.Songs.Add(new Song { Name = "The Last Ones", Artist = "Jahzzar", FilePath = azureURL + "59b8246f-3b06-40f6-a43b-c7a0c772c46f/Jahzzar - The last ones.ism/manifest(format=m3u8-aapl)" });
                context.Songs.Add(new Song { Name = "Moonlight Reprise", Artist = "Kai Engel", FilePath = azureURL + "1979f225-0a31-4aef-9d9e-1a0322557511/Kai Engel - Moonlight Reprise.ism/manifest(format=m3u8-aapl)" });
                context.Songs.Add(new Song { Name = "Colourful As Ever", Artist = "Broke For Free", FilePath = azureURL + "5a0fa18f-a3e5-4ec4-8fc5-6ba47d38578f/Broke For Free - As Colorful As .ism/manifest(format=m3u8-aapl)" });
                context.Songs.Add(new Song { Name = "Stance Gives You Balance", Artist = "Hogan Grip", FilePath = azureURL + "0ac0d647-68bf-4560-9b4d-a1814188b389/Hogan Grip - Stance Gives You Ba.ism/manifest(format=m3u8-aapl)" });
                context.SaveChanges();

                
                


            }
        }
    }
}