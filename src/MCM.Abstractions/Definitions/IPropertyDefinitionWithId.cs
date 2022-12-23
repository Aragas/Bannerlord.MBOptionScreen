namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionWithId
    {
        string Id { get; }
    }
}