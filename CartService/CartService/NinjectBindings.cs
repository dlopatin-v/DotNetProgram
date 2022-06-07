using CartService.DAL;
using Ninject.Modules;

namespace CartService
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IGatewayCart>().To<GatewayCart>();
        }
    }
}
