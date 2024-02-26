#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.AppUser
{
    public class UserScheduleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;

        public UserScheduleModel(UserManager<ApplicationUser> userManager, IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore)
        {
            _userManager = userManager;
            _userScheduleStore = userScheduleStore;
        }
        
        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public UserSchedule UserSchedule { get; set; }

        public IList<string> DivPositions { get; set; } = new List<string>();

        public IList<string> DivHeights { get; set; } = new List<string>();

        [BindProperty]
        public Dictionary<DayOfWeek, int> NoOfDivsPerDayCreated { get; set; } = new Dictionary<DayOfWeek, int>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.FindByNameAsync(UserName);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with UserName {UserName}.");
            }
            UserSchedule = await _userScheduleStore.FindScheduleByUserIdAsync(ApplicationUser.Id);
            CreateDivParameters(UserSchedule, DivPositions, DivHeights, NoOfDivsPerDayCreated);
            return Page();
        }
        
        private void CreateDivParameters(UserSchedule schedule, IList<string> posParams,
            IList<string> heightParams, Dictionary<DayOfWeek, int> numberOfDivsPerDay)
        {
            SetUpParameters(schedule.Monday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Monday);
            SetUpParameters(schedule.Tuesday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Tuesday);
            SetUpParameters(schedule.Wednesday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Wednesday);
            SetUpParameters(schedule.Thursday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Thursday);
            SetUpParameters(schedule.Friday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Friday);
            SetUpParameters(schedule.Saturday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Saturday);
            SetUpParameters(schedule.Sunday, posParams, heightParams, numberOfDivsPerDay, DayOfWeek.Sunday);
        }

        private void SetUpParameters(IList<string> daySchedule, IList<string> posParams,
            IList<string> heightParams, Dictionary<DayOfWeek, int> numberOfDivsPerDay, DayOfWeek dayOfWeek)
        {
            numberOfDivsPerDay.Add(dayOfWeek, 0);
            if (!daySchedule.Any())
            {
                posParams.Add(0.ToString());
                heightParams.Add(720.ToString());
                numberOfDivsPerDay[dayOfWeek] = 1;
            }
            else
            {
                // content div - 720px heigh
                // schedule separated into 12 parts : 2h represents 720/12 = 60px of height , 1h - 30px of height

                // calculating height of div if schedule for a day has been set 
                int dayScheduleElemens = daySchedule.Count;
                int i = 0, j = i + 1;

                for (; i < dayScheduleElemens;)
                {
                    string startTimeString = daySchedule.ElementAt(i);
                    string endTimeString = daySchedule.ElementAt(j);

                    // resolving hard scenario endingTime < staringTime
                    if (int.Parse(endTimeString.Split(":")[0]) < int.Parse(startTimeString.Split(":")[0]) && endTimeString != "00:00")
                    {
                        // bad scenario 
                        string endingHour = endTimeString.Split(":")[0];
                        string endingMinutes = endTimeString.Split(":")[1];

                        int differenceValue = (int.Parse(endingHour) + 24) * 60 + int.Parse(endTimeString.Split(":")[1]) -
                        (int.Parse(startTimeString.Split(":")[0]) * 60 + int.Parse(startTimeString.Split(":")[1]));

                        // calculate height for div 24:00 - x:x
                        int HeightForFirst = int.Parse(endingHour) * 60 + int.Parse(endingMinutes);
                        string calculatedHeightForFirst = (HeightForFirst / 2).ToString();
                        heightParams.Add(calculatedHeightForFirst);

                        // calculate height for div x:x - 24:00
                        int HeightForSecond = differenceValue - HeightForFirst;
                        string calculatedHeightForSecond = (HeightForSecond / 2).ToString();
                        heightParams.Add(calculatedHeightForSecond);

                        // calculate position for second div -> first straitforward 0
                        posParams.Add(0.ToString());

                        string startTime_Hour = startTimeString.Split(":")[0];
                        string startHour_Minutes = startTimeString.Split(":")[1];

                        string secondCalculatedPosition = (int.Parse(startTime_Hour) * 30 + int.Parse(startHour_Minutes) / 2).ToString();
                        posParams.Add(secondCalculatedPosition);

                        // set adnotation that in that scenario 2 divs has to be created
                        int valueAlready = numberOfDivsPerDay[dayOfWeek];
                        numberOfDivsPerDay[dayOfWeek] = valueAlready + 1;

                        // increment indexes by two spaces -> always taking pair of values
                        i += 2;
                        j += 2;
                    }
                    else
                    {
                        //classic, easy scenario
                        //in case ending time equals 00:00 change it to 24:00 (TimeOnly object never returns 24:00)
                        //if (endTimeString == "00:00")
                        //    endTimeString = "24:00";

                        int differenceValue = int.Parse(endTimeString.Split(":")[0]) * 60 + int.Parse(endTimeString.Split(":")[1]) -
                        (int.Parse(startTimeString.Split(":")[0]) * 60 + int.Parse(startTimeString.Split(":")[1]));

                        string calculatedValue = (differenceValue / 2).ToString();
                        heightParams.Add(calculatedValue);

                        // calculating position of div depending on startTime

                        string startTime_Hour = startTimeString.Split(":")[0];
                        string startHour_Minutes = startTimeString.Split(":")[1];

                        string calculatedPosition = (int.Parse(startTime_Hour) * 30 + int.Parse(startHour_Minutes) / 2).ToString();
                        posParams.Add(calculatedPosition);

                        int valueAlready = numberOfDivsPerDay[dayOfWeek];
                        numberOfDivsPerDay[dayOfWeek] = valueAlready + 1;

                        // increment indexes by two spaces -> always taking pair of values
                        i += 2;
                        j += 2;
                    }
                }
            }
        }
    }
}