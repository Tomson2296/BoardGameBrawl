using Microsoft.AspNetCore.Identity;
using BoardGameBrawl.Data.Models.DTO;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IGroupParticipantStore<TGP, TGroup, TUser> : IDisposable where TGP : class where TGroup : class where TUser : class
    {
        Task<IdentityResult> CreateGroupParticipantAsync(TGP groupParticipant, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteGroupParticipantAsync(TGP groupParticipant, CancellationToken cancellationToken = default);

        Task<TGP> FindGroupParticipantByIdAsync(string Id, CancellationToken cancellationToken = default);


        Task SetOwnershipAsync(TGP groupParticipant, bool isOwner, CancellationToken cancellationToken = default);

        Task SetGroupAsync(TGP groupParticipant, TGroup group, CancellationToken cancellationToken = default);
        
        Task SetUserAsync(TGP groupParticipant, TUser user, CancellationToken cancellationToken = default);

        
        Task<bool> GetGroupOwnershipByParticipantIdAsync(TGroup group, string participantId, CancellationToken cancellationToken = default);

        Task<IEnumerable<GroupInfoDTO>> GetUserGroupsParticipationListAsync(TUser user, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicUserInfoDTO>> GetGroupParticipantsListAsync(TGroup group, CancellationToken cancellationToken = default);
    }
}
