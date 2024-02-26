#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public static class UserScheduleExtensions
    {
        public static async Task<List<string>> GetDayofWeekScheduleAsync(this IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
            UserSchedule userSchedule, DayOfWeek dayOfWeek, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userSchedule);

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return await Task.FromResult(userSchedule.Monday);

                case DayOfWeek.Tuesday:
                    return await Task.FromResult(userSchedule.Tuesday);

                case DayOfWeek.Wednesday:
                    return await Task.FromResult(userSchedule.Wednesday);

                case DayOfWeek.Thursday:
                    return await Task.FromResult(userSchedule.Thursday);

                case DayOfWeek.Friday:
                    return await Task.FromResult(userSchedule.Friday);

                case DayOfWeek.Saturday:
                    return await Task.FromResult(userSchedule.Saturday);

                case DayOfWeek.Sunday:
                    return await Task.FromResult(userSchedule.Sunday);

                default:
                    return await Task.FromResult(new List<string>());
            }
        }

        public static async Task ResetDayofWeekScheduleAsync(this IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
            UserSchedule userSchedule, DayOfWeek dayOfWeek, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userSchedule);

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    userSchedule.Monday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Tuesday:
                    userSchedule.Tuesday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Wednesday:
                    userSchedule.Wednesday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Thursday:
                    userSchedule.Thursday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Friday:
                    userSchedule.Friday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Saturday:
                    userSchedule.Saturday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Sunday:
                    userSchedule.Sunday.Clear();
                    await userScheduleStore.UpdateScheduleAsync(userSchedule, CancellationToken.None);
                    await Task.CompletedTask;
                    break;

                default:
                    await Task.CompletedTask;
                    break;
            }
        }

        public static async Task SetDayofWeekScheduleAsync(this IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
            UserSchedule userSchedule, List<string> day_values, DayOfWeek dayOfWeek, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userSchedule);
            ArgumentNullException.ThrowIfNull(day_values);

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    userSchedule.Monday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Tuesday:
                    userSchedule.Tuesday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Wednesday:
                    userSchedule.Wednesday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Thursday:
                    userSchedule.Thursday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Friday:
                    userSchedule.Friday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Saturday:
                    userSchedule.Saturday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                case DayOfWeek.Sunday:
                    userSchedule.Sunday.AddRange(day_values);
                    await Task.CompletedTask;
                    break;

                default:
                    await Task.CompletedTask;
                    break;
            }
        }

        public static async Task<bool> CheckIfSetDayofWeekScheduleAsync(this IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
            UserSchedule userSchedule, DayOfWeek dayOfWeek, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userSchedule);

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return await Task.FromResult(userSchedule.Monday.Count != 0);

                case DayOfWeek.Tuesday:
                    return await Task.FromResult(userSchedule.Tuesday.Count != 0);

                case DayOfWeek.Wednesday:
                    return await Task.FromResult(userSchedule.Wednesday.Count != 0);

                case DayOfWeek.Thursday:
                    return await Task.FromResult(userSchedule.Thursday.Count != 0);

                case DayOfWeek.Friday:
                    return await Task.FromResult(userSchedule.Friday.Count != 0);

                case DayOfWeek.Saturday:
                    return await Task.FromResult(userSchedule.Saturday.Count != 0);

                case DayOfWeek.Sunday:
                    return await Task.FromResult(userSchedule.Sunday.Count != 0);

                default:
                    return await Task.FromResult(false);
            }
        }

        public static async Task<bool> CheckIfDayofWeekScheduleOverlapAsync(this IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
           UserSchedule userSchedule, List<string> day_values, DayOfWeek dayOfWeek, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(dayOfWeek);
            ArgumentNullException.ThrowIfNull(userSchedule);
            ArgumentNullException.ThrowIfNull(day_values);

            List<string> values = await GetDayofWeekScheduleAsync(userScheduleStore, userSchedule, dayOfWeek);
            // if there is none values in weekday field - cannot overlap -> return false
            if (!values.Any())
                return await Task.FromResult(false);

            // calculating entered string values into int value for a purpose of evaluation
            string enteredStartTimeString = day_values.ElementAt(0);
            string enteredEndTimeString = day_values.ElementAt(1);

            string enteredStartTime_Hour = enteredStartTimeString.Split(":")[0];
            string enteredStartTime_Minutes = enteredStartTimeString.Split(":")[1];
            int enteredStartTimeValue = int.Parse(enteredStartTime_Hour) * 60 + int.Parse(enteredStartTime_Minutes);
            
            string enteredEndTime_Hour = enteredEndTimeString.Split(":")[0];
            string enteredEndTime_Minutes = enteredEndTimeString.Split(":")[1];
            int enteredEndTimeValue = int.Parse(enteredEndTime_Hour) * 60 + int.Parse(enteredEndTime_Minutes);

            int valuesCount = values.Count;
            int notOverlappedValues = 0;
            int i = 0, j = i + 1;
            for (; i < valuesCount; )
            {
                // calculating startTime and endTime to int value for a purpose of evaluation
                string startTimeString = values.ElementAt(i);
                string endTimeString = values.ElementAt(j);

                string startTime_Hour = startTimeString.Split(":")[0];
                string startTime_Minutes = startTimeString.Split(":")[1];
                int startTimeValue = int.Parse(startTime_Hour) * 60 + int.Parse(startTime_Minutes);

                string endTime_Hour = endTimeString.Split(":")[0];
                string endTime_Minutes = endTimeString.Split(":")[1];
                int endTimeValue = int.Parse(endTime_Hour) * 60 + int.Parse(endTime_Minutes);

                // checking if entered values overlappping with taken pair of times : startingTime and endingTime
                if (enteredEndTimeValue <= startTimeValue || enteredStartTimeValue >= endTimeValue)
                    notOverlappedValues++;
                else
                    return await Task.FromResult(true);
                
                // increment in search if another pair of saved time values
                i += 2;
                j += 2;
            }

            if (notOverlappedValues == valuesCount/2)
                return await Task.FromResult(false);
            else
                return await Task.FromResult(true);
        }
    }
}