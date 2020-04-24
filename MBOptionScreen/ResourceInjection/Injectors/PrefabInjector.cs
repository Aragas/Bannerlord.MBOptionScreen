using HarmonyLib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection.Injectors
{
    internal static class PrefabInjector
    {
        private static readonly AccessTools.FieldRef<WidgetFactory, IDictionary> _customTypes =
            AccessTools.FieldRefAccess<WidgetFactory, IDictionary>("_customTypes");

        public static WidgetPrefab InjectDocumentAndCreate(string name, XmlDocument doc)
        {
            static Type CustomWidgetType() => typeof(WidgetTemplate).Assembly
                .GetType("TaleWorlds.GauntletUI.PrefabSystem.CustomWidgetType");
            static FieldInfo ResourcesPathField() => CustomWidgetType()
                .GetField("_resourcesPath", BindingFlags.Instance | BindingFlags.NonPublic);
            static PropertyInfo NameProperty() => CustomWidgetType()
                .GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            static PropertyInfo WidgetFactoryProperty() => CustomWidgetType()
                .GetProperty("WidgetFactory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            static PropertyInfo WidgetPrefabProperty() => CustomWidgetType()
                .GetProperty("WidgetPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var widgetPrefab = LoadFromDocument(
                UIResourceManager.WidgetFactory.PrefabExtensionContext,
                UIResourceManager.WidgetFactory.WidgetAttributeContext,
                doc);

            var customWidgetType = FormatterServices.GetUninitializedObject(CustomWidgetType());
            ResourcesPathField().SetValue(customWidgetType, "");
            NameProperty().SetValue(customWidgetType, name);
            WidgetFactoryProperty().SetValue(customWidgetType, UIResourceManager.WidgetFactory);
            WidgetPrefabProperty().SetValue(customWidgetType, widgetPrefab);

            var dict = _customTypes(UIResourceManager.WidgetFactory);
            if (!dict.Contains(name))
                dict.Add(name, customWidgetType);

            return widgetPrefab;
        }

        private static WidgetPrefab LoadFromDocument(PrefabExtensionContext prefabExtensionContext, WidgetAttributeContext widgetAttributeContext, XmlDocument xmlDocument)
        {
            static MethodInfo LoadParametersMethod() => typeof(WidgetPrefab)
                .GetMethod("LoadParameters", BindingFlags.Static | BindingFlags.NonPublic);

            static MethodInfo LoadConstantsMethod() => typeof(WidgetPrefab)
                .GetMethod("LoadConstants", BindingFlags.Static | BindingFlags.NonPublic);

            static MethodInfo LoadCustomElementsMethod() => typeof(WidgetPrefab)
                .GetMethod("LoadCustomElements", BindingFlags.Static | BindingFlags.NonPublic);

            static MethodInfo LoadVisualDefinitionsMethod() => typeof(WidgetPrefab)
                .GetMethod("LoadVisualDefinitions", BindingFlags.Static | BindingFlags.NonPublic);

            static PropertyInfo RootTemplateProperty() => typeof(WidgetPrefab)
                .GetProperty("RootTemplate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            static MethodInfo OnLoadingFinishedMethod() => typeof(PrefabExtension)
                .GetMethod("OnLoadingFinished", BindingFlags.Instance | BindingFlags.NonPublic);

            var widgetPrefab = new WidgetPrefab();
            var xmlNode = xmlDocument.SelectSingleNode("Prefab");
            WidgetTemplate widgetTemplate;
            if (xmlNode != null)
            {
                var xmlNode2 = xmlNode.SelectSingleNode("Parameters");
                var xmlNode3 = xmlNode.SelectSingleNode("Constants");
                var xmlNode4 = xmlNode.SelectSingleNode("Variables");
                var xmlNode5 = xmlNode.SelectSingleNode("VisualDefinitions");
                var xmlNode6 = xmlNode.SelectSingleNode("CustomElements");
                var firstChild = xmlNode.SelectSingleNode("Window").FirstChild;
                widgetTemplate = LoadFrom(prefabExtensionContext, widgetAttributeContext, firstChild);

                if (xmlNode2 != null)
                {
                    widgetPrefab.Parameters =
                        (Dictionary<string, string>)LoadParametersMethod().Invoke(null, new object[] { xmlNode2 });
                }

                if (xmlNode3 != null)
                {
                    widgetPrefab.Constants =
                        (Dictionary<string, ConstantDefinition>)LoadConstantsMethod()
                            .Invoke(null, new object[] { xmlNode3 });
                }

                if (xmlNode6 != null)
                {
                    widgetPrefab.CustomElements =
                        (Dictionary<string, XmlElement>)LoadCustomElementsMethod()
                            .Invoke(null, new object[] { xmlNode6 });
                }

                if (xmlNode5 != null)
                {
                    widgetPrefab.VisualDefinitionTemplates =
                        (Dictionary<string, VisualDefinitionTemplate>)LoadVisualDefinitionsMethod()
                            .Invoke(null, new object[] { xmlNode5 });
                }
            }
            else
            {
                var firstChild2 = xmlDocument.SelectSingleNode("Window").FirstChild;
                widgetTemplate = LoadFrom(prefabExtensionContext, widgetAttributeContext, firstChild2);
            }

            widgetTemplate.SetRootTemplate(widgetPrefab);
            RootTemplateProperty().SetValue(widgetPrefab, widgetTemplate);
            foreach (var prefabExtension in prefabExtensionContext.PrefabExtensions)
            {
                OnLoadingFinishedMethod().Invoke(prefabExtension, new object[] { widgetPrefab });
            }

            return widgetPrefab;
        }

        // TODO: There was a bug in the native call. Investigate
        private static WidgetTemplate LoadFrom(PrefabExtensionContext prefabExtensionContext, WidgetAttributeContext widgetAttributeContext, XmlNode node)
        {
            static PropertyInfo LogicalChildrenLocationProperty() => typeof(WidgetTemplate)
                .GetProperty("LogicalChildrenLocation", BindingFlags.Instance | BindingFlags.NonPublic);

            var template = new WidgetTemplate(node.Name);
            LoadAttributeCollection(template, widgetAttributeContext, node.Attributes);
            //LoadAttributeCollectionMethod.Invoke(template, new object[] { widgetAttributeContext, node.Attributes });

            if (node.SelectSingleNode("LogicalChildrenLocation") != null)
                LogicalChildrenLocationProperty().SetValue(template, true);

            foreach (var prefabExtension in prefabExtensionContext.PrefabExtensions)
                DoLoading(prefabExtensionContext, widgetAttributeContext, template, node);

            var xmlNode = node.SelectSingleNode("Children");
            if (xmlNode != null)
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    var child = LoadFrom(prefabExtensionContext, widgetAttributeContext, childNode);
                    template.AddChild(child);
                }
            }

            return template;
        }
        private static void LoadAttributeCollection(WidgetTemplate instance, WidgetAttributeContext widgetAttributeContext, XmlAttributeCollection attributes)
        {
            static FieldInfo AttributesField() => typeof(WidgetTemplate)
                .GetField("_attributes", BindingFlags.Instance | BindingFlags.NonPublic);

            var dict = (Dictionary<Type, Dictionary<string, WidgetAttributeTemplate>>) AttributesField().GetValue(instance);
            foreach (var registeredKeyType in widgetAttributeContext.RegisteredKeyTypes)
                dict.Add(registeredKeyType.GetType(), new Dictionary<string, WidgetAttributeTemplate>());

            if (attributes == null)
                return;

            foreach (XmlAttribute attribute in attributes)
                instance.AddAttributeTo(widgetAttributeContext, attribute.Name, attribute.Value);
        }
        private static void DoLoading(PrefabExtensionContext prefabExtensionContext, WidgetAttributeContext widgetAttributeContext, WidgetTemplate template, XmlNode node)
        {
            var xmlNodeList = node.SelectNodes("ItemTemplate");
            ItemTemplateUsage itemTemplateUsage = null!;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                var xmlAttribute = xmlNode.Attributes["Type"];
                if (xmlAttribute == null || xmlAttribute.Value == "Default")
                {
                    var firstChild = xmlNode.FirstChild;
                    itemTemplateUsage =
                        new ItemTemplateUsage(LoadFrom(prefabExtensionContext, widgetAttributeContext, firstChild));
                    template.AddExtensionData(itemTemplateUsage);
                }
            }

            if (itemTemplateUsage != null)
            {
                foreach (XmlNode xmlNode2 in xmlNodeList)
                {
                    var xmlAttribute2 = xmlNode2.Attributes["Type"];
                    if (xmlAttribute2 != null && xmlAttribute2.Value != "Default")
                    {
                        var firstChild2 = xmlNode2.FirstChild;
                        var widgetTemplate = LoadFrom(prefabExtensionContext, widgetAttributeContext, firstChild2);
                        if (xmlAttribute2.Value == "First")
                            itemTemplateUsage.FirstItemTemplate = widgetTemplate;
                        else if (xmlAttribute2.Value == "Last")
                            itemTemplateUsage.LastItemTemplate = widgetTemplate;
                    }
                }
            }
        }
    }
}