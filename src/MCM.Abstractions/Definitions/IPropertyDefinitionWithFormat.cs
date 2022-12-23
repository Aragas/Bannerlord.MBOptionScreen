namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionWithFormat
    {
        /// <summary>
        /// The format in which the slider's value will be displayed in.
        /// </summary>
        string ValueFormat { get; }
    }
}