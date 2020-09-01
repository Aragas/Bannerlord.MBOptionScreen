extern alias v4;

using v4::MCM.Abstractions.Settings.Definitions;
using v4::MCM.Abstractions.Settings.Definitions.Wrapper;

namespace MCM.Implementation.MCMv3.Settings.Definitions
{
    internal sealed class MCMv3PropertyDefinitionTextWrapper : BasePropertyDefinitionWrapper,
        IPropertyDefinitionText
    {
        public MCMv3PropertyDefinitionTextWrapper(object @object) : base(@object) { }
    }
}