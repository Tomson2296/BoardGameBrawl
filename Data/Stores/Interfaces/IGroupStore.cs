using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IGroupStore<TGroup> : IDisposable where TGroup : class
    {
        Task<IdentityResult> CreateGroupAsync(TGroup group, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteGroupAsync(TGroup group, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateGroupAsync(TGroup group, CancellationToken cancellationToken = default);


        Task<TGroup> FindGroupByIdAsync(string groupId, CancellationToken cancellationToken = default);
  
        Task<TGroup> FindGroupByGroupNameAsync(string groupName, CancellationToken cancellationToken = default);


        Task<string> GetGroupNameAsync(TGroup group, CancellationToken cancellationToken = default);
        
        Task SetGroupNameAsync(TGroup group, string groupName, CancellationToken cancellationToken = default);

        Task<string> GetGroupDescAsync(TGroup group, CancellationToken cancellationToken = default);

        Task SetGroupDescAsync(TGroup group, string groupDesc, CancellationToken cancellationToken = default);

        Task<DateOnly> GetGroupCreationDateAsync(TGroup group, CancellationToken cancellationToken = default);

        Task SetGroupCreationDateAsync(TGroup group, DateOnly creationDate, CancellationToken cancellationToken = default);

        Task<byte[]> GetGroupMiniatureAsync(TGroup group, CancellationToken cancellationToken = default);

        Task SetGroupMiniatureAsync(TGroup group, byte[] groupMiniature, CancellationToken cancellationToken = default);
    }
}