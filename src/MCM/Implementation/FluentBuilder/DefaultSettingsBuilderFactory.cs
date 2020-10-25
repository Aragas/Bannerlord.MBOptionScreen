using MCM.Abstractions.FluentBuilder;

namespace MCM.Implementation.FluentBuilder
{
    internal sealed class DefaultSettingsBuilderFactory : ISettingsBuilderFactory
    {
        public ISettingsBuilder Create(string id, string displayName) => new DefaultSettingsBuilder(id, displayName);
    }
}