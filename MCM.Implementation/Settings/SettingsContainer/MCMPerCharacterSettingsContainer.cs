using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.SettingsContainer
{
    [Version("e1.0.0", 1)]
    [Version("e1.0.1", 1)]
    [Version("e1.0.2", 1)]
    [Version("e1.0.3", 1)]
    [Version("e1.0.4", 1)]
    [Version("e1.0.5", 1)]
    [Version("e1.0.6", 1)]
    [Version("e1.0.7", 1)]
    [Version("e1.0.8", 1)]
    [Version("e1.0.9", 1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0", 1)]
    [Version("e1.2.0", 1)]
    [Version("e1.2.1", 1)]
    [Version("e1.3.0", 1)]
    internal sealed class MCMPerCharacterSettingsContainer : BasePerCharacterSettingsContainer, IPerCharacterContainer
    {
        public void OnGameStarted(Game game)
        {
            LoadedSettings.Clear();

            var settings = new List<PerCharacterSettings>();
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
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(PerCharacterSettings)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(EmptyPerCharacterSettings)))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(IWrapper)))
#if !DEBUG
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(TestSettingsBase<>)))
#endif
                .Select(obj => BasePerCharacterSettingsWrapper.Create(Activator.CreateInstance(obj)));
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
                RegisterSettings(setting);
        }

        public void OnGameEnded(Game game)
        {
            LoadedSettings.Clear();
        }
    }
}