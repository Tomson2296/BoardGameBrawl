#nullable disable
using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class GroupStore : IGroupStore<GroupModel>
    {
        private readonly ApplicationDbContext _context;

        public GroupStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateGroupAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            _context.Groups.Add(group);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create group {group.Id}." });
        }

        public async Task<IdentityResult> DeleteGroupAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            var groupFromDB = await _context.Groups.FindAsync(group.Id);

            if (groupFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find group to deletion process: {group.Id}." });
            }
            else
            {
                _context.Groups.Remove(groupFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete group {group.Id}." });
            }
        }

        public async Task<GroupModel> FindGroupByIdAsync(string groupId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupId);
            return await _context.Groups.SingleOrDefaultAsync(g => g.Id.Equals(groupId), cancellationToken);
        }
        
        public async Task<GroupModel> FindGroupByGroupNameAsync(string groupName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupName);
            return await _context.Groups.SingleOrDefaultAsync(g => g.GroupName.Equals(groupName), cancellationToken);
        }

        public async Task<DateOnly> GetGroupCreationDateAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            return await Task.FromResult(group.GroupCreationDate);
        }

        public async Task<string> GetGroupDescAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            return await Task.FromResult(group.GroupDesc);
        }

        public async Task<byte[]> GetGroupMiniatureAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            return await Task.FromResult(group.GroupMiniature);
        }

        public async Task<string> GetGroupNameAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            return await Task.FromResult(group.GroupName);
        }

        public async Task SetGroupCreationDateAsync(GroupModel group, DateOnly creationDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            ArgumentNullException.ThrowIfNull(creationDate);
            group.GroupCreationDate = creationDate;
            await Task.CompletedTask;
        }

        public async Task SetGroupDescAsync(GroupModel group, string groupDesc, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            ArgumentException.ThrowIfNullOrEmpty(groupDesc);
            group.GroupDesc = groupDesc;
            await Task.CompletedTask;
        }

        public async Task SetGroupMiniatureAsync(GroupModel group, byte[] groupMiniature, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            ArgumentNullException.ThrowIfNull(groupMiniature);
            group.GroupMiniature = groupMiniature;
            await Task.CompletedTask;
        }

        public async Task SetGroupNameAsync(GroupModel group, string groupName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            ArgumentException.ThrowIfNullOrEmpty(groupName);
            group.GroupName = groupName;
            await Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateGroupAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            _context.Groups.Update(group);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update group {group.Id}." });
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