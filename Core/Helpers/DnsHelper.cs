﻿using System.Net.NetworkInformation;

namespace Core.Helpers
{
    public class DnsHelper
    {
        public static string GetLocalhostFqdn()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            return string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
        }
    }
}
