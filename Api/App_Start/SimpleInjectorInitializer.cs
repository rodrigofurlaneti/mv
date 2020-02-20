using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using Clube.Premiar;
using Clube.Premiar.Adapter;
using InitializerHelper.Ioc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.WebApi;

namespace Api
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            InitializeContainer(container);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            container.Register(() =>
            {
                return WebConfigurationManager.OpenWebConfiguration("~").GetSectionGroup("premiarClubeSettings") as PremiarClubeSettings;
            });
            container.Register<Dominio.Providers.IClubeProvider, PremiarClubeProviderAdapter>();

            FabricaSimpleInjector.Registrar(container);
        }
    }
}