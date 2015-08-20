using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Meditech.Startup))]
namespace Meditech
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
