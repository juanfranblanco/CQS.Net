using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CQS.Demo.Startup))]
namespace CQS.Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
