using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SolutionName.Web.Startup))]
namespace SolutionName.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
