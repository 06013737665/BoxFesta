using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Box.Festa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Usuario",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Usuario", action = "Usuario", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Endereco",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Endereco", action = "ListarEndereco", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "FormaPagto",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FormaPagto", action = "ListarFormaPagamento", id = UrlParameter.Optional }
            );
        }
    }
}
