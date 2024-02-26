#nullable disable
using AutoMapper;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class CreateMatchModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;
        private readonly IMatchStore<MatchModel, BoardgameModel, ApplicationUser> _matchStore;
        private readonly INominatimGeolocationAPI _nominatimGeolocationService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memory;
            
        private readonly IGoogleStaticMapsService _googleStaticMapsService;
        public CreateMatchModel(UserManager<ApplicationUser> userManager,
            IBoardGameStore<BoardgameModel> boardGameStore,
            IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore,
            INominatimGeolocationAPI nominatimGeolocationAPI,
            IGoogleStaticMapsService googleStaticMapsService,
            IMapper mapper,
            IMemoryCache memory)
        {
            _userManager = userManager;
            _boardgameStore = boardGameStore;
            _matchStore = matchStore;
            _nominatimGeolocationService = nominatimGeolocationAPI;
            _googleStaticMapsService = googleStaticMapsService;
            _mapper = mapper;
            _memory = memory;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        // location data 
        public List<string> LocationData { get; set; } = new List<string>();

        public byte[] ImageData { get; set; }
        
        public BoardgameModel Chosen_Boardgame { get; set; }

        public DateTime Chosen_StartingDate { get; set; }

        public IList<BasicUserInfoDTO> Match_Participants { get; set; } = new List<BasicUserInfoDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Add ApplicationUser as a first match participant - when session data is empty
            if (HttpContext.Session.GetString("Chosen_Participants") == null)
            {
                HttpContext.Session.SetString("Chosen_Participants", ApplicationUser.UserName);
            }

            // checking data from session storage about set boardgame, users, starting date and geolocation data
            if (HttpContext.Session.GetString("Chosen_Boardgame") != null)
            {
                Chosen_Boardgame = await _boardgameStore.FindBoardGameByIdAsync(HttpContext.Session.GetString("Chosen_Boardgame"));
            }

            if (HttpContext.Session.GetString("Chosen_StartingDate") != null)
            {
                string startingDate = HttpContext.Session.GetString("Chosen_StartingDate");
                DateTime dateTime = new();
                DateTime.TryParse(startingDate, out dateTime);
                Chosen_StartingDate = dateTime;
            }

            if (HttpContext.Session.GetString("Chosen_Participants") != null)
            {
                string participants = HttpContext.Session.GetString("Chosen_Participants");
                List<string> participantsList = participants.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                foreach (string participant in participantsList)
                {
                    BasicUserInfoDTO user = _mapper.Map<BasicUserInfoDTO>(await _userManager.FindByNameAsync(participant));
                    Match_Participants.Add(user);
                }
            }

            if (HttpContext.Session.GetString("Chosen_Location") != null)
            {
                LocationData.Add(HttpContext.Session.GetString("Chosen_Location").Split(",")[0]);
                LocationData.Add(HttpContext.Session.GetString("Chosen_Location").Split(",")[1]);
            }

            if (_memory.TryGetValue("Chosen_Location_Image", out byte[] imageData))
            {
                ImageData = imageData;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateMatchAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            int setCount = 0;

            if (HttpContext.Session.GetString("Chosen_Boardgame") != null)
                setCount++;
            if (HttpContext.Session.GetString("Chosen_StartingDate") != null)
                setCount++;
            if (HttpContext.Session.GetString("Chosen_Participants") != null)
                setCount++;
            if (HttpContext.Session.GetString("Chosen_Location") != null)
                setCount++;

            if (setCount != 4)
            {
                StatusMessage = "Error - not everything has been set. Set all parameters to proceed.";
                return Page();
            }
            else
            {
                BoardgameModel boardgame = await _boardgameStore.FindBoardGameByIdAsync(HttpContext.Session.GetString("Chosen_Boardgame"));

                string participants = HttpContext.Session.GetString("Chosen_Participants");
                List<string> participantsList = participants.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                int participantsCount = participantsList.Count;

                // checking in number of players is in appropriate range
                if (participantsCount < boardgame.MinPlayers || participantsCount > boardgame.MaxPlayers)
                {
                    StatusMessage = "Error - inadequate number of players assigned to that game. Try again later";
                    return Page();
                }
                else
                {
                    // everything set

                    MatchModel matchModel = CreateMatch();
                    await _matchStore.SetMatchHostUserAsync(matchModel, ApplicationUser);
                    await _matchStore.SetMatchBoardgameAsync(matchModel, boardgame);
                    await _matchStore.SetMatchCreatedDateAsync(matchModel, DateTime.Now);

                    await _matchStore.SetMatchParticipaintsAsync(matchModel, participantsList);
                    await _matchStore.SetMatchNumberOfPlayersAsync(matchModel, participantsCount);

                    await _matchStore.SetMatchLocationLatitudeAsync(matchModel, HttpContext.Session.GetString("Chosen_Location").Split(",")[0]);
                    await _matchStore.SetMatchLocationLongitudeAsync(matchModel, HttpContext.Session.GetString("Chosen_Location").Split(",")[1]);

                    if (_memory.TryGetValue("Chosen_Location_Image", out byte[] imageData))
                        await _matchStore.SetMatchLocationImageAsync(matchModel, imageData);
                    
                    string startingDate = HttpContext.Session.GetString("Chosen_StartingDate");
                    DateTime dateTime = new();
                    DateTime.TryParse(startingDate, out dateTime);
                    await _matchStore.SetMatchStartDateAsync(matchModel, dateTime);
                    
                    await _matchStore.SetMatchProgressInfoAsync(matchModel, MatchProgress.Upcoming);
                    
                    IdentityResult addMatchModel = await _matchStore.CreateMatchAsync(matchModel);

                    if (addMatchModel.Succeeded)
                    {
                        StatusMessage = "Match has been successfully added.";
                        // reset all imputs and refresh the page
                        ResetMatchParameters();
                        return RedirectToPage();
                    }
                    else
                    {
                        StatusMessage = "Error during process of adding match to database. Try again later";
                        foreach (var error in addMatchModel.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                            return Page();
                        }
                    } 
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAbortCreationAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            // clean all information in session storage and in memory cache 

            if (HttpContext.Session.GetString("Chosen_Boardgame") != null)
            {
                HttpContext.Session.Remove("Chosen_Boardgame");
            }

            if (HttpContext.Session.GetString("Chosen_StartingDate") != null)
            {
                HttpContext.Session.Remove("Chosen_StartingDate");
            }

            if (HttpContext.Session.GetString("Chosen_Participants") != null)
            {
                HttpContext.Session.Remove("Chosen_Participants");

                // Add ApplicationUser as a first match participant
                HttpContext.Session.SetString("Chosen_Participants", ApplicationUser.UserName);
            }

            if (HttpContext.Session.GetString("Chosen_Location") != null)
            {
                HttpContext.Session.Remove("Chosen_Location");
            }

            if (_memory.Get("Chosen_Location_Image") != null)
            {
                _memory.Remove("Chosen_Location_Image");
            }

            return RedirectToPage();
        }

        private MatchModel CreateMatch()
        {
            try
            {
                return Activator.CreateInstance<MatchModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(MatchModel)}'.");
            }
        }

        private void ResetMatchParameters()
        {
            if (HttpContext.Session.GetString("Chosen_Boardgame") != null)
            {
                HttpContext.Session.Remove("Chosen_Boardgame");
            }

            if (HttpContext.Session.GetString("Chosen_StartingDate") != null)
            {
                HttpContext.Session.Remove("Chosen_StartingDate");
            }

            if (HttpContext.Session.GetString("Chosen_Participants") != null)
            {
                HttpContext.Session.Remove("Chosen_Participants");
            }

            if (HttpContext.Session.GetString("Chosen_Location") != null)
            {
                HttpContext.Session.Remove("Chosen_Location");
            }

            if (_memory.Get("Chosen_Location_Image") != null)
            {
                _memory.Remove("Chosen_Location_Image");
            }
        }
    }
}
