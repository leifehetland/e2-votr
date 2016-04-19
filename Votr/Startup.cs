using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Votr.Startup))]
namespace Votr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
