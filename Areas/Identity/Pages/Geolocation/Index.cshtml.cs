#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.Geolocation
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserGeolocationStore<UserGeolocation, ApplicationUser> _userGeolocationStore;
        private readonly INominatimGeolocationAPI _nominatimGeolocationAPI;
        private readonly IGoogleStaticMapsService _googleStaticMapsService;

        public IndexModel(UserManager<ApplicationUser> userManager, IUserGeolocationStore<UserGeolocation, ApplicationUser> userGeolocationStore, INominatimGeolocationAPI nominatimGeolocationAPI, IGoogleStaticMapsService googleStaticMapsService)
        {
            _userManager = userManager;
            _userGeolocationStore = userGeolocationStore;
            _nominatimGeolocationAPI = nominatimGeolocationAPI;
            _googleStaticMapsService = googleStaticMapsService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public UserGeolocation UserGeolocation { get; set; }

        [BindProperty]
        public byte[] ImageData { get; set; }

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

            if (await _userGeolocationStore.CheckIfUserEverProvidedDataAsync(ApplicationUser) == true)
            {
                UserGeolocation = await _userGeolocationStore.FindGeolocationDataByUserIdAsync(ApplicationUser.Id);
                ImageData = UserGeolocation.GeolocationImage;
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

            byte[] imageData = await _googleStaticMapsService.GetStaticMapUsingGeolocationDataAsync(geolocationData.Place.Latitude,
                geolocationData.Place.Longitude, "14", "600", "300");

            // checking whether calling to external API cause any kind of error and not producing a response 200 code 
            if (geolocationData != null)
            {
                UserGeolocation userGeolocation = CreateUserGeolocationModel();
                await _userGeolocationStore.SetCityAsync(userGeolocation, Input.City);
                await _userGeolocationStore.SetLatitudeAsync(userGeolocation, geolocationData.Place.Latitude);
                await _userGeolocationStore.SetLongitudeAsync(userGeolocation, geolocationData.Place.Longitude);
                await _userGeolocationStore.SetUserByAsync(userGeolocation, user);

                if (!imageData.IsNullOrEmpty())
                    await _userGeolocationStore.SetGeolocationImageAsync(userGeolocation, imageData);

                // checking if already those geolocationData exist in database 
                bool ifTheSame = await _userGeolocationStore.CheckIfValuesProvidedAreTheSame(userGeolocation, user);

                if (ifTheSame)
                {
                    StatusMessage = "Error - Entered Geolocation data exists in database.";
                    return Page();
                }

                IdentityResult newGeolocationDataAdded = await _userGeolocationStore.CreateGeolocationDataAsync(userGeolocation);
                if (newGeolocationDataAdded.Succeeded)
                {
                    StatusMessage = "New Geolocation Data has been added";
                    return RedirectToPage();
                }
                else
                {
                    foreach (var error in newGeolocationDataAdded.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    StatusMessage = "Error - New Geolocation data has not been added. Try again later";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Error during getting geolocation data from external API. Try again later";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostClear()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            UserGeolocation userGeolocation = UserGeolocation = await _userGeolocationStore.FindGeolocationDataByUserIdAsync(user.Id);

            if (userGeolocation != null)
            {
                IdentityResult clearUserGeolocationData = await _userGeolocationStore.DeleteGeolocationDataAsync(userGeolocation);
                if (clearUserGeolocationData.Succeeded)
                {
                    StatusMessage = "User Geolocation data has been cleared.";
                    return RedirectToPage();
                }
                else
                {
                    foreach (var error in clearUserGeolocationData.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    StatusMessage = "Error - New Geolocation data has not been cleared. Try again later";
                    return Page();
                }
            }
            return Page();
        }

        private UserGeolocation CreateUserGeolocationModel()
        {
            try
            {
                return Activator.CreateInstance<UserGeolocation>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserGeolocation)}'.");
            }
        }
    }
}
