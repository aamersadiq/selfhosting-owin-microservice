using Core.Helpers;
using Microsoft.Owin.Hosting;
using System;
using System.Reactive.Disposables;
using System.Reflection;

namespace Core
{
    public sealed class ServiceMain
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable(1);
        private readonly string _name;
        private readonly int _port;
        private readonly AssemblyInfo _ai;
        private readonly string _scheme;

        public ServiceMain(AssemblyInfo ai, int port, string scheme)
        {
            _name = GetType().Assembly.GetName().Name;
            _port = port;
            _scheme = scheme;
            _ai = ai;

        }

        public bool Start()
        {
            var url = $"{_scheme}://+:{_port}";
            var urlLocalhost = $"{_scheme}://{DnsHelper.GetLocalhostFqdn()}:{_port}";

            Log.InfoFormat("{0} is starting ... ", _ai.Product);
            Log.InfoFormat("Listening on {0}", url);
            Log.InfoFormat("Swagger UI on {0}/swagger/ui/index", url);
            Log.InfoFormat("Host url is {0}", urlLocalhost);

            try
            {
                _compositeDisposable.Add(WebApp.Start<OwinStartup>(url));
            }
            catch (Exception exc)
            {
                while (exc != null)
                {
                    Log.Error(exc);
                    exc = exc.InnerException;
                }
            }

            return true;
        }

        public bool Stop()
        {
            _compositeDisposable.Dispose();

            Log.InfoFormat("{0} is stopping ... ", _name);
            return true;
        }
    }
}
