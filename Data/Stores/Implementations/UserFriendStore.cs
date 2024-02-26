#nullable disable

using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class UserFriendStore : IUserFriendStore<UserFriend, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration; 

        public UserFriendStore(ApplicationDbContext context, AutoMapper.IConfigurationProvider configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateFriendshipAsync(UserFriend friendship, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(friendship);
            ArgumentException.ThrowIfNullOrEmpty(friendship.UserId);
            ArgumentException.ThrowIfNullOrEmpty(friendship.FriendId);

            _context.UserFriends.Add(friendship);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create friendship {friendship.Id}." });
        }

        public async Task<IdentityResult> DeleteFriendshipAsync(UserFriend friendship, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(friendship);
            var friendshipFromDB = await _context.UserFriends.FindAsync(friendship.Id);

            if (friendshipFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find friendship to deletion process: {friendship.Id}." });
            }
            else
            {
                _context.UserFriends.Remove(friendshipFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete friendship {friendship.Id}." });
            }
        }

        public async Task<UserFriend> FindFriendshipByIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(Id);
            return await _context.UserFriends.AsNoTracking().SingleOrDefaultAsync(f => f.Id.Equals(Id), cancellationToken);
        }

        public async Task<IdentityResult> UpdateFriendshipAsync(UserFriend friendship, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(friendship);
            _context.UserFriends.Update(friendship);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update friendship {friendship.Id}." });
        }

        public async Task SetUserAsync(UserFriend friendship, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(friendship);
            ArgumentNullException.ThrowIfNull(user);
            friendship.UserId = user.Id;
            await Task.CompletedTask;
        }

        public async Task SetFriendAsync(UserFriend friendship, ApplicationUser friend, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(friendship);
            ArgumentNullException.ThrowIfNull(friend);
            friendship.FriendId = friend.Id;
            await Task.CompletedTask;
        }

        public async Task SetFriendshipAcceptedAsync(UserFriend friendship, bool IsAccepted, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(friendship);
            ArgumentNullException.ThrowIfNull(IsAccepted);
            friendship.isAccepted = IsAccepted;
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<UserFriend>> FindFriendshipsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            return await _context.UserFriends
                .AsNoTracking()
                .Where(uf => uf.UserId.Equals(userId) && uf.isAccepted == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicUserInfoDTO>> FindAllFriendsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            return await _context.UserFriends
                .AsNoTracking()
                .Where(uf => uf.UserId.Equals(userId) && uf.isAccepted == true)
                .Select(uf => uf.Friend)
                .ProjectTo<BasicUserInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserFriend>> FindFriendshipsByFriendIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            return await _context.UserFriends
                .AsNoTracking()
                .Where(uf => uf.FriendId.Equals(userId) && uf.isAccepted == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicUserInfoDTO>> FindAllFriendsByFriendIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            return await _context.UserFriends
                .AsNoTracking()
                .Where(uf => uf.FriendId.Equals(userId) && uf.isAccepted == true)
                .Select(uf => uf.User)
                .ProjectTo<BasicUserInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
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

        public async Task<IEnumerable<BasicUserInfoDTO>> GetUserFriendsAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            IEnumerable<BasicUserInfoDTO> friendshipsByUserId = await FindAllFriendsByUserIdAsync(userId, cancellationToken);
            IEnumerable<BasicUserInfoDTO> friendshipsByFriendId = await FindAllFriendsByFriendIdAsync(userId, cancellationToken);
            
            return friendshipsByUserId.Concat(friendshipsByFriendId).Distinct().ToList();
        }

        public async Task<UserFriend> FindFriendshipByUserFriendIdsAsync(string userId, string friendId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            ArgumentException.ThrowIfNullOrEmpty(friendId);
            return await _context.UserFriends
                .AsNoTracking()
                .Where(uf => uf.UserId == userId && uf.FriendId == friendId)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> CheckIfFriendshipExistsAsync(string userId, string friendId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            ArgumentException.ThrowIfNullOrEmpty(friendId);
            var friendship = await _context.UserFriends
                .AsNoTracking()
                .Where(uf => uf.UserId == userId && uf.FriendId == friendId)
                .SingleOrDefaultAsync(cancellationToken);

            if (friendship != default)
            {
                return true;
            }
            else
            {
                return false;
            }
        }       
    }
}