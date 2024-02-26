using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    [AutoMap(typeof(MatchModel))]
    public class BasicMatchInfoDTO
    {
        [SourceMember(nameof(MatchModel.Id))]
        public string? Id { get; set; }

        [SourceMember(nameof(MatchModel.BoardgameId))]
        public string? BoardgameId { get; set; }

        [SourceMember(nameof(MatchModel.MatchHostId))]
        public string? HostId { get; set; }

        [SourceMember(nameof(MatchModel.MatchCreated))]
        public DateTime MatchCreated { get; set; }

        [SourceMember(nameof(MatchModel.MatchDate_Start))]
        public DateTime MatchDate_Start { get; set; }

        [SourceMember(nameof(MatchModel.MatchDate_End))]
        public DateTime MatchDate_End { get; set; }

        [SourceMember(nameof(MatchModel.NumberOfPlayers))]
        public int NumberOfPlayers { get; set; }

        [SourceMember(nameof(MatchModel.MatchProgress))]
        public MatchProgress MatchProgress { get; set; }

        [SourceMember(nameof(MatchModel.Location_Latitude))]
        public string? Location_Latitude { get; set; }

        [SourceMember(nameof(MatchModel.Location_Longitude))]
        public string? Location_Longitude { get; set; }
    }
}
