using Core.Helpers;
using System;
using System.Reflection;
using Topshelf;

namespace Core
{
    public sealed class MicroServiceHost
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly int Port = 9000;
        private static readonly string Scheme =  "http";

        public void Run()
        {
            try
            {
                Log.InfoFormat("Starting on port {0}", Port);
                var entryAssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());

                var code = HostFactory.Run(x =>
                {
                    x.Service<ServiceMain>(s =>
                    {
                        s.ConstructUsing(name => new ServiceMain(entryAssemblyInfo, Port, Scheme));
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    x.StartAutomatically();
                    x.EnableServiceRecovery(c => c.RestartService(1));
                    x.RunAsPrompt();
                    x.SetDescription(entryAssemblyInfo.Description);
                    x.SetDisplayName(entryAssemblyInfo.Product);
                    x.SetServiceName(entryAssemblyInfo.ProductTitle);
                });

                Log.InfoFormat("Topshelf Exit Code: {0}", code);

            }
            catch (Exception ex)
            {
                Log.Error("Error: ", ex);
            }

        }
    }
}
