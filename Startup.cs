using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MWR.Startup))]
namespace MWR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
