#nullable disable
using AutoMapper;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class ShowUserMatchesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;
        private readonly IMatchStore<MatchModel, BoardgameModel, ApplicationUser> _matchStore;
        private readonly IMapper _mapper;

        public ShowUserMatchesModel(UserManager<ApplicationUser> userManager, 
            IBoardGameStore<BoardgameModel> boardgameStore, 
            IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore,
            IMapper mapper)
        {
            _userManager = userManager;
            _boardgameStore = boardgameStore;
            _matchStore = matchStore;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        // lists for retrieving information

        public IEnumerable<BasicMatchInfoDTO> UserMatches { get; set; } = new List<BasicMatchInfoDTO>();

        public IList<BoardgameDTO> BoardgameList { get; set; } = new List<BoardgameDTO>();

        public IList<BasicUserInfoDTO> UserList { get; set; } = new List<BasicUserInfoDTO>();


        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Filter == "Upcoming")
            {
                UserMatches = await _matchStore.FindAllUpcomingMatchesDTOByHostIdAsync(ApplicationUser.Id);
                if (UserMatches.Count() != 0)
                {
                    await GetBoardgameModelDTOs(UserMatches);
                    await GetUsersDTOs(UserMatches);
                }
            }

            if (Filter == "Started")
            {
                UserMatches = await _matchStore.FindAllStartedMatchesDTOByHostIdAsync(ApplicationUser.Id);
                if (UserMatches.Count() != 0)
                {
                    await GetBoardgameModelDTOs(UserMatches);
                    await GetUsersDTOs(UserMatches);
                }
            }

            return Page();
        }

        private async Task GetBoardgameModelDTOs(IEnumerable<BasicMatchInfoDTO> matches)
        {
            foreach (var match in matches)
            {
                BoardgameDTO boardgame = _mapper.Map<BoardgameDTO>(await _boardgameStore.FindBoardGameByIdAsync(match.BoardgameId));
                BoardgameList.Add(boardgame);
            }
        }

        private async Task GetUsersDTOs(IEnumerable<BasicMatchInfoDTO> matches)
        {
            foreach (var match in matches)
            {
                BasicUserInfoDTO user = _mapper.Map<BasicUserInfoDTO>(await _userManager.FindByIdAsync(match.HostId));
                UserList.Add(user);
            }
        }

        public IActionResult OnPostShowUpcoming()
        {
            return RedirectToPage("ShowUserMatches", new { Filter = "Upcoming" });
        }

        public IActionResult OnPostShowOngoing()
        {
            return RedirectToPage("ShowUserMatches", new { Filter = "Started" });
        }
    }
}