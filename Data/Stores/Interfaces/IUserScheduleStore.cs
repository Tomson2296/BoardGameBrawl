using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IUserScheduleStore<TSchedule, TUser> : IDisposable where TSchedule : class where TUser : class
    {
        Task<IdentityResult> CreateScheduleAsync(TSchedule schedule, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteScheduleAsync(TSchedule schedule, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateScheduleAsync(TSchedule schedule, CancellationToken cancellationToken = default);


        Task<TSchedule> FindScheduleByIdAsync(string scheduleId, CancellationToken cancellationToken = default);

        Task<TSchedule> FindScheduleByUserIdAsync(string userID, CancellationToken cancellationToken = default);

        Task<TSchedule> FindScheduleByUserAsync(TUser user, CancellationToken cancellationToken = default);

        
        Task SetUserByAsync(TSchedule schedule, TUser user, CancellationToken cancellationToken = default);

        Task ClearUserScheduleAsync(string userId, CancellationToken cancellationToken = default);
    }
}
