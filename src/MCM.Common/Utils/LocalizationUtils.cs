using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;

namespace MCM.Common
{
    public static class LocalizationUtils
    {
        private readonly ref struct TextObjectWrapper
        {
            private delegate object TextObjectCtorDelegate(string rawText, Dictionary<string, object>? attributes = null);
            private static readonly TextObjectCtorDelegate? _textObjectCtor =
                AccessTools2.GetDeclaredConstructorDelegate<TextObjectCtorDelegate>("TaleWorlds.Localization.TextObject", new[] { typeof(string), typeof(Dictionary<string, object>) });
            
            private delegate string? ToStringDelegate(object instance);
            private static readonly ToStringDelegate? _toString =
                AccessTools2.GetDeclaredDelegate<ToStringDelegate>("TaleWorlds.Localization.TextObject:ToString", Type.EmptyTypes);

            public static TextObjectWrapper Create(string rawText, Dictionary<string, object>? attributes = null) => new(_textObjectCtor?.Invoke(rawText, attributes));
            
            private readonly object? _object;

            private TextObjectWrapper(object? @object)
            {
                _object = @object;
            }

            public string Localize() => _object is not null && _toString is not null ? _toString(_object) ?? string.Empty : string.Empty;
        }
        
        public static string Localize(string rawText, Dictionary<string, object>? attributes = null) => TextObjectWrapper.Create(rawText, attributes).Localize();
    }
}