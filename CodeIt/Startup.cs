using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeIt.Startup))]
namespace CodeIt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
