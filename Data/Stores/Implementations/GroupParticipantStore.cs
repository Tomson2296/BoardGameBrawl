#nullable disable
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class GroupParticipantStore : IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration;
        
        public GroupParticipantStore(ApplicationDbContext context, AutoMapper.IConfigurationProvider configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateGroupParticipantAsync(GroupParticipant groupParticipant, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupParticipant.GroupId);
            ArgumentNullException.ThrowIfNull(groupParticipant.ParticipantId);
            _context.GroupParticipants.Add(groupParticipant);

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create entity of groupParticipant." });
        }

        public async Task<IdentityResult> DeleteGroupParticipantAsync(GroupParticipant groupParticipant, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupParticipant);
            var groupParticipantFromDB = await _context.GroupParticipants.FindAsync(groupParticipant);

            if (groupParticipantFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find groupParticipant to deletion process: {groupParticipant.Id}." });
            }
            else
            {
                _context.GroupParticipants.Remove(groupParticipantFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete groupParticipant {groupParticipant.Id}." });
            }
        }

        public async Task<GroupParticipant> FindGroupParticipantByIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(Id);
            return await _context.GroupParticipants.SingleOrDefaultAsync(gp => gp.Id.Equals(Id), cancellationToken);
        }

        public async Task SetGroupAsync(GroupParticipant groupParticipant, GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupParticipant);
            ArgumentNullException.ThrowIfNull(group);
            groupParticipant.GroupId = group.Id;
            await Task.CompletedTask;
        }

        public async Task SetOwnershipAsync(GroupParticipant groupParticipant, bool isOwner, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupParticipant);
            ArgumentNullException.ThrowIfNull(isOwner);
            groupParticipant.IsOwner = isOwner;
            await Task.CompletedTask;
        }

        public async Task SetUserAsync(GroupParticipant groupParticipant, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(groupParticipant);
            ArgumentNullException.ThrowIfNull(user);
            groupParticipant.ParticipantId = user.Id;
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

        public async Task<IEnumerable<GroupInfoDTO>> GetUserGroupsParticipationListAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await _context.GroupParticipants
                .AsNoTracking()
                .Where(gp => gp.ParticipantId == user.Id)
                .Select(gp => gp.Group)
                .ProjectTo<GroupInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicUserInfoDTO>> GetGroupParticipantsListAsync(GroupModel group, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            return await _context.GroupParticipants
                .AsNoTracking()
                .Where(gp => gp.GroupId == group.Id)
                .Select(gp => gp.Participant)
                .ProjectTo<BasicUserInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> GetGroupOwnershipByParticipantIdAsync(GroupModel group, string participantId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(group);
            ArgumentException.ThrowIfNullOrEmpty(participantId);
            return await _context.GroupParticipants
                .AsNoTracking()
                .Where(gp => gp.Group.Id == group.Id)
                .Select(gp => gp.ParticipantId == participantId)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}