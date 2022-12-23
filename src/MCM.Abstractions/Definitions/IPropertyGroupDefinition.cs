namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyGroupDefinition
    {
        /// <summary>
        /// The name of the settings group. Includes SubGroup notation if present.
        /// </summary>
        string GroupName { get; }

        int GroupOrder { get; }
    }
}