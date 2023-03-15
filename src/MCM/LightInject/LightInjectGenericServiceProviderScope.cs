using BUTR.DependencyInjection;

namespace MCM.LightInject
{
    internal class LightInjectGenericServiceProviderScope : IGenericServiceProviderScope
    {
        private readonly Scope _scope;

        public LightInjectGenericServiceProviderScope(Scope scope) => _scope = scope;

        public TService? GetService<TService>() where TService : class => _scope.GetInstance<TService>();

        public void Dispose() => _scope.Dispose();
    }
}