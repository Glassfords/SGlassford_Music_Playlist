using System.Web;
using System.Web.Mvc;

namespace SGlassford_Music_Playlist
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
