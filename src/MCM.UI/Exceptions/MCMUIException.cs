using System;
using System.Runtime.Serialization;
using MCM.Exceptions;

namespace MCM.UI.Exceptions
{
    [Serializable]
    public class MCMUIException : MCMException
    {
        public MCMUIException() { }
        public MCMUIException(string message) : base(message) { }
        public MCMUIException(string message, Exception inner) : base(message, inner) { }
        protected MCMUIException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}