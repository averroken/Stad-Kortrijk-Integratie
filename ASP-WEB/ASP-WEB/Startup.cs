using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASP_WEB.Startup))]
namespace ASP_WEB
{
#pragma warning disable 1591
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
