using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IMatchmakingRuleStore<IRule> : IDisposable where IRule : class
    {
        Task<IdentityResult> CreateRuleAsync(IRule rule, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateRuleAsync(IRule rule, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteRuleAsync(IRule rule, CancellationToken cancellationToken = default);


        Task<IRule> FindRuleByIdAsync(string ruleId, CancellationToken cancellationToken = default);


        Task<string?> GetRuleIdAsync(IRule rule, CancellationToken cancellationToken = default);

        Task<string?> GetRuleDescriptionAsync(IRule rule, CancellationToken cancellationToken = default);

        Task SetRuleDescriptionAsync(IRule rule, string ruleDescription, CancellationToken cancellationToken = default);

        Task<bool> GetRuleDeciderAsync(IRule rule, CancellationToken cancellationToken = default);

        Task SetRuleDeciderAsync(IRule rule, bool isRuleDecider, CancellationToken cancellationToken = default);

        Task<RuleType> GetRuleTypeAsync(IRule rule, CancellationToken cancellationToken = default);

        Task SetRuleTypeAsync(IRule rule, RuleType ruleType, CancellationToken cancellationToken = default);
    }
}
