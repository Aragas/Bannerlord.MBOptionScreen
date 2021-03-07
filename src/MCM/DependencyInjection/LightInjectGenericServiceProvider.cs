using MCM.LightInject;

namespace MCM.DependencyInjection
{
    internal class LightInjectGenericServiceProvider : IGenericServiceProvider
    {
        private readonly IServiceContainer _serviceContainer;

        public LightInjectGenericServiceProvider(IServiceContainer serviceContainer) => _serviceContainer = serviceContainer;

        public TService GetService<TService>() where TService : class => _serviceContainer.GetInstance<TService>();
    }
}