using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BanMyPham.Startup))]
namespace BanMyPham
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
