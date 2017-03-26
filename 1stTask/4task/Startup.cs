using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_4task.Startup))]
namespace _4task
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
