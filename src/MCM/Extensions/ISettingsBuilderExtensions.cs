using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions.Xml;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TaleWorlds.Engine;
using Path = System.IO.Path;

namespace MCM.Extensions
{
    public static class ISettingsBuilderExtensions
    {
        public static ISettingsBuilder? BuildFromXmlStream(Stream xmlStream)
        {
            using var reader = XmlReader.Create(xmlStream, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            var serializer = new XmlSerializer(typeof(SettingsXmlModel));
            if (!serializer.CanDeserialize(reader) || !(serializer.Deserialize(reader) is SettingsXmlModel settingsXmlModel))
                return null;

            var builder = BaseSettingsBuilder.Create(settingsXmlModel.Id, settingsXmlModel.DisplayName)?
                .SetUIVersion(settingsXmlModel.UIVersion)
                .SetSubGroupDelimiter(settingsXmlModel.SubGroupDelimiter[0])
                .CreateGroup(SettingsPropertyGroupDefinition.DefaultGroupName, groupBuilder => AddProperties(groupBuilder, settingsXmlModel.Properties));
            if (builder != null)
                AddGroups(builder, settingsXmlModel.Groups);
            return builder;
        }

        private static void AddGroups(ISettingsBuilder builder, IEnumerable<PropertyGroupXmlModel> groups)
        {
            foreach (var propertyGroup in groups)
            {
                builder.CreateGroup(propertyGroup.GroupName, groupBuilder =>
                {
                    groupBuilder.SetGroupOrder(propertyGroup.GroupOrder);
                    AddProperties(groupBuilder, propertyGroup.Properties);
                });
                //AddGroups(builder, propertyGroup.Groups);
            }
        }

        private static void AddProperties(ISettingsPropertyGroupBuilder groupBuilder, IEnumerable<PropertyBaseXmlModel> properties)
        {
            foreach (var property in properties)
            {
                switch (property)
                {
                    case PropertyBoolXmlModel boolXmlModel:
                        groupBuilder.AddBool(boolXmlModel.Id, boolXmlModel.DisplayName, new ProxyRef<object>(null, null), textBuilder =>
                            textBuilder
                                .SetHintText(boolXmlModel.HintText)
                                .SetOrder(boolXmlModel.Order)
                                .SetRequireRestart(boolXmlModel.RequireRestart));
                        break;
                    case PropertyDropdownXmlModel dropdownXmlModel:
                        groupBuilder.AddDropdown(dropdownXmlModel.Id, dropdownXmlModel.DisplayName, dropdownXmlModel.SelectedIndex, new ProxyRef<object>(null, null), textBuilder =>
                            textBuilder
                                .SetHintText(dropdownXmlModel.HintText)
                                .SetOrder(dropdownXmlModel.Order)
                                .SetRequireRestart(dropdownXmlModel.RequireRestart));
                        break;
                    case PropertyFloatingIntegerXmlModel floatingIntegerXmlModel:
                        groupBuilder.AddFloatingInteger(floatingIntegerXmlModel.Id, floatingIntegerXmlModel.DisplayName, (float) floatingIntegerXmlModel.MinValue, (float) floatingIntegerXmlModel.MaxValue, new ProxyRef<object>(null, null), textBuilder =>
                            textBuilder
                                .SetHintText(floatingIntegerXmlModel.HintText)
                                .SetOrder(floatingIntegerXmlModel.Order)
                                .SetRequireRestart(floatingIntegerXmlModel.RequireRestart));
                        break;
                    case PropertyIntegerXmlModel integerXmlModel:
                        groupBuilder.AddInteger(integerXmlModel.Id, integerXmlModel.DisplayName, (int) integerXmlModel.MinValue, (int) integerXmlModel.MaxValue, new ProxyRef<object>(null, null), textBuilder =>
                            textBuilder
                                .SetHintText(integerXmlModel.HintText)
                                .SetOrder(integerXmlModel.Order)
                                .SetRequireRestart(integerXmlModel.RequireRestart));
                        break;
                    case PropertyTextXmlModel textXmlModel:
                        groupBuilder.AddText(textXmlModel.Id, textXmlModel.DisplayName, new ProxyRef<object>(null, null), textBuilder =>
                            textBuilder
                                .SetHintText(textXmlModel.HintText)
                                .SetOrder(textXmlModel.Order)
                                .SetRequireRestart(textXmlModel.RequireRestart));
                        break;
                }
            }
        }
    }
}