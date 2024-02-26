using System.ComponentModel.DataAnnotations.Schema;
namespace BoardGameBrawl.Data.Models.Entities;

public class BoardgameRule
{
    [ForeignKey(nameof(Boardgame))]
    public string? BoardgameId { get; set; }

    [ForeignKey(nameof(MatchmakingRule))]
    public string? MatchmakingRuleId { get; set; }

    public BoardgameModel? Boardgame { get; set; }

    public MatchmakingRule? MatchmakingRule { get; set; }
}
