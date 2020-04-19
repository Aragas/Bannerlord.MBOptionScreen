using HarmonyLib;

namespace ModLib
{
    internal static class Utils
    {
        public static string GetModuleFolderName(object @object)
        {
            var type = @object.GetType();
            var subFolderProperty = AccessTools.Property(type, "ModuleFolderName");
            if (subFolderProperty != null)
                return (string) subFolderProperty.GetValue(@object);
            return string.Empty;
        }
        public static string GetSubFolder(object @object)
        {
            var type = @object.GetType();
            var subFolderProperty = AccessTools.Property(type, "SubFolder");
            if (subFolderProperty != null)
                return (string)subFolderProperty.GetValue(@object);
            return string.Empty;
        }
        public static string GetID(object @object)
        {
            var type = @object.GetType();
            var idProperty = AccessTools.Property(type, "ID") ?? AccessTools.Property(type, "Id");
            if (idProperty != null)
                return (string)idProperty.GetValue(@object);
            return string.Empty;
        }
        public static void SetID(object @object, string id)
        {
            var type = @object.GetType();
            var idProperty = AccessTools.Property(type, "ID") ?? AccessTools.Property(type, "Id");
            if (idProperty != null)
                idProperty.SetValue(@object, id);
        }
    }
}
