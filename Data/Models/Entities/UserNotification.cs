using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class UserNotification
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Receiver))]
        public string? ReceiverId { get; set; }

        public ApplicationUser? Receiver { get; set; }

        [MaxLength(250)]
        public string? Notification { get; set; }

        public bool IsShown { get; set; } = false; 

        public NotificationType NotificationType { get; set; } 
    }
    public enum NotificationType
    {
        Response,
        AddToFriends,
        AddToGroup,
        AddToModeratorRole,
        AddToBoardGameModeration,
        AddToMatch
    }
}
