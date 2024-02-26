#nullable disable

using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BoardGameBrawl.Areas.Identity.Pages.Account
{
    public class FinderModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration;

        public FinderModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            AutoMapper.IConfigurationProvider configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration; 
        }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        public IEnumerable<BasicUserInfoDTO> Users { get; set; } = new List<BasicUserInfoDTO>();

        public IEnumerable<BoardgameDTO> Boardgames { get; set; } = new List<BoardgameDTO>();

        public IEnumerable<GroupInfoDTO> Groups { get; set; } = new List<GroupInfoDTO>();


        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!Category.IsNullOrEmpty())
            {
                switch (Category)
                {
                    case "User":
                        Users = await _userManager.Users
                            .AsNoTracking()
                            .Where(u => u.UserName.Contains(Filter))
                            .ProjectTo<BasicUserInfoDTO>(_configuration)
                            .ToListAsync();
                        break;
                    case "Boardgame":
                        Boardgames = await _context.Boardgames
                            .AsNoTracking()
                            .Where(b => b.Name.Contains(Filter))
                            .ProjectTo<BoardgameDTO>(_configuration)
                            .ToListAsync();
                        break;
                    case "Group":
                        Groups = await _context.Groups
                            .AsNoTracking()
                            .Where(g => g.GroupName.Contains(Filter))
                            .ProjectTo<GroupInfoDTO>(_configuration)
                            .ToListAsync();
                        break;
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Finder", new { Filter, Category });
        }
    }
}
