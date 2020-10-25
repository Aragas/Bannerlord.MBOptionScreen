using HarmonyLib;

using System.Reflection;

namespace MCM.Implementation.ModLib.Utils
{
    // TODO: User ButterLib
    internal static class AccessTools3
    {
        /// <summary>Creates an instance field reference</summary>
        /// <typeparam name="T">The class the field is defined in</typeparam>
        /// <typeparam name="F">The type of the field</typeparam>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A read and writable field reference delegate</returns>
        public static AccessTools.FieldRef<T, F>? FieldRefAccess<T, F>(string fieldName)
        {
            var field = typeof(T).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            return field == null ? null : AccessTools.FieldRefAccess<T, F>(field);
        }
    }
}