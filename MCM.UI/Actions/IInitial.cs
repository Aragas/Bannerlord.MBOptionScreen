namespace MCM.UI.Actions
{
    internal interface IInitial
    {
        Ref Context { get; }
        object Value { get; }
        void Reset();
        bool Changed();
    }
}