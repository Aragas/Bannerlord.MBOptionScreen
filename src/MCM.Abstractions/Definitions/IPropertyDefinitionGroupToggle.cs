namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionGroupToggle : IPropertyDefinitionBase
    {
        bool IsToggle { get; }
    }
}