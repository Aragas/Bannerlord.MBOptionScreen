using System;
using System.Runtime.Serialization;

namespace MCM.Exceptions
{
    [Serializable]
    public class MCMException : Exception
    {
        public MCMException() { }
        public MCMException(string message) : base(message) { }
        public MCMException(string message, Exception inner) : base(message, inner) { }
        protected MCMException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}