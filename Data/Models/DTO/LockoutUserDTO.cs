using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    [AutoMap(typeof(ApplicationUser))]
    public class LockoutUserDTO
    {
        [SourceMember(nameof(ApplicationUser.Id))]
        public string? DTOUserID { get; set; }

        [SourceMember(nameof(ApplicationUser.UserName))]
        public string? DTOUsername { get; set; }

        [SourceMember(nameof(ApplicationUser.Email))]
        public string? DTOUserEmail { get; set; }

        [SourceMember(nameof(ApplicationUser.LockoutEnd))]
        public DateTimeOffset? LockOutEnd { get; set; }
    }
}
