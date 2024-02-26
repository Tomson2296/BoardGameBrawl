#nullable disable
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;
        private readonly IMatchStore<MatchModel, BoardgameModel, ApplicationUser> _matchStore;
        private readonly IDateService _dateService;

        public IndexModel(UserManager<ApplicationUser> userManager,
            IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
            IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore,
            IDateService dateService)
        {
            _userManager = userManager;
            _matchStore = matchStore;
            _userScheduleStore = userScheduleStore;
            _dateService = dateService;
        }

        [BindProperty]
        public string Filter { get; set; }

        [BindProperty]
        public string MatchProgress { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public UserSchedule AppUserSchedule { get; set; }

        public IEnumerable<BasicMatchInfoDTO> Upcoming_Matches { get; set; } = new List<BasicMatchInfoDTO>();

        public IEnumerable<BasicMatchInfoDTO> Started_Matches { get; set; } = new List<BasicMatchInfoDTO>();

        public IList<BoardgameDTO> BoardgameList { get; set; } = new List<BoardgameDTO>();

        public IList<BasicUserInfoDTO> UserList { get; set; } = new List<BasicUserInfoDTO>();


        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            AppUserSchedule = await _userScheduleStore.FindScheduleByUserIdAsync(ApplicationUser.Id);

            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index", new { Filter, MatchProgress });
        }
    }
}
