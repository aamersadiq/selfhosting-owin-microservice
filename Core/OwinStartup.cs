using Autofac;
using Core.Helpers;
using Core.Interfaces;
using Owin;
using System.Reflection;
using System.Web.Http;

namespace Core
{
    internal class OwinStartup
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            Log.InfoFormat("Owin server is starting");

            var entryAssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());
            var bootstrapper = new Configure(entryAssemblyInfo);
            bootstrapper.ApplyConfiguration(new HttpConfiguration());
            var container = bootstrapper.ResolveDependencies();

            app.UseWebApi(bootstrapper.Config);

            using (var s = container.BeginLifetimeScope())
            {
                var micro = s.Resolve<IMicroConfiguration>();
                foreach (var line in micro.LogoLines)
                {
                    Log.Info(line);
                }
                micro.Configure(app);
            }
        }
    }
}
