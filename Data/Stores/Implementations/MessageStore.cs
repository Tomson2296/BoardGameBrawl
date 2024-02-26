using AutoMapper.Configuration.Annotations;
using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class MessageStore : IMessageStore<MessageModel, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration;

        public MessageStore(ApplicationDbContext context, AutoMapper.IConfigurationProvider configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateMessageAsync(MessageModel message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentException.ThrowIfNullOrEmpty(message.SenderId);
            ArgumentException.ThrowIfNullOrEmpty(message.ReceiverId);

            _context.Messages.Add(message);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create message {message.Id}." });
        }

        public async Task<IdentityResult> DeleteMessageAsync(MessageModel message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            var messageFromDB = await _context.Messages.FindAsync(message.Id);

            if (messageFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find message to deletion process: {message.Id}." });
            }
            else
            {
                _context.Messages.Remove(messageFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete message {message.Id}." });
            }
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesBySenderIdAsync(string senderId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(senderId);
            return await _context.Messages
                .AsNoTracking()
                .Where(m => m.SenderId == senderId)
                .OrderByDescending(m => m.MessageSentTime)
                .ToListAsync(cancellationToken);
        }
        
        public async Task SetSenderIdAsync(MessageModel message, string senderId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(senderId);
            message.SenderId = senderId;
            await Task.CompletedTask;
        }

        public async Task SetReceiverIdAsync(MessageModel message, string receiverId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(receiverId);
            message.ReceiverId = receiverId;
            await Task.CompletedTask;
        }

        public async Task SetMessageTitleAsync(MessageModel message, string messageTitle, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(messageTitle);
            message.MessageTopic = messageTitle;
            await Task.CompletedTask;
        }

        public async Task SetMessageBodyAsync(MessageModel message, string messageBody, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(messageBody);
            message.MessageBody = messageBody;
            await Task.CompletedTask;
        }

        public async Task SetMessageSendingTimeAsync(MessageModel message, DateTime sendingTime, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(sendingTime);
            message.MessageSentTime = sendingTime;
            await Task.CompletedTask;
        }

        public async Task SetSenderAsync(MessageModel message, ApplicationUser sender, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(sender);
            message.Sender = sender;
            await Task.CompletedTask;
        }

        public async Task SetReceiverAsync(MessageModel message, ApplicationUser receiver, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(receiver);
            message.Receiver = receiver;
            await Task.CompletedTask;
        }

        public async Task SetIsMessageReadAsync(MessageModel message, bool isRead, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(isRead);
            message.IsMessageRead = isRead;
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesByReceiverIdAsync(string receiverId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(receiverId);
            return await _context.Messages
                .AsNoTracking()
                .Where(m => m.ReceiverId == receiverId)
                .OrderByDescending(m => m.MessageSentTime)
                .ToListAsync(cancellationToken);
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

        public async Task<IEnumerable<BasicUserInfoDTO>> GetListofMessageReceiversAsync(string senderId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(senderId);
            return await _context.Messages
                .AsNoTracking()
                .Where(m => m.SenderId == senderId)
                .Select(m => m.Receiver)
                .ProjectTo<BasicUserInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicUserInfoDTO>> GetListofMessageSendersAsync(string receiverId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(receiverId);
            return await _context.Messages
                .AsNoTracking()
                .Where(m => m.ReceiverId == receiverId)
                .Select(m => m.Sender)
                .ProjectTo<BasicUserInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<BasicUserInfoDTO?> GetSenderInfoByMessageIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(Id);
            return await _context.Messages
                .AsNoTracking()
                .Where(m => m.Id == Id)
                .Select(m => m.Sender)
                .ProjectTo<BasicUserInfoDTO>(_configuration)
                .FirstAsync(cancellationToken);
        }

        public async Task<BasicUserInfoDTO?> GetReceiverInfoByMessageIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(Id);
            return await _context.Messages
               .AsNoTracking()
               .Where(m => m.Id == Id)
               .Select(m => m.Receiver)
               .ProjectTo<BasicUserInfoDTO>(_configuration)
               .FirstAsync(cancellationToken);
        }

        public async Task<MessageModel?> GetMessageByIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(Id);
            return await _context.Messages.AsNoTracking().SingleOrDefaultAsync(m => m.Id.Equals(Id), cancellationToken);
        }

        public async Task<IdentityResult> UpdateMessageAsync(MessageModel message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            _context.Messages.Update(message);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update message {message.Id}." });
        }

        public async Task<bool> GetIfMessageReadAsync(MessageModel message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(message);
            return await Task.FromResult(message.IsMessageRead);
        }
    }
}