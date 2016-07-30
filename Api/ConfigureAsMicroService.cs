using Core;
using Core.Interfaces;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    [Chained]
    public class ConfigureAsMicroService : IMicroConfiguration
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IList<string> LogoLines => new List<string>() {
            @"                               ____                ",
            @"          /\       \    /     |    |      /|       ",
            @"         /  \       \  /      |    |     / |       ",
            @"        /    \       \/       |____|      _|_      ",
            @"                                                   ",
        };

        public ConfigureAsMicroService()
        {

        }

        public void Configure(IAppBuilder app)
        {
            // add static server middleware
            var webRoot = ConfigurationManager.AppSettings.Get("web_root");
            if (string.IsNullOrEmpty(webRoot)) return;

            Log.InfoFormat("Adding static file server at: {0}", webRoot);

            var authenticationMode = ConfigurationManager.AppSettings.Get("authentication_mode");
            if (!string.IsNullOrEmpty(authenticationMode) && authenticationMode == "windows")
            {
                HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];
                listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
                Log.InfoFormat("Integrated windows authentication enabled");
            }


            app.UseStaticFiles(webRoot);
        }
    }
}
