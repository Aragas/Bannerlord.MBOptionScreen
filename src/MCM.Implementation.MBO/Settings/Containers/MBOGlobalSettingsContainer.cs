extern alias v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MCM.Implementation.MBO.Settings.Base;
using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;

namespace MCM.Implementation.MBO.Settings.Containers
{
    internal sealed class MBOGlobalSettingsContainer : BaseGlobalSettingsContainer, IMBOGlobalSettingsContainer
    {
        public MBOGlobalSettingsContainer()
        {
            var settings = new List<GlobalSettings>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                // ignore v1 and v2 classes
                .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            var mbOptionScreenSettings = allTypes
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t,  "MBOptionScreen.Settings.SettingsBase"))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MBOptionScreen.Settings.EmptySettings"))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MBOptionScreen.Settings.SettingsWrapper"))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MBOptionScreen.Settings.ModLibSettings"))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MBOptionScreen.Settings.TestSettingsBase<>"))
                .Select(obj => new MBOGlobalSettingsWrapper(Activator.CreateInstance(obj)));
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
                RegisterSettings(setting);
        }
    }
}