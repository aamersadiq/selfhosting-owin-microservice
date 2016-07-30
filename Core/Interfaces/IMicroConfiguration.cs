using Owin;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IMicroConfiguration
    {
        IList<string> LogoLines { get; }
        void Configure(IAppBuilder app);
    }
}
