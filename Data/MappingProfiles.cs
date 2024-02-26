using AutoMapper;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<ApplicationUser, ViewUserDTO>()
               .ForMember(dest => dest.DTOUserID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.DTOUsername, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.DTOUserEmail, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();

            CreateMap<ApplicationUser, LockoutUserDTO>()
               .ForMember(dest => dest.DTOUserID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.DTOUsername, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.DTOUserEmail, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.LockOutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
            .ReverseMap();

            CreateMap<ApplicationUser, BasicUserInfoDTO>()
              .ForMember(dest => dest.DTOUserID, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.DTOUsername, opt => opt.MapFrom(src => src.UserName))
              .ForMember(dest => dest.DTOUserEmail, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.DTOUserAvatar, opt => opt.MapFrom(src => src.UserAvatar))
           .ReverseMap();

            CreateMap<GroupModel, GroupInfoDTO>()
              .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
              .ForMember(dest => dest.GroupDesc, opt => opt.MapFrom(src => src.GroupDesc))
              .ForMember(dest => dest.GroupMiniature, opt => opt.MapFrom(src => src.GroupMiniature))
           .ReverseMap();

            CreateMap<MatchmakingRule, MatchmakingRuleDTO>()
              .ForMember(dest => dest.RuleId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.RuleDesc, opt => opt.MapFrom(src => src.RuleDescription))
              .ForMember(dest => dest.RuleDecider, opt => opt.MapFrom(src => src.RuleDecider))
              .ForMember(dest => dest.RuleType, opt => opt.MapFrom(src => src.RuleType))
           .ReverseMap();

            CreateMap<BoardgameModel, BoardgameDTO>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.BGGId, opt => opt.MapFrom(src => src.BGGId))
              .ForMember(dest => dest.YearPublished, opt => opt.MapFrom(src => src.YearPublished))
              .ForMember(dest => dest.BoardgamePic, opt => opt.MapFrom(src => src.ImageFile))
           .ReverseMap();

            CreateMap<MatchModel, BasicMatchInfoDTO>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.BoardgameId, opt => opt.MapFrom(src => src.BoardgameId))
             .ForMember(dest => dest.HostId, opt => opt.MapFrom(src => src.MatchHostId))
             .ForMember(dest => dest.MatchCreated, opt => opt.MapFrom(src => src.MatchCreated))
             .ForMember(dest => dest.MatchDate_Start, opt => opt.MapFrom(src => src.MatchDate_Start))
             .ForMember(dest => dest.MatchDate_End, opt => opt.MapFrom(src => src.MatchDate_End))
             .ForMember(dest => dest.NumberOfPlayers, opt => opt.MapFrom(src => src.NumberOfPlayers))
             .ForMember(dest => dest.MatchProgress, opt => opt.MapFrom(src => src.MatchProgress))
             .ForMember(dest => dest.Location_Latitude, opt => opt.MapFrom(src => src.Location_Latitude))
             .ForMember(dest => dest.Location_Longitude, opt => opt.MapFrom(src => src.Location_Longitude))
            .ReverseMap();
        }
    }
}
