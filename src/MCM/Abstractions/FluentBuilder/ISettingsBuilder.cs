using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base.PerCampaign;

using System;
using System.ComponentModel;

namespace MCM.Abstractions.FluentBuilder
{
    /// <summary>
    /// An interface that defines the necessary members for implementing a settings builder.
    /// </summary>
    public interface ISettingsBuilder
    {
        /// <summary>
        /// See <see cref="BaseSettings.FolderName"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder SetFolderName(string value);
        /// <summary>
        /// See <see cref="BaseSettings.SubFolder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder SetSubFolder(string value);
        /// <summary>
        /// See <see cref="BaseSettings.FormatType"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder SetFormat(string value);
        /// <summary>
        /// See <see cref="BaseSettings.UIVersion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder SetUIVersion(int value);
        /// <summary>
        /// See <see cref="BaseSettings.SubGroupDelimiter"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder SetSubGroupDelimiter(char value);
        /// <summary>
        /// See <see cref="BaseSettings.OnPropertyChanged"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder SetOnPropertyChanged(PropertyChangedEventHandler value);

        /// <summary>
        /// Creates a property group where you can define your properties.
        /// The default Group name is 'Misc'.
        /// Use the action delegate to configure the property group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="builder"></param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder CreateGroup(string name, Action<ISettingsPropertyGroupBuilder> builder);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="builder"></param>
        /// <returns>The settings builder.</returns>
        ISettingsBuilder CreatePreset(string name, Action<ISettingsPresetBuilder> builder);

        /// <summary>
        /// Returns a Global setting instance.
        /// Use Register and Unregister for MCM to use it.
        /// </summary>
        /// <returns></returns>
        FluentGlobalSettings BuildAsGlobal();
        /// <summary>
        /// Returns a PerCampaign setting instance.
        /// Use Register and Unregister for MCM to use it.
        /// The registered settings will be cleared before and after player joins the campaign, so do the register thing when the campaign was already joined in.
        /// </summary>
        /// <returns></returns>
        FluentPerCampaignSettings BuildAsPerCampaign();
    }
}