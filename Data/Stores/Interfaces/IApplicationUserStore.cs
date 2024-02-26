using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IApplicationUserStore : IUserStore<ApplicationUser>
    {
        public Task<string?> GetBGGUsernameAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        public Task SetBGGUsernameAsync(ApplicationUser user, string? BGGUsername, CancellationToken cancellationToken = default);

        public Task<string?> GetUserDescriptionAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        public Task SetUserDescriptionAsync(ApplicationUser user, string? userDescription, CancellationToken cancellationToken = default);

        public Task<DateOnly?> GetUserDateTimeCreationAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        public Task SetUserDateTimeCreationAsync(ApplicationUser user, DateOnly creationDate, CancellationToken cancellationToken = default);

        public Task<DateOnly?> GetUserLastLoginDateAsync(ApplicationUser user, CancellationToken cancellationToken = default);
       
        public Task SetUserLastLoginDateAsync(ApplicationUser user, DateOnly userLastLogin, CancellationToken cancellationToken = default);

        public Task<byte[]?> GetUserAvatarAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        public Task SetUserAvatarAsync(ApplicationUser user, byte[]? userImage, CancellationToken cancellationToken = default);

        public Task<string?> GetUserFirstNameAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        public Task SetUserFirstNameAsync(ApplicationUser user, string? firstName, CancellationToken cancellationToken = default);

        public Task<string?> GetUserLastNameAsync(ApplicationUser user, CancellationToken cancellationToken = default);

        public Task SetUserLastNameAsync(ApplicationUser user, string? lastName, CancellationToken cancellationToken = default);
    }
}