using System;
using System.Runtime.Serialization;

namespace MCM.Common
{
    [Serializable]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class MCMException : Exception
    {
        public MCMException() { }
        public MCMException(string message) : base(message) { }
        public MCMException(string message, Exception inner) : base(message, inner) { }
        protected MCMException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}