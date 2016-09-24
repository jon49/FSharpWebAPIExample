using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.FSharp;

namespace FSharpWebAPIExample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.Converters.Add(new TupleArrayConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new UnionEnumConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new ListConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new SingleCaseUnionConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new OptionConverter());
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver;
            //    new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

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
