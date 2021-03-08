namespace MCM.DependencyInjection
{
    public static class GenericServiceProvider
    {
        internal static IGenericServiceProvider ServiceProvider { get; set; }

        public static TService? GetService<TService>() where TService : class => ServiceProvider?.GetService<TService>();
    }
}