namespace MCMv5.Tests.Utils
{
    public sealed class InquiryDataWrapper
    {
        public static InquiryDataWrapper Create(object @object) => new(@object);

        public object Object { get; }

        private InquiryDataWrapper(object @object)
        {
            Object = @object;
        }
    }
}