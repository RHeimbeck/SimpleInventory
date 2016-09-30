using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using InventoryDemo1.Models;

namespace InventoryDemo1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{label}",
                defaults: new { label = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "AddInventoryApi",
                routeTemplate: "api/inventory/{InventoryItem}",
                defaults: new { InventoryItem = RouteParameter.Optional }
            );
        }
    }
}
