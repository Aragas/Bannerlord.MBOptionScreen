using HarmonyLib;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Patches
{
    internal static class WidgetPrefabPatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.CreateReversePatcher(
                SymbolExtensions.GetMethodInfo(() => WidgetPrefab.LoadFrom(null!, null!, null!)),
                new HarmonyMethod(SymbolExtensions.GetMethodInfo(() => LoadFromDocument(null!, null!, null!, null!)))).Patch();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static WidgetPrefab LoadFromDocument(
            PrefabExtensionContext prefabExtensionContext,
            WidgetAttributeContext widgetAttributeContext,
            string path,
            XmlDocument document)
        {
            // Replaces reading XML from file with assigning it from the new local variable `XmlDocument document`
            [MethodImpl(MethodImplOptions.NoInlining)]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase method)
            {
                var returnNull = new List<CodeInstruction>
                {
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Ret)
                }.AsEnumerable();

                var instructionList = instructions.ToList();

                var locals = method.GetMethodBody()?.LocalVariables;
                var typeLocal = locals?.FirstOrDefault(x => x.LocalType == typeof(XmlDocument));

                if (typeLocal == null)
                    return returnNull;

                var constructorIndex = -1;
                var constructor = AccessTools.Constructor(typeof(WidgetPrefab));
                for (var i = 0; i < instructionList.Count; i++)
                {
                    if (instructionList[i].opcode == OpCodes.Newobj && Equals(instructionList[i].operand, constructor))
                        constructorIndex = i;
                }

                if (constructorIndex == -1)
                    return returnNull;

                for (var i = 0; i < constructorIndex; i++)
                {
                    instructionList[i] = new CodeInstruction(OpCodes.Nop);
                }

                instructionList[constructorIndex - 2] = new CodeInstruction(OpCodes.Ldarg_S, 3);
                instructionList[constructorIndex - 1] = new CodeInstruction(OpCodes.Stloc_S, typeLocal.LocalIndex);

                return instructionList.AsEnumerable();
            }

            // make compiler happy
            _ = Transpiler(null!, null!);

            // make analyzer happy
            prefabExtensionContext.AddExtension(null);
            widgetAttributeContext.RegisterKeyType(null);
            path.Do(null);
            document.Validate(null);

            // make compiler happy
            return null!;
        }
    }
}