using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    /// <summary>
    /// DTO Object used to show only information about the group - without list of participants
    /// </summary>
    /// 

    [AutoMap(typeof(GroupModel))]
    public class GroupInfoDTO
    {
        [SourceMember(nameof(GroupModel.Id))]
        public string? GroupID { get; set; }

        [SourceMember(nameof(GroupModel.GroupName))]
        public string? GroupName { get; set; }

        [SourceMember(nameof(GroupModel.GroupDesc))]
        public string? GroupDesc { get; set; }

        [SourceMember(nameof(GroupModel.GroupMiniature))]
        public byte[]? GroupMiniature { get; set; }
    }
}
