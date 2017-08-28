using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_da544B.Startup))]
namespace web_da544B
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
