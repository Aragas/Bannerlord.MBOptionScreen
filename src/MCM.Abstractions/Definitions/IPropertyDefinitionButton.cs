namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionButton : IPropertyDefinitionBase
    {
        public string Content { get; }
    }
}