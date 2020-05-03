namespace MCM.UI.Actions
{
    internal interface IInitial
    {
        IRef Context { get; }
        object Value { get; }
        void Reset();
        bool Changed();
    }
}