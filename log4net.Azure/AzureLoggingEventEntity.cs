using System;
using System.Collections;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using log4net.Core;

namespace log4net.Appender
{
    internal sealed class AzureLoggingEventEntity : TableEntity
    {
        public AzureLoggingEventEntity(LoggingEvent e, PartitionKeyTypeEnum partitionKeyType)
        {
            Level = e.Level.ToString();
            var sb = new StringBuilder(e.Properties.Count);
            foreach (DictionaryEntry entry in e.Properties)
            {
                sb.AppendFormat("{0}:{1}", entry.Key, entry.Value);
                sb.AppendLine();
            }
            Message = e.RenderedMessage + Environment.NewLine + e.GetExceptionString();
            Timestamp = e.TimeStamp;

            PartitionKey = e.MakePartitionKey(partitionKeyType);
            RowKey = e.MakeRowKey();
        }

        public AzureLoggingEventEntity(LoggingEvent e, PartitionKeyTypeEnum partitionKeyType, string message, int sequenceNumber) : this(e, partitionKeyType)
        {
            Message = message;
        }

#pragma warning disable CS0108 // Un membre masque un membre hérité ; le mot clé new est manquant
        public DateTime Timestamp { get; set; }
#pragma warning restore CS0108 // Un membre masque un membre hérité ; le mot clé new est manquant

        public string Message { get; set; }

        public string Level { get; set; }

    }
}
