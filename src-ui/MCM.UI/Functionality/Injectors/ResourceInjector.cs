using Bannerlord.BUTR.Shared.Helpers;

using MCM.UI.Exceptions;

using System;
using System.Xml;

namespace MCM.UI.Functionality.Injectors
{
    public abstract class ResourceInjector
    {
        // Game v1.4.0 rewrote TaleWorlds.GauntletUI's StackLayout and made the vertical
        // LayoutMethods literal. This swapped the visual order of "VerticalTopToBottom" and
        // "VerticalBottomToTop" compared to v1.3.x and older: on those older versions
        // "VerticalBottomToTop" actually rendered top-to-bottom (and vice versa), and MCM's
        // prefabs relied on that. (The same rewrite also reordered the LayoutMethod enum and
        // added LayoutMethod.VerticalSpaced.)
        //
        // Our prefabs are now authored using the new (literal, >= v1.4.0) semantics. On older
        // games we swap the two vertical methods back at load time so every list keeps reading
        // top-to-bottom.
        private static readonly bool SwapVerticalLayoutMethod = ComputeSwapVerticalLayoutMethod();

        // FloatInputTextWidget.EnableClamp exists from game v1.2.0 on; older games drop the attribute. It is set here
        // instead of in the prefabs, so the UIExtenderEx analyzers, which check the prefabs against every supported
        // version, find no attribute missing in some of them. Every FloatInputTextWidget of MCM clamps.
        // SettingsPropertyVM clamps the stored value on every version either way, the widget also corrects the typed text
        private static readonly bool EnableFloatInputClamp = ComputeEnableFloatInputClamp();

        private static bool ComputeSwapVerticalLayoutMethod()
        {
            // Anything older than v1.4.0 uses the pre-rewrite (inverted) StackLayout and needs
            // the swap. If the version cannot be determined, assume the modern (>= v1.4.0) layout.
            return ApplicationVersionHelper.GameVersion() is { } version &&
                   (version.Major < 1 || (version.Major == 1 && version.Minor < 4));
        }

        private static bool ComputeEnableFloatInputClamp()
        {
            // If the version cannot be determined, assume a game that has the attribute
            return ApplicationVersionHelper.GameVersion() is not { } version ||
                   version.Major > 1 || (version.Major == 1 && version.Minor >= 2);
        }

        protected static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(DefaultResourceInjector).Assembly.GetManifestResourceStream(embedPath);
            if (stream is null) throw new MCMUIEmbedResourceNotFoundException($"Could not find embed resource '{embedPath}'!");
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);

            if (SwapVerticalLayoutMethod && doc.DocumentElement is not null)
                SwapVerticalLayoutMethods(doc.DocumentElement);

            if (EnableFloatInputClamp && doc.DocumentElement is not null)
                SetFloatInputClamp(doc.DocumentElement);

            return doc;
        }

        private static void SwapVerticalLayoutMethods(XmlNode node)
        {
            if (node.Attributes is not null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (!attribute.Name.Equals("LayoutImp.LayoutMethod", StringComparison.Ordinal))
                        continue;

                    if (attribute.Value.Equals("VerticalTopToBottom", StringComparison.Ordinal))
                        attribute.Value = "VerticalBottomToTop";
                    else if (attribute.Value.Equals("VerticalBottomToTop", StringComparison.Ordinal))
                        attribute.Value = "VerticalTopToBottom";
                }
            }

            foreach (XmlNode child in node.ChildNodes)
                SwapVerticalLayoutMethods(child);
        }

        private static void SetFloatInputClamp(XmlNode node)
        {
            if (node is XmlElement { Name: "FloatInputTextWidget" } element && !element.HasAttribute("EnableClamp"))
                element.SetAttribute("EnableClamp", "true");

            foreach (XmlNode child in node.ChildNodes)
                SetFloatInputClamp(child);
        }

        public abstract void Inject();
    }
}
