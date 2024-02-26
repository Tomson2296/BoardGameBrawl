using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public static class UserNotificationExtensions
    {
        public static async Task<NotificationType> GetNotificationTypeAsync(this IUserNotificationStore<UserNotification, ApplicationUser> userNotificationStore,
            UserNotification userNotification, CancellationToken cancellationToken = default)
        {  
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userNotification);
            return await Task.FromResult(userNotification.NotificationType);
        }

        public static async Task SetNotificationTypeAsync(this IUserNotificationStore<UserNotification, ApplicationUser> userNotificationStore,
            UserNotification userNotification, NotificationType notificationType, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userNotification);
            ArgumentNullException.ThrowIfNull(notificationType);
            userNotification.NotificationType = notificationType;
            await Task.CompletedTask;
        }
    }
}
