using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class MessageModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Sender))]
        public string? SenderId { get; set; }

        [ForeignKey(nameof(Receiver))]
        public string? ReceiverId { get; set; }

        [MaxLength(250)]
        public string? MessageTopic { get; set; }

        [MaxLength(500)]
        public string? MessageBody { get; set; }

        public DateTime MessageSentTime { get; set; }

        public bool IsMessageRead { get; set; } = false;

        public ApplicationUser? Sender { get; set; }

        public ApplicationUser? Receiver { get; set; }
    }
}
