using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TF_Finanzas.Startup))]
namespace TF_Finanzas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
