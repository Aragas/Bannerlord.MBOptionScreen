namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyDefinitionWithMinMax
    {
        /// <summary>
        /// The minimum value the setting can be set to. Used by the slider control.
        /// </summary>
        decimal MinValue { get; }

        /// <summary>
        /// The maximum value the setting can be set to. Used by the slider control.
        /// </summary>
        decimal MaxValue { get; }
    }
}