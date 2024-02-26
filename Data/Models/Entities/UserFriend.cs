using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class UserFriend
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        [ForeignKey(nameof(Friend))]
        public string? FriendId { get; set; }

        public ApplicationUser? User { get; set; }

        public ApplicationUser? Friend { get; set; }

        public bool? isAccepted { get; set; } = false;
    }
}
