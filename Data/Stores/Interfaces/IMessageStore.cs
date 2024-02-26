using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using BoardGameBrawl.Data.Models.DTO;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IMessageStore<TMessage, TUser> : IDisposable where TMessage : class where TUser : class
    {
        Task<IdentityResult> CreateMessageAsync(TMessage message, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteMessageAsync(TMessage message, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateMessageAsync(TMessage message, CancellationToken cancellationToken = default);


        Task<TMessage?> GetMessageByIdAsync(string Id, CancellationToken cancellationToken = default);

        Task<BasicUserInfoDTO?> GetSenderInfoByMessageIdAsync(string Id, CancellationToken cancellationToken = default);

        Task<BasicUserInfoDTO?> GetReceiverInfoByMessageIdAsync(string Id, CancellationToken cancellationToken = default);


        Task<IEnumerable<TMessage>> GetMessagesBySenderIdAsync(string senderId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicUserInfoDTO>> GetListofMessageReceiversAsync(string senderId, CancellationToken cancellationToken = default);

        Task<IEnumerable<TMessage>> GetMessagesByReceiverIdAsync(string receiverId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicUserInfoDTO>> GetListofMessageSendersAsync(string receiverId, CancellationToken cancellationToken = default);

        Task<bool> GetIfMessageReadAsync(TMessage message, CancellationToken cancellationToken = default);
        

        Task SetSenderIdAsync(TMessage message, string senderId, CancellationToken cancellationToken = default);

        Task SetReceiverIdAsync(TMessage message, string receiverId, CancellationToken cancellationToken = default);
        
        Task SetMessageTitleAsync(TMessage message, string messageTitle, CancellationToken cancellationToken = default);

        Task SetMessageBodyAsync(TMessage message, string messageBody, CancellationToken cancellationToken = default);

        Task SetMessageSendingTimeAsync(TMessage message, DateTime sendingTime, CancellationToken cancellationToken = default);

        Task SetSenderAsync(TMessage message, TUser sender, CancellationToken cancellationToken = default);
        
        Task SetReceiverAsync(TMessage message, TUser receiver, CancellationToken cancellationToken = default);

        Task SetIsMessageReadAsync(TMessage message, bool isRead, CancellationToken cancellationToken = default);
    }
}