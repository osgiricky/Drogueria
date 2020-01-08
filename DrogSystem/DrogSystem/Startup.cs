using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DrogSystem.Startup))]
namespace DrogSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
