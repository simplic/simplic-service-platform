using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml;

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
        public static string GetSpan(Color foreground, string text)
        {
            return $@"<t:Span ForeColor=""{foreground}"" Text=""{XmlConvert.EncodeName(text)}"" />";
        }

        /// <summary>
        /// Gets a paragraph statement.
        /// </summary>
        /// <param name="content">Content of the paragraph</param>
        /// <returns>Paragraph statement as string</returns>
        public static string GetParagraph(string content)
        {
            return $@"<t:Paragraph>{XmlConvert.EncodeName(content)}</t:Paragraph>";
        }

        /// <inheritdoc cref="GetParagraph(string)"/>
        public static string GetParagraph(IList<string> contents)
        {
            var contentBuilder = new StringBuilder();
            foreach (var item in contents)
                contentBuilder.Append(XmlConvert.EncodeName(item));
            return $@"<t:Paragraph>{contentBuilder}</t:Paragraph>";
        }

        /// <summary>
        /// Gets a rad document.
        /// </summary>
        /// <param name="paragraphs"></param>
        /// <returns>Rad document as string</returns>
        public static string GetDocument(IList<string> paragraphs)
        {
            var paragraphsBuilder = new StringBuilder();

            foreach (var paragraph in paragraphs)
            {
                paragraphsBuilder.Append(XmlConvert.EncodeName(paragraph));
            }

            var documentString = $@"<t:RadDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:t=""clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents"" xmlns:s=""clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents"" xmlns:r=""clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents"" xmlns:n=""clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents"" xmlns:th=""clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents"" xmlns:sdt=""clr-namespace:Telerik.Windows.Documents.Model.StructuredDocumentTags;assembly=Telerik.Windows.Documents"" LayoutMode=""Flow"" LineSpacing=""1.15"" LineSpacingType=""Auto"" ParagraphDefaultSpacingAfter=""12"" ParagraphDefaultSpacingBefore=""0"" StyleName=""defaultDocumentStyle"">
                                      <t:RadDocument.Captions>
                                        <t:CaptionDefinition IsDefault=""True"" IsLinkedToHeading=""False"" Label=""Figure"" LinkedHeadingLevel=""0"" NumberingFormat=""Arabic"" SeparatorType=""Hyphen"" />
                                        <t:CaptionDefinition IsDefault=""True"" IsLinkedToHeading=""False"" Label=""Table"" LinkedHeadingLevel=""0"" NumberingFormat=""Arabic"" SeparatorType=""Hyphen"" />
                                      </t:RadDocument.Captions>
                                      <t:RadDocument.ProtectionSettings>
                                        <t:DocumentProtectionSettings EnableDocumentProtection=""False"" Enforce=""False"" HashingAlgorithm=""None"" HashingSpinCount=""0"" ProtectionMode=""ReadOnly"" />
                                      </t:RadDocument.ProtectionSettings>
                                      <t:RadDocument.Styles>
                                        <s:StyleDefinition DisplayName=""defaultDocumentStyle"" IsCustom=""False"" IsDefault=""False"" IsPrimary=""True"" Name=""defaultDocumentStyle"" Type=""Default"">
                                          <s:StyleDefinition.ParagraphStyle>
                                            <s:ParagraphProperties LineSpacing=""1.15"" SpacingAfter=""12"" />
                                          </s:StyleDefinition.ParagraphStyle>
                                          <s:StyleDefinition.SpanStyle>
                                            <s:SpanProperties FontFamily=""Verdana"" FontSize=""16"" FontStyle=""Normal"" FontWeight=""Normal"" />
                                          </s:StyleDefinition.SpanStyle>
                                        </s:StyleDefinition>
                                        <s:StyleDefinition DisplayName=""Normal"" IsCustom=""False"" IsDefault=""True"" IsPrimary=""True"" Name=""Normal"" Type=""Paragraph"" UIPriority=""0"" />
                                        <s:StyleDefinition DisplayName=""Table Normal"" IsCustom=""False"" IsDefault=""True"" IsPrimary=""False"" Name=""TableNormal"" Type=""Table"" UIPriority=""59"">
                                          <s:StyleDefinition.TableStyle>
                                            <s:TableProperties CellPadding=""5,0,5,0"">
                                              <s:TableProperties.TableLook>
                                                <t:TableLook />
                                              </s:TableProperties.TableLook>
                                            </s:TableProperties>
                                          </s:StyleDefinition.TableStyle>
                                        </s:StyleDefinition>
                                      </t:RadDocument.Styles>
                                      <t:Section>
                                        {paragraphsBuilder}
                                      </t:Section>
                                    </t:RadDocument>";
            return documentString;
        }
    }
}
