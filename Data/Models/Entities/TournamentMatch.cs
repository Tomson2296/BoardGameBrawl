using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities;

public class TournamentMatch
{
    [ForeignKey(nameof(Tournament))]
    public string? TournamentId { get; set; }

    public Tournament? Tournament { get; set; }

    [ForeignKey(nameof(Match))]
    public string? MatchId { get; set; }

    public MatchModel? Match { get; set; }
}
