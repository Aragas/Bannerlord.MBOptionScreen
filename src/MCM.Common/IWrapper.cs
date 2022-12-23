namespace MCM.Common
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IWrapper
    {
        object Object { get; }
    }
}