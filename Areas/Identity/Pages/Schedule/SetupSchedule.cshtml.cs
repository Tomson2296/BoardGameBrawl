#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace BoardGameBrawl.Areas.Identity.Pages.Schedule
{
    public class SetupScheduleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;

        public SetupScheduleModel(UserManager<ApplicationUser> userManager, IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore)
        {
            _userManager = userManager;
            _userScheduleStore = userScheduleStore;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public UserSchedule UserSchedule { get; set; }

        [BindProperty]
        public IList<DayOfWeek> DaysOfWeek { get; set; }

        [BindProperty]
        public string StartTime { get; set; }

        [BindProperty]
        public string EndTime { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser = await _userManager.GetUserAsync(User);
                    UserSchedule = await _userScheduleStore.FindScheduleByUserIdAsync(ApplicationUser.Id);
                    List<string> TimesValues = [StartTime, EndTime];

                    foreach (DayOfWeek selectedDay in DaysOfWeek)
                    {
                        string startTimeString = TimesValues.ElementAt(0);
                        string endTimeString = TimesValues.ElementAt(1);

                        string startTime_Hour = startTimeString.Split(":")[0];
                        string startTime_Minutes = startTimeString.Split(":")[1];
                        int startTimeValue = int.Parse(startTime_Hour) * 60 + int.Parse(startTime_Minutes);

                        string endTime_Hour = endTimeString.Split(":")[0];
                        string endTime_Minutes = endTimeString.Split(":")[1];
                        int endTimeValue = int.Parse(endTime_Hour) * 60 + int.Parse(endTime_Minutes);

                        if (startTimeValue == endTimeValue)
                        {
                            // scenario when startTimeValue = endTimeValue (the same setup times - nothing should happen)
                            // return back to page
                            StatusMessage = "Error: Starting time the same as ending time - Try again";
                            return Page();
                        }
                        else if (startTimeValue > endTimeValue)
                        {
                            // hard scenario : for example 18:00 - 1:00
                            // separate into two entities : first 18:00 - 24:00, second 0:00 - 1:00 on next day
                            string firstEntityEndTime = "24:00";
                            List<string> FirstEntityValue = [StartTime, firstEntityEndTime];

                            if (await _userScheduleStore.CheckIfSetDayofWeekScheduleAsync(UserSchedule, selectedDay))
                            {
                                // check if new values actually overlapping any values for a weekday already existing in database
                                if (await _userScheduleStore.CheckIfDayofWeekScheduleOverlapAsync(UserSchedule, FirstEntityValue, selectedDay))
                                {
                                    // if true: reset schedule for that particular day and set a new values
                                    await _userScheduleStore.ResetDayofWeekScheduleAsync(UserSchedule, selectedDay);
                                }
                                await _userScheduleStore.SetDayofWeekScheduleAsync(UserSchedule, FirstEntityValue, selectedDay);
                            }
                            else
                            {
                                await _userScheduleStore.SetDayofWeekScheduleAsync(UserSchedule, FirstEntityValue, selectedDay);
                            }

                            // special case - checking if Ending Time == 00:00 (either creating a new entity (if not 24th hour) or done)
                            if (EndTime != "00:00")
                            {
                                string secondEntityStartTime = "00:00";
                                List<string> SecondEntityValue = [secondEntityStartTime, EndTime];
                                DayOfWeek nextDay = PassAvailabilityOnNextDay(selectedDay);

                                if (await _userScheduleStore.CheckIfSetDayofWeekScheduleAsync(UserSchedule, nextDay))
                                {
                                    // check if new values actually overlapping any values for a weekday already existing in database
                                    if (await _userScheduleStore.CheckIfDayofWeekScheduleOverlapAsync(UserSchedule, SecondEntityValue, nextDay))
                                    {
                                        // if true: reset schedule for that particular day and set a new values
                                        await _userScheduleStore.ResetDayofWeekScheduleAsync(UserSchedule, nextDay);
                                    }
                                    await _userScheduleStore.SetDayofWeekScheduleAsync(UserSchedule, SecondEntityValue, nextDay);
                                }
                                else
                                {
                                    await _userScheduleStore.SetDayofWeekScheduleAsync(UserSchedule, SecondEntityValue, nextDay);
                                }
                            }
                        }
                        else
                        {
                            // easy scenario: startingTime < endingTime (ending Time before 24:00)
                            if (await _userScheduleStore.CheckIfSetDayofWeekScheduleAsync(UserSchedule, selectedDay))
                            {
                                // check if new values actually overlapping any values for a weekday already existing in database
                                if (await _userScheduleStore.CheckIfDayofWeekScheduleOverlapAsync(UserSchedule, TimesValues, selectedDay))
                                {
                                    // if true: reset schedule for that particular day and set a new values
                                    await _userScheduleStore.ResetDayofWeekScheduleAsync(UserSchedule, selectedDay);
                                }
                                await _userScheduleStore.SetDayofWeekScheduleAsync(UserSchedule, TimesValues, selectedDay);
                            }
                            else
                            {
                                await _userScheduleStore.SetDayofWeekScheduleAsync(UserSchedule, TimesValues, selectedDay);
                            }
                        }
                    }

                    IdentityResult result = await _userScheduleStore.UpdateScheduleAsync(UserSchedule);
                    
                    if (result.Succeeded)
                    {
                        StatusMessage = "New entry / entries to your schedule has been added";
                        return RedirectToPage();
                    }
                    else
                    {
                        StatusMessage = "Error during setup yor schedule - Try again later";
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error during setup your schedule - {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }

        private static DayOfWeek PassAvailabilityOnNextDay(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return DayOfWeek.Tuesday;

                case DayOfWeek.Tuesday:
                    return DayOfWeek.Wednesday;

                case DayOfWeek.Wednesday:
                    return DayOfWeek.Thursday;

                case DayOfWeek.Thursday:
                    return DayOfWeek.Friday;

                case DayOfWeek.Friday:
                    return DayOfWeek.Saturday;

                case DayOfWeek.Saturday:
                    return DayOfWeek.Sunday;

                case DayOfWeek.Sunday:
                    return DayOfWeek.Monday;

                default:
                    return dayOfWeek;
            }
        }

        //private string TransformToAppropriateTime(string time)
        //{
        //string[] restructTime = time.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        //string hours = restructTime[0];
        //string minutes = restructTime[1];

        //if (!minutes.Equals("00") || !minutes.Equals("30"))
        //{
        //bool ifParse = int.TryParse(minutes, out int minuteValue);
        //if (minuteValue <= 30)
        //{
        //int value = 30 - minuteValue;
        //if (value < 15)
        //return string.Concat(hours, ":", "00"); 
        //else
        //return string.Concat(hours, ":", "30");
        //}
        //else
        //{
        //int value = 60 - minuteValue;
        //if (value < 15)
        //return string.Concat(hours, ":", "30");
        //else
        //int hoursValue = int.Parse(hours) + 1;
        //string newHour =  hoursValue.ToString();
        //return string.Concat(newHour, ":", "00");
        //}
        //}
        //return time;
        //}
    }
}