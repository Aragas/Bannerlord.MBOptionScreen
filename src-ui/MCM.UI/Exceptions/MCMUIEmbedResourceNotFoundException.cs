using MCM.Common;

using System;
using System.Runtime.Serialization;

namespace MCM.UI.Exceptions;

[Serializable]
public class MCMUIEmbedResourceNotFoundException : MCMException
{
    public MCMUIEmbedResourceNotFoundException() { }
    public MCMUIEmbedResourceNotFoundException(string message) : base(message) { }
    public MCMUIEmbedResourceNotFoundException(string message, Exception inner) : base(message, inner) { }
    protected MCMUIEmbedResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}