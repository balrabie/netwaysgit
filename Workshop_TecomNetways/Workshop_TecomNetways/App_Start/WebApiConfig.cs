using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Workshop_TecomNetways.Context;

namespace Workshop_TecomNetways
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            using (var Context = new BRDContext())
            {
                Context.Database.CreateIfNotExists();
            }
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

  

        }
    }
}
