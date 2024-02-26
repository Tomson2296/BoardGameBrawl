using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    [AutoMap(typeof(MatchmakingRule))]
    public class MatchmakingRuleDTO
    {
        [SourceMember(nameof(MatchmakingRule.Id))]
        public string? RuleId { get; set; }

        [SourceMember(nameof(MatchmakingRule.RuleDescription))]
        public string? RuleDesc { get; set; }

        [SourceMember(nameof(MatchmakingRule.RuleDecider))]
        public bool RuleDecider { get; set; }

        [SourceMember(nameof(MatchmakingRule.RuleType))]
        public RuleType RuleType { get; set; }
    }
}