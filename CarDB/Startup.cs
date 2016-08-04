using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarDB.Startup))]
namespace CarDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
