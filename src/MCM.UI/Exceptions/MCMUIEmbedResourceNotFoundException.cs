using MCM.Exceptions;

using System;
using System.Runtime.Serialization;

namespace MCM.UI.Exceptions
{
    [Serializable]
    public class MCMUIEmbedResourceNotFoundException : MCMException
    {
        public MCMUIEmbedResourceNotFoundException() : base() { }
        public MCMUIEmbedResourceNotFoundException(string message) : base(message) { }
        public MCMUIEmbedResourceNotFoundException(string message, Exception inner) : base(message, inner) { }
        public MCMUIEmbedResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}