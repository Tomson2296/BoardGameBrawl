#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class AddStartingDateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AddStartingDateModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Chosen_StartingDate { get; set; }

        [BindProperty]
        public string Chosen_Hour { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

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
            ApplicationUser user = await _userManager.GetUserAsync(User);
            string startingDate = Request.Form["ChosenStartingDate"];
            string startingHour = Request.Form["ChosenHour"];
            string concatenateValue = startingDate + " " + startingHour;
            HttpContext.Session.SetString("Chosen_StartingDate", concatenateValue);
            return RedirectToPage("CreateMatch");
        }
    }
}