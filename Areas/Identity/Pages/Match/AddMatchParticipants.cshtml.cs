#nullable disable
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class AddMatchParticipantsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IMapper _mapper;
        
        public AddMatchParticipantsModel(UserManager<ApplicationUser> userManager,
            IUserFriendStore<UserFriend, ApplicationUser> userFriendStore,
            ApplicationDbContext context,
            AutoMapper.IConfigurationProvider configurationProvider,
            IMapper mapper)
        {
            _userManager = userManager;
            _userFriendStore = userFriendStore;
            _context = context;
            _configurationProvider = configurationProvider;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<BasicUserInfoDTO> UserFriends { get; set; } = new List<BasicUserInfoDTO>();

        public IEnumerable<BasicUserInfoDTO> FilteredUsers { get; set; } = new List<BasicUserInfoDTO>();

        // Map to SelectItemsObject

        public List<SelectListItem> Select_UserFriends { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Select_FilteredUsers { get; set; } = new List<SelectListItem>();

        // Map to listed result 

        [BindProperty]
        public List<string> Result { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserFriends = await _userFriendStore.GetUserFriendsAsync(ApplicationUser.Id);
            if (UserFriends.Count() != 0)
            {
                Select_UserFriends = UserFriends.Select(u => new SelectListItem { Value = u.DTOUsername, Text = u.DTOUsername }).ToList();
            }

            if(!Filter.IsNullOrEmpty())
            {
                FilteredUsers = await GetFilteredUsersAsync(Filter);
                if (FilteredUsers.Count() != 0)
                {
                    Select_FilteredUsers = FilteredUsers.Select(u => new SelectListItem { Value = u.DTOUsername, Text = u.DTOUsername }).ToList();
                }
            }
            return Page();
        }

        private async Task<IEnumerable<BasicUserInfoDTO>> GetFilteredUsersAsync(string filter)
        {
            BasicUserInfoDTO AppUserDTO = _mapper.Map<BasicUserInfoDTO>(await _userManager.GetUserAsync(User));
            var filteredUsers = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserName.Contains(filter))
                .ProjectTo<BasicUserInfoDTO>(_configurationProvider)
                .ToListAsync();

            if (filteredUsers.Contains(AppUserDTO))
            {
                int index = filteredUsers.FindIndex(u => u.DTOUserID == AppUserDTO.DTOUserID);
                return filteredUsers.Where((v, i) => i != index).ToList();
            }
            else
            {
                return filteredUsers;
            }
        }

        public IActionResult OnPostSearchUsers()
        {
            return RedirectToPage("AddMatchParticipants", new { Filter });
        }

        public async Task<IActionResult> OnPostAddFriendsToParticipantListAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // checking if already some users has been invited
            if (HttpContext.Session.GetString("Chosen_Participants") != null)
            {
                string participants = HttpContext.Session.GetString("Chosen_Participants");
                List<string> participantsList = participants.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                // add entities to already existing invated users
                participantsList.AddRange(Result);

                // prepare string for a new session string value
                string sessionString = String.Empty;
                for (int i = 0; i < participantsList.Count; i++)
                {
                    sessionString += participantsList.ElementAt(i) + ",";
                    if (i == participantsList.Count - 1)
                        sessionString += participantsList.ElementAt(i);
                }

                //clear session 
                HttpContext.Session.Remove("Chosen_Participants");

                //add new value to session store
                HttpContext.Session.SetString("Chosen_Participants", sessionString);
            }
            else
            {
                // if session store never set

                string sessionString = String.Empty;
                for (int i = 0; i < Result.Count; i++)
                {
                    sessionString += Result.ElementAt(i) + ",";
                    if (i == Result.Count - 1)
                        sessionString += Result.ElementAt(i);
                }

                //add new value to session store
                HttpContext.Session.SetString("Chosen_Participants", sessionString);
            }
            return RedirectToPage("CreateMatch");
        }

        public async Task<IActionResult> OnPostAddUsersToParticipantListAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // checking if already some users has been invited
            if (HttpContext.Session.GetString("Chosen_Participants") != null)
            {
                string participants = HttpContext.Session.GetString("Chosen_Participants");
                List<string> participantsList = participants.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                // add entities to already existing invated users
                participantsList.AddRange(Result);

                // prepare string for a new session string value
                string sessionString = String.Empty;
                for (int i = 0; i < participantsList.Count; i++)
                {
                    if (i == participantsList.Count - 1)
                        sessionString += participantsList.ElementAt(i);
                    else
                        sessionString += participantsList.ElementAt(i) + ",";
                }

                //clear session 
                HttpContext.Session.Remove("Chosen_Participants");

                //add new value to session store
                HttpContext.Session.SetString("Chosen_Participants", sessionString);
            }
            else
            {
                // if session store never set
                
                string sessionString = String.Empty;
                for (int i = 0; i < Result.Count; i++)
                {
                    if (i == Result.Count - 1)
                        sessionString += Result.ElementAt(i);
                    else 
                        sessionString += Result.ElementAt(i) + ",";
                }
                
                //add new value to session store
                HttpContext.Session.SetString("Chosen_Participants", sessionString);
            }
            return RedirectToPage("CreateMatch");
        }
    }
}