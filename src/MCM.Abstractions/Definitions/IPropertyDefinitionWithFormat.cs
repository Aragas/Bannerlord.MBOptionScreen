namespace MCM.Abstractions
{
    public interface IPropertyDefinitionWithFormat
    {
        /// <summary>
        /// The format in which the slider's value will be displayed in.
        /// </summary>
        string ValueFormat { get; }
    }
}