﻿namespace MCM.Utils
{
    public sealed class InformationMessageWrapper
    {
        public static InformationMessageWrapper Create(object @object) => new(@object);

        public object Object { get; }

        private InformationMessageWrapper(object @object)
        {
            Object = @object;
        }
    }
}