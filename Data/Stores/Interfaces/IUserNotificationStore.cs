using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IUserNotificationStore<TNotify, TUser> : IDisposable where TNotify : class where TUser : class
    {
        Task<IdentityResult> CreateNotificationAsync(TNotify notification, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteNotificationAsync(TNotify notification, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateNotificationAsync(TNotify notification, CancellationToken cancellationToken = default);



        Task<TNotify> FindNotificationByIdAsync(string Id, CancellationToken cancellationToken = default);

        Task<IEnumerable<TNotify>> FindNotificationsByReceiverIdAsync(string receiverId, CancellationToken cancellationToken = default);

        

        Task SetReceiverAsync(TNotify notification, TUser user, CancellationToken cancellationToken = default);

        Task SetNotificationAsync(TNotify notification, string notificationDesc, CancellationToken cancellationToken = default);

        Task<string> GetNotificationAsync(TNotify notification, CancellationToken cancellationToken = default);

        Task SetNotificationShown(TNotify notification, bool isShown, CancellationToken cancellationToken = default);

        Task<bool> GetNotificationShown(TNotify notification, CancellationToken cancellationToken = default);
    }
}
