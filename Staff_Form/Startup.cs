using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Staff_Form.Startup))]
namespace Staff_Form
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
