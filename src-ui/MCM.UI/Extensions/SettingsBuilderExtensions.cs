using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.SubSystems;
using Bannerlord.ButterLib.SubSystems.Settings;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.FluentBuilder;
using MCM.Common;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace MCM.UI.Extensions
{
    internal static class SettingsBuilderExtensions
    {
        private static PropertyInfo? GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression) =>
            expression is LambdaExpression { Body: MemberExpression { Member: PropertyInfo propertyInfo } } ? propertyInfo : null;

        private static void AddForSubSystem<TSubSystem>(TSubSystem subSystem, ISettingsBuilder settingsBuilder) where TSubSystem : ISubSystem
        {
            if (subSystem is not ISubSystemSettings<TSubSystem> settings)
                return;

            foreach (var declaration in settings.Declarations)
            {
                settingsBuilder.CreateGroup($"SubSystem {subSystem.Id}", groupBuilder =>
                {
                    _ = declaration switch
                    {
                        SubSystemSettingsPropertyBool<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi =>
                            groupBuilder.AddBool($"{subSystem.GetType().Name}_{pi.Name}", new TextObject(sp.Name).ToString(), new PropertyRef(pi, subSystem), builder =>
                            {
                                builder.SetHintText(sp.Description);
                            }),
                        SubSystemSettingsPropertyDropdown<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi =>
                            groupBuilder.AddDropdown($"{subSystem.GetType().Name}_{pi.Name}", new TextObject(sp.Name).ToString(), sp.SelectedIndex, new PropertyRef(pi, subSystem), builder =>
                            {
                                builder.SetHintText(sp.Description);
                            }),
                        SubSystemSettingsPropertyFloat<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi =>
                            groupBuilder.AddFloatingInteger($"{subSystem.GetType().Name}_{pi.Name}", new TextObject(sp.Name).ToString(), sp.MinValue, sp.MaxValue, new PropertyRef(pi, subSystem), builder =>
                            {
                                builder.SetHintText(sp.Description);
                            }),
                        SubSystemSettingsPropertyInt<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi =>
                            groupBuilder.AddInteger($"{subSystem.GetType().Name}_{pi.Name}", new TextObject(sp.Name).ToString(), sp.MinValue, sp.MaxValue, new PropertyRef(pi, subSystem), builder =>
                            {
                                builder.SetHintText(sp.Description);
                            }),
                        SubSystemSettingsPropertyText<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi =>
                            groupBuilder.AddText($"{subSystem.GetType().Name}_{pi.Name}", new TextObject(sp.Name).ToString(), new PropertyRef(pi, subSystem), builder =>
                            {
                                builder.SetHintText(sp.Description);
                            }),
                    };
                });
            }
        }

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

                var subSystemType = subSystem.GetType();
                if (subSystemType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISubSystemSettings<>)))
                {
                    var method = AccessTools2.DeclaredMethod(typeof(SettingsBuilderExtensions), "AddForSubSystem")?.MakeGenericMethod(subSystemType);
                    method?.Invoke(null, [subSystem, settings]);
                }
            }
            return settings;
        }
    }
}