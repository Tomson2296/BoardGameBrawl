#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class UserNotificationStore : IUserNotificationStore<UserNotification, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public UserNotificationStore(ApplicationDbContext context)
        {
            _context= context;
        }
        public async Task<IdentityResult> CreateNotificationAsync(UserNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentException.ThrowIfNullOrEmpty(notification.ReceiverId);

            _context.UserNotifications.Add(notification);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create notification {notification.Id}." });
        }

        public async Task<IdentityResult> DeleteNotificationAsync(UserNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            var notificationFromDB = await _context.UserNotifications.FindAsync(notification.Id);

            if (notificationFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find notification to deletion process: {notification.Id}." });
            }
            else
            {
                _context.UserNotifications.Remove(notificationFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete notification {notification.Id}." });
            }
        }

        public async Task<IdentityResult> UpdateNotificationAsync(UserNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            _context.UserNotifications.Update(notification);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update notification {notification.Id}." });
        }

        public async Task<UserNotification> FindNotificationByIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(Id);
            return await _context.UserNotifications.AsNoTracking().SingleOrDefaultAsync(n => n.Id.Equals(Id), cancellationToken);
        }

        public async Task<IEnumerable<UserNotification>> FindNotificationsByReceiverIdAsync(string receiverId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(receiverId);
            return await _context.UserNotifications.AsNoTracking().Where(un => un.ReceiverId.Equals(receiverId)).ToListAsync(cancellationToken);
        }

        public async Task<string> GetNotificationAsync(UserNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            return await Task.FromResult(notification.Notification);
        }

        public async Task SetNotificationAsync(UserNotification notification, string notificationDesc, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notificationDesc);
            notification.Notification = notificationDesc;
            await Task.CompletedTask;
        }

        public async Task SetReceiverAsync(UserNotification notification, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(user);
            notification.ReceiverId = user.Id;
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

        public async Task SetNotificationShown(UserNotification notification, bool isShown, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(isShown);
            notification.IsShown = isShown;
            await Task.CompletedTask;
        }

        public async Task<bool> GetNotificationShown(UserNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(notification);
            return await Task.FromResult(notification.IsShown);
        }
    }
}