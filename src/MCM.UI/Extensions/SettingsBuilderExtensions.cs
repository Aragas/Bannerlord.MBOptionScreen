using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.SubSystems;

using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

using TaleWorlds.Core;

namespace MCM.UI.Extensions
{
    internal static class SettingsBuilderExtensions
    {
        public static ISettingsBuilder AddButterLibSubSystems(this ISettingsBuilder settings)
        {
            foreach (var subSystem in DependencyInjectionExtensions.GetServiceProvider((null as Game)!).GetRequiredService<IEnumerable<ISubSystem>>())
            {
                if (!subSystem.CanBeDisabled)
                    continue;

                var prop = new ProxyRef<bool>(() => subSystem.IsEnabled, state =>
                {
                    if (state)
                        subSystem.Enable();
                    else
                        subSystem.Disable();
                });
                settings = settings.CreateGroup($"SubSystem {subSystem.Id}", builder => builder.AddBool($"{subSystem.Id} Enabled", "Enabled", prop,
                    bBuilder => bBuilder.SetHintText(subSystem.Description).SetRequireRestart(!subSystem.CanBeSwitchedAtRuntime)));
            }
            return settings;
        }
    }
}