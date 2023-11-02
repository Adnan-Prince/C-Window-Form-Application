using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Search_n_View.Startup))]
namespace Search_n_View
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
