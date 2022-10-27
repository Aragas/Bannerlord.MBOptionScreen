using HarmonyLib.BUTR.Extensions;

using System;
using System.Linq;
using System.Reflection;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.Utils
{
    public static class HintViewModelUtils
    {
        private delegate ViewModel Ctor1V1Delegate();
        private static readonly Ctor1V1Delegate? Ctor1V1;

        private delegate ViewModel Ctor2V1Delegate(TextObject hintText, string? uniqueName = null);
        private static readonly Ctor2V1Delegate? Ctor2V1;

        static HintViewModelUtils()
        {
            var type = AccessTools2.TypeByName("TaleWorlds.Core.ViewModelCollection.HintViewModel") ??
                       AccessTools2.TypeByName("TaleWorlds.Core.ViewModelCollection.Information.HintViewModel");
            foreach (var methodInfo in HarmonyLib.AccessTools.GetDeclaredMethods(type) ?? Enumerable.Empty<MethodInfo>())
            {
                var @params = methodInfo.GetParameters();
                if (@params.Length == 0)
                {
                    Ctor1V1 = AccessTools2.GetDelegate<Ctor1V1Delegate>(methodInfo);
                }
                if (@params.Length == 2 && @params[0].ParameterType.Name.Equals("TextObject", StringComparison.Ordinal) && @params[1].ParameterType == typeof(string))
                {
                    Ctor2V1 = AccessTools2.GetDelegate<Ctor2V1Delegate>(methodInfo);
                }
            }
        }

        public static ViewModel? Create()
        {
            if (Ctor1V1 is not null)
                return Ctor1V1();

            return null;
        }

        public static ViewModel? Create(TextObject hintText, string? uniqueName = null)
        {
            if (Ctor2V1 is not null)
                return Ctor2V1(hintText, uniqueName);

            return null;
        }
    }
}