using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public static class MatchStoreExtensions
    {
        public static async Task<MatchProgress> GetMatchProgressInfoAsync(this IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore,
            MatchModel matchModel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(matchModel);
            return await Task.FromResult(matchModel.MatchProgress);
        }

        public static async Task SetMatchProgressInfoAsync(this IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore,
            MatchModel matchModel, MatchProgress matchProgress, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(matchModel);
            matchModel.MatchProgress = matchProgress;
            await Task.CompletedTask;
        }
    }
}
