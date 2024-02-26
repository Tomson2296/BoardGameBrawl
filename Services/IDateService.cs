using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Services
{
    public interface IDateService
    {
        Task<string> GetTimeAsync(DateTime date);

        Task<bool> CheckUserScheduleAvailibilityAsync(DateTime date, UserSchedule schedule);
    }
}
