using MCM.Common;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder
{
    // Setter/Getter
    /// <summary>
    /// An interface that defines the necessary members for implementing a settings property builder.
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyBuilder
    {
        /// <summary>
        /// See <see cref="IPropertyDefinitionBase.DisplayName"/>
        /// </summary>
        string Name { get; }
        /// <summary>
        /// <see cref="Mono.Cecil.PropertyReference"/>
        /// </summary>
        IRef PropertyReference { get; }

        /// <summary>
        /// Gets this property definition.
        /// </summary>
        /// <returns>An enumerable of the property definitions.</returns>
        IEnumerable<IPropertyDefinitionBase> GetDefinitions();
    }

    /// <summary>
    /// An interface that defines the necessary members for implementing a settings property builder.
    /// </summary>
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyBuilder<out TSettingsPropertyBuilder> : ISettingsPropertyBuilder
        where TSettingsPropertyBuilder : ISettingsPropertyBuilder
    {
        /// <inheritdoc cref="IPropertyDefinitionBase.Order"/>
        /// <param name="value">The value.</param>
        /// <returns>The settings property builder.</returns>
        TSettingsPropertyBuilder SetOrder(int value);
        /// <inheritdoc cref="IPropertyDefinitionBase.RequireRestart"/>
        /// <param name="value">The value.</param>
        /// <returns>The settings property builder.</returns>
        TSettingsPropertyBuilder SetRequireRestart(bool value);
        /// <inheritdoc cref="IPropertyDefinitionBase.HintText"/>
        /// <param name="value">The value.</param>
        /// <returns>The settings property builder.</returns>
        TSettingsPropertyBuilder SetHintText(string value);
    }
}