using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SliceAndDiceWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Download",
                url: "{key}/{filename}",
                defaults: new { controller = "Main", action = "Download" }
            );


            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
