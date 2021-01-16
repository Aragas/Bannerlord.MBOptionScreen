namespace MCM.Abstractions.Synchronization
{
    public sealed class SynchronizationProviderWrapper : BaseSynchronizationProvider, IWrapper
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public SynchronizationProviderWrapper(object @object) { }

        public override string Name => "ERROR";
        public override bool IsFirstInitialization => false;
        public override void Dispose() { }
    }
}