using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Core.Entities
{
    public class ChatMessage : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;

        // Navigation
        public User Sender { get; set; } = null!;
        public User Receiver { get; set; } = null!;
        public Application Application { get; set; } = null!;
    }
}
