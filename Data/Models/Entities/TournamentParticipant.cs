using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BoardGameBrawl.Data.Models.Entities;

public class TournamentParticipant
{
    [ForeignKey(nameof(Tournament))]
    public string? TournamentId { get; set; }

    public Tournament? Tournament { get; set; }

    [ForeignKey(nameof(Participant))]
    public string? ParticipantId { get; set; }

    public ApplicationUser? Participant { get; set; }
}
