using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Common;
using MCM.UI.Adapter.MCMv5.Base;
using MCM.UI.Adapter.MCMv5.Presets;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MCM.UI.Adapter.MCMv5.Providers
{
    internal sealed class MCMv5SettingsProviderWrapper : SettingsProviderWrapper
    {
        private delegate IEnumerable GetPresetsDelegate(string settingsId);
        private readonly GetPresetsDelegate? _methodGetPresetsDelegate;

        public MCMv5SettingsProviderWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            _methodGetPresetsDelegate = AccessTools2.GetDelegate<GetPresetsDelegate>(@object, type, nameof(GetPresets));
        }

        protected override BaseSettings? Create(object obj)
        {
            var type = obj.GetType();

            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.Global.FluentGlobalSettings"))
                return new MCMv5FluentSettingsWrapper(obj);
            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.Global.GlobalSettings"))
                return new MCMv5AttributeSettingsWrapper(obj);

            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerCampaign.FluentPerCampaignSettings"))
                return new MCMv5FluentSettingsWrapper(obj);
            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerCampaign.PerCampaignSettings"))
                return new MCMv5AttributeSettingsWrapper(obj);

            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerSave.FluentPerSaveSettings"))
                return new MCMv5FluentSettingsWrapper(obj);
            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerSave.PerSaveSettings"))
                return new MCMv5AttributeSettingsWrapper(obj);

            return null;
        }

        protected override bool IsSettings(BaseSettings settings, [NotNullWhen(true)] out object? wrapped)
        {
            if (settings is (MCMv5AttributeSettingsWrapper or MCMv5FluentSettingsWrapper) and IWrapper { Object: { } obj })
            {
                wrapped = obj;
                return true;
            }

            wrapped = null;
            return false;
        }

        public override IEnumerable<ISettingsPreset> GetPresets(string settingsId)
        {
            var settings = GetSettings(settingsId);
            if (settings is not MCMv5AttributeSettingsWrapper or MCMv5FluentSettingsWrapper || settings is not IWrapper { Object: { } obj } || _methodGetPresetsDelegate is null)
                return Enumerable.Empty<ISettingsPreset>();

            var type = obj.GetType();
            var presets = _methodGetPresetsDelegate.Invoke(settingsId).OfType<object>();

            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.Global.FluentGlobalSettings"))
                return presets.Select(x => new MCMv5FluentSettingsPresetWrapper(x));
            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.Global.GlobalSettings"))
                return presets.Select(x => new MCMv5AttributeSettingsPresetWrapper(x));

            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerCampaign.FluentPerCampaignSettings"))
                return presets.Select(x => new MCMv5FluentSettingsPresetWrapper(x));
            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerCampaign.PerCampaignSettings"))
                return presets.Select(x => new MCMv5AttributeSettingsPresetWrapper(x));

            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerSave.FluentPerSaveSettings"))
                return presets.Select(x => new MCMv5FluentSettingsPresetWrapper(x));
            if (ReflectionUtils.ImplementsOrImplementsEquivalent(type, "MCM.Abstractions.Base.PerSave.PerSaveSettings"))
                return presets.Select(x => new MCMv5AttributeSettingsPresetWrapper(x));

            return Enumerable.Empty<ISettingsPreset>();
        }

        public override IEnumerable<UnavailableSetting> GetUnavailableSettings() => Enumerable.Empty<UnavailableSetting>();

        public override IEnumerable<SettingSnapshot> SaveAvailableSnapshots() => Enumerable.Empty<SettingSnapshot>();

        public override IEnumerable<BaseSettings> LoadAvailableSnapshots(IEnumerable<SettingSnapshot> snapshots) => Enumerable.Empty<BaseSettings>();
    }
}