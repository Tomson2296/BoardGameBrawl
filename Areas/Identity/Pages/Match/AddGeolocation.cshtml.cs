#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class AddGeolocationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMatchStore<MatchModel, BoardgameModel, ApplicationUser> _matchStore;
        private readonly INominatimGeolocationAPI _nominatimGeolocationAPI;
        private readonly IGoogleStaticMapsService _googleStaticMapsService;
        private readonly IMemoryCache _memory;
        private readonly ILogger<AddGeolocationModel> _logger;

        public AddGeolocationModel(UserManager<ApplicationUser> userManager, 
            IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore, 
            INominatimGeolocationAPI nominatimGeolocationAPI, 
            IGoogleStaticMapsService googleStaticMapsService,
            IMemoryCache memory,
            ILogger<AddGeolocationModel> logger)
        {
            _userManager = userManager;
            _matchStore = matchStore;
            _nominatimGeolocationAPI = nominatimGeolocationAPI;
            _googleStaticMapsService = googleStaticMapsService;
            _memory = memory;
            _logger = logger;
        }

        public List<string> LocationData { get; set; } = new List<string>();

        public byte[] ImageData { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Street")]
            public string Street { get; set; }

            [Required]
            [Display(Name = "City")]
            public string City { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (HttpContext.Session.GetString("Chosen_Location") != null)
            {
                LocationData.Add(HttpContext.Session.GetString("Chosen_Location").Split(",")[0]);
                LocationData.Add(HttpContext.Session.GetString("Chosen_Location").Split(",")[1]);
            }

            if (_memory.TryGetValue("Chosen_Location_Image", out byte[] imageData))
            {
                _logger.Log(LogLevel.Information, "ImageData found in cache");
                ImageData = imageData;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            GeolocationData geolocationData = new();

            // get information from external API about localization (latitude and longitude) 
            if (!Input.Street.IsNullOrEmpty())
                geolocationData = await _nominatimGeolocationAPI.GetGeolocationDataByStreetNameAsync(Input.Street, Input.City);
            else
                geolocationData = await _nominatimGeolocationAPI.GetGeolocationDataByCityNameAsync(Input.City);

            LocationData.Add(geolocationData.Place.Latitude);
            LocationData.Add(geolocationData.Place.Longitude);
            ImageData = await _googleStaticMapsService.GetStaticMapUsingGeolocationDataAsync(geolocationData.Place.Latitude,
                geolocationData.Place.Longitude, "14", "600", "300");

            // If already something exists in cache memory - clear it and create another entity 
            if (_memory.Get("Chosen_Location_Image") != null)
            {
                _memory.Remove("Chosen_Location_Image");

                var memoryEntryOptions = new MemoryCacheEntryOptions()
                      .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                _memory.Set("Chosen_Location_Image", ImageData, memoryEntryOptions);
            }
            else
            {
                // save ImageData in cache memory to prevent nasty consequences of innefective binary convertions
                // setting memoty cache options

                var memoryEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);

                _memory.Set("Chosen_Location_Image", ImageData, memoryEntryOptions);
            }

            // resseting values saved in Chosen_Location element in session storage - if not exist - set values in session storage
            if (HttpContext.Session.GetString("Chosen_Location") != null)
            {
                HttpContext.Session.Remove("Chosen_Location");
                string concatenateValue = LocationData.ElementAt(0) + "," + LocationData.ElementAt(1);
                HttpContext.Session.SetString("Chosen_Location", concatenateValue);
            }
            else
            {
                string concatenateValue = LocationData.ElementAt(0) + "," + LocationData.ElementAt(1);
                HttpContext.Session.SetString("Chosen_Location", concatenateValue);
            }

            return RedirectToPage();
        }
    }
}