#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class UserScheduleStore : IUserScheduleStore<UserSchedule, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public UserScheduleStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateScheduleAsync(UserSchedule schedule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(schedule);
            ArgumentException.ThrowIfNullOrEmpty(schedule.UserId);

            _context.UserSchedules.Add(schedule);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create schedule {schedule.Id}." });
        }

        public async Task<IdentityResult> DeleteScheduleAsync(UserSchedule schedule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(schedule);
            var scheduleFromDB = await _context.UserSchedules.FindAsync(schedule.Id);

            if (scheduleFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find schedule to deletion process: {schedule.Id}." });
            }
            else
            {
                _context.UserSchedules.Remove(scheduleFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete schedule {schedule.Id}." });
            }
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

        public async Task<UserSchedule> FindScheduleByIdAsync(string scheduleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(scheduleId);
            return await _context.UserSchedules.AsNoTracking().SingleOrDefaultAsync(s => s.Id.Equals(scheduleId), cancellationToken);
        }

        public async Task<UserSchedule> FindScheduleByUserAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            return await _context.UserSchedules.AsNoTracking().SingleOrDefaultAsync(s => s.UserId.Equals(user.Id), cancellationToken);
        }

        public async Task<UserSchedule> FindScheduleByUserIdAsync(string userID, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userID);
            return await _context.UserSchedules.AsNoTracking().SingleOrDefaultAsync(s => s.UserId.Equals(userID), cancellationToken);
        }

        public async Task<IdentityResult> UpdateScheduleAsync(UserSchedule schedule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(schedule);
            _context.UserSchedules.Update(schedule);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update schedule {schedule.Id}." });
        }

        public async Task SetUserByAsync(UserSchedule schedule, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(schedule);
            ArgumentNullException.ThrowIfNull(user);
            schedule.UserId = user.Id;
            await Task.CompletedTask;
        }

        public async Task ClearUserScheduleAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userId);
            UserSchedule userSchedule = await FindScheduleByUserIdAsync(userId, cancellationToken);

            if (userSchedule.Monday.Count != 0)
                userSchedule.Monday.Clear();

            if (userSchedule.Tuesday.Count != 0)
                userSchedule.Tuesday.Clear();

            if (userSchedule.Wednesday.Count != 0)
                userSchedule.Wednesday.Clear();

            if (userSchedule.Thursday.Count != 0)
                userSchedule.Thursday.Clear();

            if (userSchedule.Friday.Count != 0)
                userSchedule.Friday.Clear();

            if (userSchedule.Saturday.Count != 0)
                userSchedule.Saturday.Clear();

            if (userSchedule.Sunday.Count != 0)
                userSchedule.Sunday.Clear();

            await UpdateScheduleAsync(userSchedule, cancellationToken);
            await Task.CompletedTask;
        }
    }
}