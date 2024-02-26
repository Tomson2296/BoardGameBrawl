#nullable disable
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardruleBrawl.Data.Stores.Implementations
{
    public class MatchmakingRuleStore : IMatchmakingRuleStore<MatchmakingRule>
    {
        private readonly ApplicationDbContext _context;
        public MatchmakingRuleStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateRuleAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            _context.MatchmakingRules.Add(rule);

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create matchmakingRule {rule.Id}." });
        }

        public async Task<IdentityResult> DeleteRuleAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            var ruleFromDB = await _context.MatchmakingRules.FindAsync(rule.Id);

            if (ruleFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find matchmakingRule to deletion process: {rule.Id}." });
            }
            else
            {
                _context.MatchmakingRules.Remove(ruleFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete matchmakingRule {rule.Id}." });
            }
        }

        public async Task<MatchmakingRule> FindRuleByIdAsync(string ruleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(ruleId);
            return await _context.MatchmakingRules.SingleOrDefaultAsync(gp => gp.Id.Equals(ruleId), cancellationToken);
        }

        public async Task<bool> GetRuleDeciderAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            return await Task.FromResult(rule.RuleDecider);
        }

        public async Task<string> GetRuleDescriptionAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            return await Task.FromResult(rule.RuleDescription);
        }

        public async Task<string> GetRuleIdAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            return await Task.FromResult(rule.Id);
        }

        public async Task<RuleType> GetRuleTypeAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            return await Task.FromResult(rule.RuleType);
        }

        public async Task SetRuleDeciderAsync(MatchmakingRule rule, bool isRuleDecider, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentNullException.ThrowIfNull(isRuleDecider);
            rule.RuleDecider = isRuleDecider;
            await Task.CompletedTask;
        }

        public async Task SetRuleDescriptionAsync(MatchmakingRule rule, string ruleDescription, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentException.ThrowIfNullOrEmpty(ruleDescription);
            rule.RuleDescription = ruleDescription;
            await Task.CompletedTask;
        }

        public async Task SetRuleTypeAsync(MatchmakingRule rule, RuleType ruleType, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentNullException.ThrowIfNull(ruleType);
            rule.RuleType = ruleType;
            await Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateRuleAsync(MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rule);
            _context.MatchmakingRules.Update(rule);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update matchmakingRule {rule.Id}." });
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}