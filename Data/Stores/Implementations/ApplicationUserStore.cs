using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class ApplicationUserStore : IApplicationUserStore
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomNormalizer _normalizer;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;
        private readonly IMessageStore<MessageModel, ApplicationUser> _messageStore;
        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;

        public ApplicationUserStore(ApplicationDbContext context,
            CustomNormalizer normalizer,
            IUserFriendStore<UserFriend, ApplicationUser> userFriendStore,
            IMessageStore<MessageModel, ApplicationUser> messageStore,
            IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore)
        {
            _context = context;
            _normalizer = normalizer;
            _userFriendStore = userFriendStore;
            _messageStore = messageStore;
            _userScheduleStore = userScheduleStore;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(user.Id);

            if (!_context.Users.Contains(user))
            {
                _context.Users.Add(user);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not create user {user.Id}." });
            }
            else 
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"User {user.Id} already exists in database - creation process failed."});
            }
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            var userFromDB = await _context.Users.FindAsync(user.Id);

            if (userFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find user to deletion process: {user.Id}." });
            }
            else
            {
                // delete all friendships related to user (from user and friend side of relationship)
                var user_friendships = await _userFriendStore.FindFriendshipsByUserIdAsync(user.Id, cancellationToken);
                foreach (var friendship in user_friendships)
                {
                    await _userFriendStore.DeleteFriendshipAsync(friendship, cancellationToken);
                }

                var friend_friendships = await _userFriendStore.FindFriendshipsByFriendIdAsync(user.Id, cancellationToken);
                foreach (var friendship in friend_friendships)
                {
                    await _userFriendStore.DeleteFriendshipAsync(friendship, cancellationToken);
                }

                // delete all sender messages related to user (from sender and also from receiver side of relationship)
                var sender_messages = await _messageStore.GetMessagesBySenderIdAsync(user.Id, cancellationToken);
                foreach (var message in sender_messages)
                {
                    await _messageStore.DeleteMessageAsync(message, cancellationToken);
                }

                var receiver_messages = await _messageStore.GetMessagesByReceiverIdAsync(user.Id, cancellationToken);
                foreach (var message in receiver_messages)
                {
                    await _messageStore.DeleteMessageAsync(message, cancellationToken);
                }

                // delete schedule from database related to removed user
                var userSchedule = await _userScheduleStore.FindScheduleByUserIdAsync(user.Id, cancellationToken);  
                await _userScheduleStore.DeleteScheduleAsync(userSchedule, cancellationToken);

                _context.Users.Remove(userFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete boardgame {user.Id}." });
            }
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(g => g.Id.Equals(userId), cancellationToken);
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(normalizedUserName, nameof(normalizedUserName));
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(g => g.NormalizedUserName!.Equals(normalizedUserName), cancellationToken);
        }

        public async Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.NormalizedUserName);
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.Id);
        }

        public async Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.UserName);
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(normalizedName, nameof(normalizedName));
            user.NormalizedUserName = _normalizer.NormalizeName(normalizedName);
            await Task.CompletedTask;
        }

        public async Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(userName, nameof(userName));
            user.UserName = userName;
            await Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            _context.Users.Update(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update user {user.Id}." });
        }

        public async Task<string?> GetBGGUsernameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.BGGUsername);
        }

        public async Task SetBGGUsernameAsync(ApplicationUser user, string? BGGUsername, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(BGGUsername, nameof(BGGUsername));
            user.BGGUsername = BGGUsername;
            await Task.CompletedTask;
        }

        public async Task<string?> GetUserDescriptionAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.UserDescription);
        }
        
        public async Task SetUserDescriptionAsync(ApplicationUser user, string? userDescription, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(userDescription, nameof(userDescription));
            user.UserDescription = userDescription;
            await Task.CompletedTask;
        }

        public async Task<DateOnly?> GetUserDateTimeCreationAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.UserCreatedTime);
        }

        public async Task SetUserDateTimeCreationAsync(ApplicationUser user, DateOnly creationDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(creationDate, nameof(creationDate));
            user.UserCreatedTime = creationDate;
            await Task.CompletedTask;
        }

        public async Task<DateOnly?> GetUserLastLoginDateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.UserLastLogin);
        }

        public async Task SetUserLastLoginDateAsync(ApplicationUser user, DateOnly userLastLogin, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(userLastLogin, nameof(userLastLogin));
            user.UserLastLogin = userLastLogin;
            await Task.CompletedTask;
        }

        public async Task<byte[]?> GetUserAvatarAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.UserAvatar);
        }

        public async Task SetUserAvatarAsync(ApplicationUser user, byte[]? userImage, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(userImage, nameof(userImage));
            user.UserAvatar = userImage;
            await Task.CompletedTask;
        }
        
        public async Task<string?> GetUserFirstNameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.FirstName);
        }

        public async Task SetUserFirstNameAsync(ApplicationUser user, string? firstName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(firstName, nameof(firstName));
            user.FirstName = firstName;
            await Task.CompletedTask;
        }

        public async Task<string?> GetUserLastNameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return await Task.FromResult(user.FirstName);
        }

        public async Task SetUserLastNameAsync(ApplicationUser user, string? lastName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(lastName);
            user.LastName = lastName;
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
    }
}