#nullable disable
using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class BoardgameRuleStore : IBoardgameRuleStore<BoardgameRule, BoardgameModel, MatchmakingRule>
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration;

        public BoardgameRuleStore(ApplicationDbContext context, AutoMapper.IConfigurationProvider configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateBoardgameRuleAsync(BoardgameRule boardgameRule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(boardgameRule);
            ArgumentException.ThrowIfNullOrEmpty(boardgameRule.BoardgameId);
            ArgumentException.ThrowIfNullOrEmpty(boardgameRule.MatchmakingRuleId);
            
            _context.BoardgameRules.Add(boardgameRule);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create instance of BoardgameRule entity." });
        }

        public async Task<IdentityResult> DeleteBoardgameRuleAsync(BoardgameModel boardgame, MatchmakingRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(boardgame);
            ArgumentNullException.ThrowIfNull(rule);
            var boadrdgameRuleFromDB = await FindBoardgameRuleByIdAsync(boardgame.Id, rule.Id);

            if (boadrdgameRuleFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find boardgameRule to deletion process." });
            }
            else
            {
                _context.BoardgameRules.Remove(boadrdgameRuleFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete boardgameRule wntity." });
            }
        }

        public async Task<BoardgameRule> FindBoardgameRuleByIdAsync(string boardgameId, string roleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(boardgameId);
            ArgumentException.ThrowIfNullOrEmpty(roleId);
            return await _context.BoardgameRules.AsNoTracking().SingleOrDefaultAsync(g => g.BoardgameId.Equals(boardgameId) && g.MatchmakingRuleId.Equals(roleId), cancellationToken);
        }

        public async Task<IEnumerable<MatchmakingRuleDTO>> GetBoardgameMatchmakingRulesListAsync(string boardgameId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.BoardgameRules
                .AsNoTracking()
                .Where(br => br.BoardgameId == boardgameId)
                .Select(br => br.MatchmakingRule)
                .ProjectTo<MatchmakingRuleDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task SetBoardgameIdAsync(BoardgameRule boardgameRule, BoardgameModel boardgame, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(boardgameRule);
            ArgumentNullException.ThrowIfNull(boardgame);
            boardgameRule.BoardgameId = boardgame.Id;
            await Task.CompletedTask;
        }

        public async Task SetMatchmakingRuleIdAsync(BoardgameRule boardgameRule, MatchmakingRule matchmakingRule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(boardgameRule);
            ArgumentNullException.ThrowIfNull(matchmakingRule);
            boardgameRule.MatchmakingRuleId = matchmakingRule.Id;
            await Task.CompletedTask;
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

        public async Task<bool> CheckIfBGHasAnyMatchmakingRulesAsync(string boardgameId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(boardgameId);
            var anyMatchmakingRules = await _context.BoardgameRules.AsNoTracking().FirstOrDefaultAsync(bg => bg.BoardgameId == boardgameId, cancellationToken);

            if (anyMatchmakingRules != default)
                return true;
            else
                return false;
        }

        public async Task<BoardgameModel> ReturnBoardgameByMatchmakingIdAsync(string ruleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(ruleId);
            
            return await _context.BoardgameRules
                .AsNoTracking()
                .Where(bg => bg.MatchmakingRuleId == ruleId)
                .Select(bg => bg.Boardgame)
                .SingleAsync();
        }
    }
}