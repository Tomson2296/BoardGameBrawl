using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    /// <summary>
    /// DTO class for the purpose of mapping only essential info about users  (ID, UserName, Email) 
    /// Used mainly in administrator panel to show users in tables
    /// </summary>
    /// 
    [AutoMap(typeof(ApplicationUser))]
    public class ViewUserDTO
    {
        [SourceMember(nameof(ApplicationUser.Id))]
        public string? DTOUserID { get; set; }

        [SourceMember(nameof(ApplicationUser.UserName))]
        public string? DTOUsername { get; set; }

        [SourceMember(nameof(ApplicationUser.Email))]
        public string? DTOUserEmail { get; set; }
    }
}
