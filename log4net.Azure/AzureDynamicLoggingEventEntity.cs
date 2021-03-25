using System;
using System.Collections;
using log4net.Core;

namespace log4net.Appender
{
    internal sealed class AzureDynamicLoggingEventEntity : ElasticTableEntity
    {
        public AzureDynamicLoggingEventEntity(LoggingEvent e, PartitionKeyTypeEnum partitionKeyType)
        {
            this["Level"] = e.Level.ToString();
            this["Message"] = e.RenderedMessage;
            this["Timestamp"] = e.TimeStamp;
            
            foreach (DictionaryEntry entry in e.Properties)
            {
                var key = entry.Key.ToString()
                    .Replace(":", "_")
                    .Replace("@", "_")
                    .Replace(".", "_");
                this[key] = entry.Value;
            }

            Timestamp = e.TimeStamp;
            PartitionKey = e.MakePartitionKey(partitionKeyType);
            RowKey = e.MakeRowKey();
        }

        public AzureDynamicLoggingEventEntity(LoggingEvent e, PartitionKeyTypeEnum partitionKeyType, string message, int sequenceNumber) : this(e, partitionKeyType)
        {
            this["Message"] = message;
            this["SequenceNumber"] = sequenceNumber;
        }
    }
}