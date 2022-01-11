using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SGlassford_Music_Playlist.Startup))]
namespace SGlassford_Music_Playlist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
