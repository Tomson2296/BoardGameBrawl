using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities;

public class GroupParticipant
{
    [Key]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [ForeignKey(nameof(Group))]
    public string? GroupId { get; set; }

    [ForeignKey(nameof(Participant))]
    public string? ParticipantId { get; set; }

    public GroupModel? Group { get; set; }

    public ApplicationUser? Participant { get; set; }

    public bool IsOwner { get; set; }
}