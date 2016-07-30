using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Swashbuckle.Application;
using Autofac.Configuration;
using Core.Helpers;
using Newtonsoft.Json.Serialization;

namespace Core
{
    public class Chained : Attribute { }
    public class Singleton : Attribute { }

    public sealed class Configure
    {

        private readonly AssemblyInfo _assemblyInfo;
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Configure(AssemblyInfo ai)
        {
            _assemblyInfo = ai;
        }

        public HttpConfiguration Config { get; private set; }

        public void ApplyConfiguration(HttpConfiguration config)
        {
            try
            {
                //  Enable attribute based routing
                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute(
                                name: "DefaultApi",
                                routeTemplate: "api/v1/{controller}/{id}",
                                defaults: new { id = RouteParameter.Optional }
                            );

                config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

                config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

                var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
                config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
                

                config
                .EnableSwagger(c => c.SingleApiVersion("v1", _assemblyInfo.Description))
                .EnableSwaggerUi();

                Config = config;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error configuring service {0}", ex);
                throw;
            }
        }

        public IContainer ResolveDependencies()
        {
            // bootstrapper
            var builder = new ContainerBuilder();

            // read from config file
            builder.RegisterModule(new ConfigurationSettingsReader("dependencySettings"));

            // pull assemblies
            var assemblies = _assemblyInfo.Assembly.GetReferencedAssemblies()
                             .Where(a => a.Name == "Api" || a.Name == "Core" || a.Name == "Data")
                             .Select(Assembly.Load)
                             .ToList();
            assemblies.Add(_assemblyInfo.Assembly);

            // Default lifetime instances
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                   .Where(t => t.GetCustomAttributes().FirstOrDefault(a => a is Chained) != null)
                   .AsImplementedInterfaces();

            // Singleton instances 
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                   .Where(t => t.GetCustomAttributes().FirstOrDefault(a => a is Singleton) != null)
                   .AsImplementedInterfaces().SingleInstance();

            builder.RegisterApiControllers(assemblies.ToArray());
            builder.RegisterWebApiFilterProvider(Config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            Config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}
