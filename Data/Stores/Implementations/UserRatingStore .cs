using AspNetCore;
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class UserRatingStore : IUserRatingStore<UserRating, ApplicationUser, BoardgameModel>
    {
        private readonly ApplicationDbContext _context;
        public UserRatingStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateRatingAsync(UserRating rating, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rating);

            _context.UserRatings.Add(rating);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create rating {rating.Id}." });
        }

        public async Task<IdentityResult> DeleteRatingAsync(UserRating rating, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rating);
            var ratingFromDB = await _context.UserRatings.FindAsync(rating.Id);

            if (ratingFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find rating to deletion process: {rating.Id}." });
            }
            else
            {
                _context.UserRatings.Remove(ratingFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete rating {rating.Id}." });
            }
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

        public async Task<UserRating?> FindRatingByIdAsync(string ratingId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(ratingId);
            return await _context.UserRatings.AsNoTracking().SingleOrDefaultAsync(r => r.Id!.Equals(ratingId), cancellationToken);
        }

        public async Task<UserRating?> FindRatingByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            return await _context.UserRatings.AsNoTracking().SingleOrDefaultAsync(r => r.UserId!.Equals(userId), cancellationToken);
        }

        public async Task<int> GetRatingAsync(UserRating rating, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(rating.Rating);
        }

        public async Task SetBoardgameAsync(UserRating rating, BoardgameModel boardgame, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rating);
            ArgumentNullException.ThrowIfNull(boardgame);
            rating.BoardgameId = boardgame.Id;
            await Task.CompletedTask;
        }

        public async Task SetRatingAsync(UserRating rating, int ratingValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rating);
            ArgumentNullException.ThrowIfNull(ratingValue);
            rating.Rating = ratingValue;
            await Task.CompletedTask;
        }

        public async Task SetUserAsync(UserRating rating, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rating);
            ArgumentNullException.ThrowIfNull(user);
            rating.UserId = user.Id;
            await Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateRatingAsync(UserRating rating, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(rating);
            _context.UserRatings.Update(rating);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update rating {rating.Id}." });
        }

        public async Task<bool> CheckIfUserHasRatings(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await _context.UserRatings.AnyAsync(r => r.UserId == user.Id);
        }

        public async Task<IEnumerable<UserRating?>> GetAllRatingsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);

            return await _context.UserRatings
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
