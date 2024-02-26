using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using System.Globalization;

namespace BoardGameBrawl.Services
{
    public class DateService : IDateService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;

        public DateService(ApplicationDbContext context, IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleService)
        {
            _context = context;
            _userScheduleStore = userScheduleService;
        }

        public async Task<bool> CheckUserScheduleAvailibilityAsync(DateTime date, UserSchedule schedule)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;
            bool checkDayScheduleSet = await _userScheduleStore.CheckIfSetDayofWeekScheduleAsync(schedule, dayOfWeek);
            if (!checkDayScheduleSet)
                return false;

            List<string> setAvailabilityHours = await _userScheduleStore.GetDayofWeekScheduleAsync(schedule, dayOfWeek);

            int availabilityHoursCount = setAvailabilityHours.Count;
            int i = 0, j = i + 1;

            // calculate actualTimeValue independently from two cases
            string actualTime = date.ToString("t", CultureInfo.InvariantCulture);
            int actualTimeValue = int.Parse(actualTime.Split(":")[0]) * 60 + int.Parse(actualTime.Split(":")[1]);

            while (i < availabilityHoursCount)
            {
                int startingHourValue = int.Parse(setAvailabilityHours[i].Split(":")[0]) * 60 + int.Parse(setAvailabilityHours[i].Split(":")[1]);
                int endingHourValue = int.Parse(setAvailabilityHours[j].Split(":")[0]) * 60 + int.Parse(setAvailabilityHours[j].Split(":")[1]);

                if (actualTimeValue <= endingHourValue && actualTimeValue >= startingHourValue)
                {
                    return true;
                }
                else
                {
                    // move indexes by two spaces and take another pair of starting / ending hours
                    i = i + 2;
                    j = j + 2;
                    continue;
                }
            }
            // returning false -> user is not available after checking every pair of starting / ending hours
            return false;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<string> GetTimeAsync(DateTime date)
        {
            return await Task.FromResult(date.ToString("dddd, dd MMMM yyyy"));
        }
    }
}
