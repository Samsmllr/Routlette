using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RouteScheduler.Startup))]
namespace RouteScheduler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
