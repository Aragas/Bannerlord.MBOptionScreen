using MCM.Abstractions.FluentBuilder.Models;
using MCM.Common;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder
{
    /// <summary>
    /// An interface that defines the necessary members for implementing a settings property group builder.
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyGroupBuilder
    {
        /// <summary>
        /// The actual properties stored in the builder.
        /// </summary>
        Dictionary<string, ISettingsPropertyBuilder> Properties { get; }

        /// <inheritdoc cref="IPropertyGroupDefinition.GroupOrder"/>
        /// <param name="value">The value.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder SetGroupOrder(int value);

        /// <summary>
        /// Creates a Toggle(bool) property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddToggle(string id, string name, IRef @ref, Action<ISettingsPropertyGroupToggleBuilder>? builder);
        /// <summary>
        /// Creates a Bool property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddBool(string id, string name, IRef @ref, Action<ISettingsPropertyBoolBuilder>? builder);
        /// <summary>
        /// Creates a Dropdown property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="selectedIndex">See <see cref="IPropertyDefinitionDropdown.SelectedIndex"/>.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddDropdown(string id, string name, int selectedIndex, IRef @ref, Action<ISettingsPropertyDropdownBuilder>? builder);
        /// <summary>
        /// Creates an Integer Slider property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="minValue"><see cref="IPropertyDefinitionWithMinMax.MinValue"/>.</param>
        /// <param name="maxValue"><see cref="IPropertyDefinitionWithMinMax.MaxValue"/>.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddInteger(string id, string name, int minValue, int maxValue, IRef @ref, Action<ISettingsPropertyIntegerBuilder>? builder);
        /// <summary>
        /// Creates an Float Slider property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="minValue">See <see cref="IPropertyDefinitionWithMinMax.MinValue"/>.</param>
        /// <param name="maxValue">See <see cref="IPropertyDefinitionWithMinMax.MaxValue"/>.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddFloatingInteger(string id, string name, float minValue, float maxValue, IRef @ref, Action<ISettingsPropertyFloatingIntegerBuilder>? builder);
        /// <summary>
        /// Creates an TextBox property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddText(string id, string name, IRef @ref, Action<ISettingsPropertyTextBuilder>? builder);
        /// <summary>
        /// Creates an Button property.
        /// </summary>
        /// <param name="id">Internal ID that will be used for serialization.</param>
        /// <param name="name">Will be shown at the setting screen.</param>
        /// <param name="ref">Reference to the value used by the property.</param>
        /// <param name="content">Button content.</param>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddButton(string id, string name, IRef @ref, string content, Action<ISettingsPropertyButtonBuilder>? builder);
        /// <summary>
        /// Adds a custom property.
        /// The custom property should implement one of the interfaces defined in MCM.Abstractions.Settings.Definitions namespace.
        /// Currently there is no way of defining a custom UI Control.
        /// One of the possible fixes would be to use UIExtender library.
        /// </summary>
        /// <typeparam name="TSettingsPropertyBuilder">A derived class of <see cref="ISettingsPropertyBuilder"/>.</typeparam>
        /// <param name="builder">Settings property builder.</param>
        /// <returns>The settings property group builder.</returns>
        ISettingsPropertyGroupBuilder AddCustom<TSettingsPropertyBuilder>(ISettingsPropertyBuilder<TSettingsPropertyBuilder> builder)
            where TSettingsPropertyBuilder : ISettingsPropertyBuilder;

        /// <summary>
        /// Gets this property group definition.
        /// </summary>
        /// <returns>A property group definition.</returns>
        IPropertyGroupDefinition GetPropertyGroupDefinition();
    }
}