using HarmonyLib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen.Legacy.v1
{
    internal sealed class StateProviderV1
    {
        private static readonly AccessTools.FieldRef<Module, IList> _initialStateOptions =
            AccessTools.FieldRefAccess<Module, IList>("_initialStateOptions");

        private readonly List<InitialStateOption> _containers = new List<InitialStateOption>();

        public ContainerWrapper Get(string name)
        {
            var obj = Module.CurrentModule.GetInitialStateOptionWithId(name);
            if (obj != null)
                return new ContainerWrapper(obj);
            return default;
        }

        public void CreateNewSharedState(object settings)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).FirstOrDefault(a => Path.GetFileName(a.Location) == "MBOptionScreen.dll");
            if (assembly != null)
            {
                var settingsProviderWrapperType = assembly.GetType("MBOptionScreen.Settings.SettingsProviderWrapper");
                var settingsProviderWrapper = Activator.CreateInstance(settingsProviderWrapperType, new object[] { settings });

                var sharedStateObjectType = assembly.GetType("MBOptionScreen.State.SharedStateObject");
                var sharedStateObject = Activator.CreateInstance(sharedStateObjectType, new object[] { settingsProviderWrapper, null!, null! });
                var hasInitializedProperty = sharedStateObjectType.GetProperty("HasInitialized");
                hasInitializedProperty.SetValue(sharedStateObject, true);

                var containerType = assembly.GetType("MBOptionScreen.State.DefaultStateProvider").GetNestedType("Container", BindingFlags.NonPublic);
                var container = (InitialStateOption) Activator.CreateInstance(containerType, new object[] { sharedStateObject });
                Module.CurrentModule.AddInitialStateOption(container);
                _containers.Add(container);


                var settingsDatabaseType = assembly.GetType("MBOptionScreen.Settings.SettingsDatabase");

                var mbOptionScreenSubModuleType = assembly.GetType("MBOptionScreen.MBOptionScreenSubModule");
                var sharedStateObjectField = AccessTools.Field(mbOptionScreenSubModuleType, "SharedStateObject");
                sharedStateObjectField.SetValue(null, sharedStateObject);
            }
        }

        public void Clear()
        {
            var list = _initialStateOptions(Module.CurrentModule);
            foreach (var container in _containers)
                list.Remove(container);
            _containers.Clear();
        }

        public class ContainerWrapper
        {
            private readonly object _object;
            private readonly PropertyInfo _sharedStateObjectProperty;
            private readonly PropertyInfo _settingsStorageProperty;

            public object SharedStateObject
            {
                get => _sharedStateObjectProperty.GetValue(_object);
                internal set => _sharedStateObjectProperty.SetValue(_object, value);
            }

            public ContainerWrapper(object @object)
            {
                _object = @object;
                _sharedStateObjectProperty = AccessTools.Property(_object.GetType(), "SharedStateObject");
                _settingsStorageProperty = AccessTools.Property(SharedStateObject.GetType(), "SettingsStorage");
            }

            public void SetSettings(object settings)
            {
                var settingsProviderWrapperType = _object.GetType().Assembly.GetType("MBOptionScreen.Settings.SettingsProviderWrapper");
                var settingsProviderWrapper = Activator.CreateInstance(settingsProviderWrapperType, new object[] { settings });
                _settingsStorageProperty.SetValue(SharedStateObject, settingsProviderWrapper);
            }

            public void SetInitialized()
            {
                var hasInitializedProperty = SharedStateObject.GetType().GetProperty("HasInitialized");
                hasInitializedProperty.SetValue(SharedStateObject, true);
            }
        }
    }
}