using System.Web.Http;
using System.Web.Http.Cors;
using ApiInfox.Filters;
using Core.Resolvers;
using Newtonsoft.Json;

namespace ApiInfox
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable cors
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new NHibernateContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.Culture = System.Globalization.CultureInfo.CurrentCulture;

            GlobalConfiguration.Configuration.Filters.Add(new GenericErrorFilterAttribute());

            // Start dependency injection
            SimpleInjectorInitializer.Initialize();
        }
    }
}