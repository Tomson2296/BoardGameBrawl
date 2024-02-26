using BoardGameBrawl.Data.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IBoardgameRuleStore<TBR, TGame, TRule> : IDisposable where TBR : class where TGame : class where TRule : class
    {
        Task<IdentityResult> CreateBoardgameRuleAsync(TBR boardgameRule, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteBoardgameRuleAsync(TGame boardgame, TRule matchmakingRule, CancellationToken cancellationToken = default);

        Task<TBR> FindBoardgameRuleByIdAsync(string boardgameId, string ruleId, CancellationToken cancellationToken = default);

        Task SetBoardgameIdAsync(TBR boardgameRule, TGame boardgame, CancellationToken cancellationToken = default);

        Task SetMatchmakingRuleIdAsync(TBR boardgameRule, TRule matchmakingRule, CancellationToken cancellationToken = default);

        Task<IEnumerable<MatchmakingRuleDTO>> GetBoardgameMatchmakingRulesListAsync(string boardgameId, CancellationToken cancellationToken = default);

        Task<bool> CheckIfBGHasAnyMatchmakingRulesAsync(string boardgameId, CancellationToken cancellationToken = default);

        Task<TGame> ReturnBoardgameByMatchmakingIdAsync(string ruleId, CancellationToken cancellationToken = default);
    }
}
