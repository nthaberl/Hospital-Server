using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalClient.Models
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }

    }
}
