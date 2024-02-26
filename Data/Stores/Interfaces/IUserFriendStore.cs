using BoardGameBrawl.Data.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IUserFriendStore<TFriendship, TUser> : IDisposable where TFriendship : class where TUser : class
    {
        Task<IdentityResult> CreateFriendshipAsync(TFriendship friendship, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateFriendshipAsync(TFriendship friendship, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteFriendshipAsync(TFriendship friendship, CancellationToken cancellationToken = default);


        Task<TFriendship> FindFriendshipByIdAsync(string Id, CancellationToken cancellationToken = default);

        Task<TFriendship> FindFriendshipByUserFriendIdsAsync(string userId, string friendId, CancellationToken cancellationToken = default);

        Task<bool> CheckIfFriendshipExistsAsync(string userId, string friendId, CancellationToken cancellationToken = default);


        Task SetUserAsync(TFriendship friendship, TUser user, CancellationToken cancellationToken = default);

        Task SetFriendAsync(TFriendship friendship, TUser friend, CancellationToken cancellationToken = default);

        Task SetFriendshipAcceptedAsync(TFriendship friendship, bool IsAccepted, CancellationToken cancellationToken = default);


        Task<IEnumerable<TFriendship>> FindFriendshipsByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<TFriendship>> FindFriendshipsByFriendIdAsync(string userId, CancellationToken cancellationToken = default);

        //
        // DTO methods
        //

        Task<IEnumerable<BasicUserInfoDTO>> GetUserFriendsAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicUserInfoDTO>> FindAllFriendsByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicUserInfoDTO>> FindAllFriendsByFriendIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}