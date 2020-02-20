using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Event;
using RepositorioIntegracaoPCSist.Mapeamento;
using Environment = NHibernate.Cfg.Environment;

namespace RepositorioIntegracaoPCSist.Base
{
    /// <summary>
    /// Uses Xml-configuration for setup-config and for mappings is Hbm-files used.
    /// </summary>
    public class FluentNHibContext : NHibContext
    {
        public FluentNHibContext()
        {

            if (SessionFactory == null)
                SessionFactory = CreateSessionFactory();
        }

        /// <summary>
        /// Creates and returns a session factory.
        /// </summary>
        /// <returns></returns>
        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                OracleClientConfiguration.Oracle10.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringOracle"))
                //MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringMySQL"))
                //MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringSQL"))
                //MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringSQL"))
                )
                //.Cache(c => c.UseQueryCache().ProviderClass(typeof(NHibernate.Caches.SysCache2.SysCacheProvider).AssemblyQualifiedName))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ProdutoMap>()//x => x.GetProperty("Codigo") != null
                .Conventions.Setup(GetConventions()))
                .ExposeConfiguration(cfg => cfg.SetProperty(Environment.CurrentSessionContextClass, "web"))
                .ExposeConfiguration(cfg => cfg.SetListener(ListenerType.PostUpdate, new AuditEventListener()))
                .CurrentSessionContext("web")
                .BuildSessionFactory();
        }

        private static Action<IConventionFinder> GetConventions()
        {
            return c =>
                       {
                           c.Add<CascadeConvention>();
                           c.Add<EnumConvention>();
                       };
        }
    }

    //public static class ExtensionNHibernate
    //{
    //    public static FluentMappingsContainer AddFromAssemblyOf<T>(this FluentMappingsContainer mappings, Predicate<Type> where)
    //    {
    //        //var fluentMappingsContainer = new FluentMappingsContainer();

    //        //var lista = RegistrarMaps();
    //        //for (int i = 0; i < lista.Count; i++)
    //        //{
    //        //    fluentMappingsContainer.Add(lista[i]);
    //        //}

    //        if (where == null)
    //            return mappings.AddFromAssemblyOf<T>();

    //        //var mappingClasses = typeof(T).Assembly.GetExportedTypes()
    //        //    .Where(x => typeof(IMappingProvider).IsAssignableFrom(x)
    //        //        && where(x));

    //        foreach (var type in RegistrarMaps())
    //        {
    //            mappings.Add(type);
    //        }

    //        return mappings;
    //    }

    //    private static IList<Type> RegistrarMaps()
    //    {
    //        var repositoryAssembly = typeof(EmpresaMap).Assembly;

    //        var registrations =
    //            from type in repositoryAssembly.GetExportedTypes()
    //            where type.Namespace == "SuperEscola.Repositorio.Mapeamento"
    //            where type.Name.Contains("Map")
    //            where type.IsClass
    //            //where !type.Name.Equals("AlunoMap")
    //            //where !type.Name.Equals("FucnionarioMap")
    //            //where !type.Name.Contains("ResponsavelAlunoMap")
    //            //where !type.Name.Equals("PessoaMap")
    //            //where !type.Name.Contains("CobrancaEtapaMap")
    //            //where !type.Name.Contains("CobrancaServicoMap")
    //            //where !type.Name.Contains("CobrancaMap")
    //            //where !type.Name.Contains("EncargosJurosMap")
    //            //where !type.Name.Contains("EncargosMultaMap")
    //            //where !type.Name.Contains("EncargosCobrancaMap")
    //            //where !type.Name.Contains("LancamentosCobrancaMap")
    //            //where !type.Name.Contains("RecebimentoMap")
    //            select new
    //            {
    //                //Service = type.GetInterfaces().First(x => x.Name.Equals(type.Name)),
    //                Implementation = type
    //            };

    //        IList<Type> retorno = new List<Type>();
    //        foreach (var reg in registrations)
    //        {
    //            retorno.Add(reg.Implementation);
    //        }

    //        return retorno;
    //    }
    //}

    public class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        public void Apply(IOneToOneInstance instance)
        {

        }

        public void Apply(IManyToOneInstance instance)
        {

        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.All();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {

        }
    }

    public class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum);
        }

        public void Apply(IPropertyInstance target)
        {
            target.CustomType(target.Property.PropertyType);
        }
    }
}