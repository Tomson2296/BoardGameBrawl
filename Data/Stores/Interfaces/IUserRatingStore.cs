using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IUserRatingStore<TRating, TUser, TGame> : IDisposable where TRating : class where TUser : class where TGame : class 
    {
        Task<IdentityResult> CreateRatingAsync(TRating rating, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteRatingAsync(TRating rating, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateRatingAsync(TRating rating, CancellationToken cancellationToken = default);


        Task<TRating?> FindRatingByIdAsync(string ratingId, CancellationToken cancellationToken = default);

        Task<TRating?> FindRatingByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<TRating?>> GetAllRatingsByUserIdAsync(string userId, CancellationToken cancellationToken = default);


        Task SetUserAsync(TRating rating, TUser user, CancellationToken cancellationToken = default);

        Task SetBoardgameAsync(TRating rating, TGame boardgame, CancellationToken cancellationToken = default);

        Task SetRatingAsync(TRating rating, int ratingValue, CancellationToken cancellationToken = default);

        Task<int> GetRatingAsync(TRating rating, CancellationToken cancellationToken = default);

        Task<bool> CheckIfUserHasRatings(TUser user, CancellationToken cancellationToken = default);
    }
}
