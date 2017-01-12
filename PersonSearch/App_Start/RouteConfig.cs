using System.Web.Mvc;
using System.Web.Routing;

namespace PersonSearch
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "search/{action}/{id}",
                defaults: new { controller = "Search", action = "GetUsers", id = UrlParameter.Optional }
            );
        }
    }
}
