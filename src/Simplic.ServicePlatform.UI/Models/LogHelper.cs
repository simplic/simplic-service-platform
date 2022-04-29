using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using Microsoft.Extensions.Logging;
using Simplic.Log;
using Telerik.Windows.Documents.Model;

namespace Simplic.ServicePlatform.UI.Models
{
    public static class LogHelper
    {
        public static readonly Dictionary<LogLevel, Color> LogLevelColors = new()
        {
            { LogLevel.Trace, Colors.Cyan },
            { LogLevel.Debug, Colors.Magenta },
            { LogLevel.Information, Colors.Green },
            { LogLevel.Warning, Colors.Yellow },
            { LogLevel.Error, Colors.Red },
            { LogLevel.Critical, Colors.DarkRed },
        };

        public static readonly Color TimeColor = Colors.LightGray;
        public static readonly Color MessageColor = Colors.LightGray;

        /// <summary>
        /// Parses a collection of log messages to appropriate xaml format.
        /// </summary>
        /// <param name="logMessages"></param>
        /// <returns>Xaml string</returns>
        public static string ToXaml(IEnumerable<ServiceLogMessage> logMessages)
        {
            if (logMessages == null || !logMessages.Any()) return string.Empty;

            var paragraphs = new List<string>();
            foreach (var log in logMessages)
            {
                var statements = new List<string>
                {
                    RadDocumentBuilder.GetSpanXaml(TimeColor, $"{log.Time.ToString(CultureInfo.CurrentCulture)}\t"),
                    RadDocumentBuilder.GetSpanXaml(LogLevelColors[log.LogLevel], StringUtils.FormatStringSize(log.LogLevel.ToString(), 15)),
                    RadDocumentBuilder.GetSpanXaml(MessageColor, log.Message)
                };
                paragraphs.Add(RadDocumentBuilder.GetParagraphXaml(statements));
            }
            return RadDocumentBuilder.GetDocumentXaml(paragraphs);
        }

        /// <summary>
        /// Parses a service log message to a paragraph.
        /// </summary>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        public static Paragraph ToParagraph(ServiceLogMessage logMessage)
        {
            var logLine = new List<Inline>
            {
                RadDocumentBuilder.GetSpan(TimeColor, logMessage.Time.ToString(CultureInfo.CurrentCulture) + ' '),
                RadDocumentBuilder.GetSpan(LogLevelColors[logMessage.LogLevel], StringUtils.FormatStringSize(logMessage.LogLevel.ToString(), 12) + ' '),
                RadDocumentBuilder.GetSpan(MessageColor, logMessage.Message)
            };
            return RadDocumentBuilder.GetParagraph(logLine);
        }
    }
}
