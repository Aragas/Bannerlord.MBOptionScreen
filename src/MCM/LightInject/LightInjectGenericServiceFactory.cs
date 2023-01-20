using BUTR.DependencyInjection;

namespace MCM.LightInject
{

    internal class LightInjectGenericServiceFactory : IGenericServiceFactory
    {
        private readonly IServiceFactory _serviceFactory;

        public LightInjectGenericServiceFactory(IServiceFactory factory) => _serviceFactory = factory;
        public TService GetService<TService>() where TService : class => _serviceFactory.GetInstance<TService>();
    }
}