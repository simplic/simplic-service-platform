using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Class for building xaml rad document strings.
    /// </summary>
    public static class RadDocumentBuilder
    {
        /// <summary>
        /// Gets a run statement.
        /// </summary>
        /// <param name="foreground">Foreground color</param>
        /// <param name="text">Text to display</param>
        /// <returns>Run statement as string</returns>
        public static string GetSpanXaml(Color foreground, string text)
        {
            return $@"<t:Span FontSize=""12"" FontFamily=""Consolas"" ForeColor=""{foreground}"" Text=""{text}"" />";
            //return $@"<t:Span FontSize=""12"" FontFamily=""Consolas"" ForeColor=""{foreground}"" Text=""{XmlConvert.EncodeName(text)}"" />";
        }

        /// <summary>
        /// Gets a paragraph statement.
        /// </summary>
        /// <param name="content">Content of the paragraph</param>
        /// <returns>Paragraph statement as string</returns>
        public static string GetParagraphXaml(string content)
        {
            return $@"<t:Paragraph>{content}</t:Paragraph>";
            //return $@"<t:Paragraph>{XmlConvert.EncodeName(content)}</t:Paragraph>";
        }

        /// <inheritdoc cref="GetParagraphXaml"/>
        public static string GetParagraphXaml(IList<string> contents)
        {
            var contentBuilder = new StringBuilder();
            foreach (var item in contents)
                contentBuilder.Append(item);
            //contentBuilder.Append(XmlConvert.EncodeName(item));
            return $@"<t:Paragraph>{contentBuilder}</t:Paragraph>";
        }

        /// <summary>
        /// Gets a rad document as string.
        /// </summary>
        /// <param name="paragraphs"></param>
        /// <returns>Rad document as string</returns>
        public static string GetDocumentXaml(IList<string> paragraphs)
        {
            var paragraphsBuilder = new StringBuilder();

            foreach (var paragraph in paragraphs)
            {
                paragraphsBuilder.Append(paragraph);
                //paragraphsBuilder.Append(XmlConvert.EncodeName(paragraph));
            }

            var documentString = $@"<t:RadDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                                                   xmlns:t=""clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents"" 
                                                   LayoutMode=""FlowNoWrap"" LineSpacing=""13"" LineSpacingType=""Exact""
                                                   ParagraphDefaultSpacingAfter=""1"" ParagraphDefaultSpacingBefore=""1"" StyleName=""defaultDocumentStyle"">
                                      <t:RadDocument.Captions>
                                        <t:CaptionDefinition IsDefault=""True"" IsLinkedToHeading=""False"" Label=""Figure"" LinkedHeadingLevel=""0"" NumberingFormat=""Arabic"" SeparatorType=""Hyphen"" />
                                        <t:CaptionDefinition IsDefault=""True"" IsLinkedToHeading=""False"" Label=""Table"" LinkedHeadingLevel=""0"" NumberingFormat=""Arabic"" SeparatorType=""Hyphen"" />
                                      </t:RadDocument.Captions>
                                      <t:RadDocument.ProtectionSettings>
                                        <t:DocumentProtectionSettings EnableDocumentProtection=""False"" Enforce=""False"" HashingAlgorithm=""None"" HashingSpinCount=""0"" ProtectionMode=""ReadOnly"" />
                                      </t:RadDocument.ProtectionSettings>
                                      <t:Section>
                                        {paragraphsBuilder}
                                      </t:Section>
                                    </t:RadDocument>";
            return documentString;
        }

        /// <summary>
        /// Gets a rad document from an xaml.
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns>RadDocument</returns>
        public static RadDocument GetDocumentFromXaml(string xaml)
        {
            var stringReader = new StringReader(xaml);
            var xmlReader = XmlReader.Create(stringReader);
            return (RadDocument)XamlReader.Load(xmlReader);
        }

        /// <summary>
        /// Gets a span with default font size and font family.
        /// </summary>
        /// <param name="foreground"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Span GetSpan(Color foreground, string text)
        {
            var span = new Span();
            span.ForeColor = foreground;
            span.Text = text;
            span.FontSize = 12;
            span.FontFamily = new FontFamily("Consolas");
            return span;
        }

        /// <summary>
        /// Gets a paragraph containing given inlines.
        /// </summary>
        /// <param name="inlines"></param>
        /// <returns></returns>
        public static Paragraph GetParagraph(IEnumerable<Inline> inlines)
        {
            var paragraph = new Paragraph();
            foreach (var inline in inlines)
                paragraph.Inlines.Add(inline);
            return paragraph;
        }
    }
}
