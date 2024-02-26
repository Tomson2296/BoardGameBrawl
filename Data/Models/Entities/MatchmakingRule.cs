using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class MatchmakingRule
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(100)]
        public string? RuleDescription { get; set; }

        public bool RuleDecider { get; set; }

        public RuleType RuleType { get; set; }

        [InverseProperty(nameof(BoardgameRule.MatchmakingRule))]
        public ICollection<BoardgameRule>? BoardgameRules_MatchmakingRule { get; set; }
    }

    public enum RuleType
    {
        Boolean,
        Int,
        String
    }
}