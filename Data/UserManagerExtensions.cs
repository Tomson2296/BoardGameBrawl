#nullable disable

using AutoMapper;
using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByBGGUserNameAsync(this UserManager<ApplicationUser> userManager, string BGGUsername)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(BGGUsername));
            return await userManager.Users.SingleOrDefaultAsync(x => x.BGGUsername == BGGUsername);
        }

        public static async Task<string> GetBGGUserNameAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            return await Task.FromResult(user.BGGUsername);
        }
        public static async Task<IdentityResult> SetBGGUserNameAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string BGGUsername)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(BGGUsername));
            user.BGGUsername = BGGUsername;
            IdentityResult result = await userManager.UpdateAsync(user);
            return result;
        }

        public static async Task<string> GetUserFirstNameAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            return await Task.FromResult(user.FirstName);
        }

        public static async Task<IdentityResult> SetUserFirstNameAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string firstName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(firstName));
            user.FirstName = firstName;
            IdentityResult result = await userManager.UpdateAsync(user);
            return result;
        }

        public static async Task<string> GetUserLastNameAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            return await Task.FromResult(user.LastName);
        }

        public static async Task<IdentityResult> SetUserLastNameAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string lastName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(lastName));
            user.LastName = lastName;
            IdentityResult result = await userManager.UpdateAsync(user);
            return result;
        }

        public static async Task<string> GetUserDescriptionAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            return await Task.FromResult(user.UserDescription);
        }

        public static async Task<IdentityResult> SetUserDescriptionAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string userDescription)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(userDescription));
            user.UserDescription = userDescription;
            IdentityResult result = await userManager.UpdateAsync(user);
            return result;
        }

        public static async Task<byte[]> GetUserAvatarAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            return await Task.FromResult(user.UserAvatar);
        }

        public static async Task<IdentityResult> SetUserAvatarAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, byte[] userAvatar)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(userAvatar));
            user.UserAvatar = userAvatar;
            IdentityResult result = await userManager.UpdateAsync(user);
            return result;
        }

        public static async Task<List<string>> GetUserFavouriteBoardGamesAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            return await Task.FromResult(user.User_FavouriteBoardgames);
        }

        public static async Task<IdentityResult> SetUserFavouriteBoardGamesAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, List<string> boardgameList)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(boardgameList);
            user.User_FavouriteBoardgames = boardgameList;
            IdentityResult result = await userManager.UpdateAsync(user);
            return result;
        }

        public static async Task<IEnumerable<ApplicationUser>> GetBatchOfUsersWithTheSameRole(this UserManager<ApplicationUser> userManager, int batchSize, int batchNumber, string roleName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(batchSize));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(batchNumber));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(roleName));

            IEnumerable<ApplicationUser> users = await userManager.GetUsersInRoleAsync(roleName);
            IEnumerable<ApplicationUser> userBatch = users.Skip(batchSize * (batchNumber - 1)).Take(batchSize);
            return await Task.FromResult(userBatch);
        }
    }
}