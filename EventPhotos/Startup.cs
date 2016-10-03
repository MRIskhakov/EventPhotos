using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventPhotos.Startup))]
namespace EventPhotos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
