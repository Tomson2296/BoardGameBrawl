using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data
{
    public static class UserStoreExtensions
    {
        /// <summary>
        /// Get a BGGUsername of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string?> GetBGGUsernameAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.BGGUsername);
        }

        /// <summary>
        /// Set a BGGUsername of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="BGGUsername"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetBGGUsernameAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, string? BGGUsername, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(BGGUsername);
            user.BGGUsername = BGGUsername;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get an UserDscrition of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string?> GetUserDescriptionAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.UserDescription);
        }

        /// <summary>
        /// Set a UserDescription of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="userDescription"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserDescriptionAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, string? userDescription, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(userDescription);
            user.UserDescription = userDescription;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get an User CreationTime of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<DateOnly?> GetUserDateTimeCreationAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.UserCreatedTime);
        }

        /// <summary>
        /// Set an User CreationTime of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="creationDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserDateTimeCreationAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, 
            DateOnly creationDate, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(creationDate);
            user.UserCreatedTime = creationDate;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get an Last login date of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<DateOnly?> GetUserLastLoginDateAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.UserLastLogin);
        }

        /// <summary>
        /// Set a Last login date of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="userLastLogin"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserLastLoginDateAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user,
            DateOnly userLastLogin, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(userLastLogin);
            user.UserLastLogin = userLastLogin;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get an UserAvatar image of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<byte[]?> GetUserAvatarAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.UserAvatar);
        }

        /// <summary>
        /// Set an UserAvatar image of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="userImage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserAvatarAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, byte[]? userImage, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(userImage);
            user.UserAvatar = userImage;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get a first name of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string?> GetUserFirstNameAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.FirstName);
        }

        /// <summary>
        /// Set a first name of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="firstName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserFirstNameAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, string? firstName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(firstName);
            user.FirstName = firstName;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get a last name of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string?> GetUserLastNameAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.FirstName);
        }

        /// <summary>
        /// Set a last name of the specified user.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="lastName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserLastNameAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, string? lastName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(lastName);
            user.LastName = lastName;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets list of user's favourite boardgames.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<List<string>?> GetUserFavouriteBoardGamesAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.User_FavouriteBoardgames);
        }

        /// <summary>
        /// Sets list of user's favourite boardgames.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="user"></param>
        /// <param name="boardgameList"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task SetUserFavouriteBoardGamesAsync(this IUserStore<ApplicationUser> userStore, ApplicationUser user, List<string>? boardgameList, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(boardgameList);
            user.User_FavouriteBoardgames = boardgameList;
            await Task.CompletedTask;
        }
    }
}