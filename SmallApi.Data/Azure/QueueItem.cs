using System;
using System.ComponentModel;

namespace SmallApi.Data.Azure
{
    public class QueueItem 
    {
        public string Id { get; set; }

        [DisplayName("Raw Data")]
        public string QueueMessageStored { get; set; }

        [DisplayName("Submitted Date")]
        public DateTime SubmittedDate { get; set; }

        public string PopReceipt { get; set; }
    }
}
