using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class GroupModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(100)]
        public string? GroupName { get; set; }

        [MaxLength(500)]
        public string? GroupDesc { get; set; }

        public byte[]? GroupMiniature { get; set; }

        public DateOnly GroupCreationDate { get; set; }

        [InverseProperty(nameof(GroupParticipant.Group))]
        public ICollection<GroupParticipant>? GroupParticipants_Group { get; set; }
    }
}
