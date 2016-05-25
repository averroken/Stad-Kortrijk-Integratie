using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASP_WEB.Startup))]
namespace ASP_WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
