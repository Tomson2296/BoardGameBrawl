using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    /// <summary>
    /// DTO Object used in finder panel in order to ger an User ID, Username, Email, ImageAvatar
    /// </summary>

    [AutoMap(typeof(ApplicationUser))]
    public class BasicUserInfoDTO
    {
        [SourceMember(nameof(ApplicationUser.Id))]
        public string? DTOUserID { get; set; }

        [SourceMember(nameof(ApplicationUser.UserName))]
        public string? DTOUsername { get; set; }

        [SourceMember(nameof(ApplicationUser.Email))]
        public string? DTOUserEmail { get; set; }

        [SourceMember(nameof(ApplicationUser.UserAvatar))]
        public byte[]? DTOUserAvatar { get; set; }
    }
}
