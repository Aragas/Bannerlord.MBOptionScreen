using HarmonyLib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal static class PrefabInjector
    {
        private static FieldInfo CustomTypesField { get; } = AccessTools.Field(typeof(WidgetFactory), "_customTypes");

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
            ResourcesPathField().SetValue(customWidgetType, string.Empty);
            NameProperty().SetValue(customWidgetType, name);
            WidgetFactoryProperty().SetValue(customWidgetType, UIResourceManager.WidgetFactory);
            WidgetPrefabProperty().SetValue(customWidgetType, widgetPrefab);

            var dict = (IDictionary) CustomTypesField.GetValue(UIResourceManager.WidgetFactory);
            if (!dict.Contains(name))
                dict.Add(name, customWidgetType);

            return widgetPrefab;
        }

        private static WidgetPrefab LoadFromDocument(PrefabExtensionContext prefabExtensionContext, WidgetAttributeContext widgetAttributeContext, XmlDocument xmlDocument)
        {
            static MethodInfo LoadParametersMethod() => AccessTools.Method(typeof(WidgetPrefab), "LoadParameters");
            static MethodInfo LoadConstantsMethod() => AccessTools.Method(typeof(WidgetPrefab), "LoadConstants");
            static MethodInfo LoadCustomElementsMethod() => AccessTools.Method(typeof(WidgetPrefab), "LoadCustomElements");
            static MethodInfo LoadVisualDefinitionsMethod() => AccessTools.Method(typeof(WidgetPrefab), "LoadVisualDefinitions");
            static PropertyInfo RootTemplateProperty() => AccessTools.Property(typeof(WidgetPrefab), "RootTemplate");
            static MethodInfo OnLoadingFinishedMethod() => AccessTools.Method(typeof(PrefabExtension), "OnLoadingFinished");


            WidgetPrefab widgetPrefab = new WidgetPrefab();

            WidgetTemplate widgetTemplate;

            var xmlNode = xmlDocument.SelectSingleNode("Prefab");
            if (xmlNode != null)
            {
                var parameters = xmlNode.SelectSingleNode("Parameters");
                if (parameters != null)
                    widgetPrefab.Parameters =(Dictionary<string, string>) LoadParametersMethod().Invoke(null, new object[] { parameters });

                var constants = xmlNode.SelectSingleNode("Constants");
                if (constants != null)
                    widgetPrefab.Constants = (Dictionary<string, ConstantDefinition>) LoadConstantsMethod().Invoke(null, new object[] { constants });

                var variables = xmlNode.SelectSingleNode("Variables");

                var visualDefinitions = xmlNode.SelectSingleNode("VisualDefinitions");
                if (visualDefinitions != null)
                    widgetPrefab.VisualDefinitionTemplates = (Dictionary<string, VisualDefinitionTemplate>) LoadVisualDefinitionsMethod().Invoke(null, new object[] { visualDefinitions });

                var customElements = xmlNode.SelectSingleNode("CustomElements");
                if (customElements != null)
                    widgetPrefab.CustomElements = (Dictionary<string, XmlElement>) LoadCustomElementsMethod().Invoke(null, new object[] { customElements });

                widgetTemplate = WidgetTemplate.LoadFrom(prefabExtensionContext, widgetAttributeContext, xmlNode.SelectSingleNode("Window").FirstChild);
            }
            else
            {
                widgetTemplate = WidgetTemplate.LoadFrom(prefabExtensionContext, widgetAttributeContext, xmlDocument.SelectSingleNode("Window").FirstChild);
            }

            widgetTemplate.SetRootTemplate(widgetPrefab);
            RootTemplateProperty().SetValue(widgetPrefab, widgetTemplate);

            foreach (var prefabExtension in prefabExtensionContext.PrefabExtensions)
                OnLoadingFinishedMethod().Invoke(prefabExtension, new object[] { widgetPrefab });

            return widgetPrefab;
        }
    }
}