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

        private static bool ComputeSwapVerticalLayoutMethod()
        {
            // Anything older than v1.4.0 uses the pre-rewrite (inverted) StackLayout and needs
            // the swap. If the version cannot be determined, assume the modern (>= v1.4.0) layout.
            return ApplicationVersionHelper.GameVersion() is { } version &&
                   (version.Major < 1 || (version.Major == 1 && version.Minor < 4));
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

        public abstract void Inject();
    }
}
