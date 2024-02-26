using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data.Models.DTO
{
    [AutoMap(typeof(BoardgameModel))]
    public class BoardgameDTO
    {
        [SourceMember(nameof(BoardgameModel.Id))]
        public string? Id { get; set; }

        [SourceMember(nameof(BoardgameModel.Name))]
        public string? Name { get; set; }

        [SourceMember(nameof(BoardgameModel.BGGId))]
        public int BGGId { get; set; }

        [SourceMember(nameof(BoardgameModel.ImageFile))]
        public string? BoardgamePic { get; set; }

        [SourceMember(nameof(BoardgameModel.YearPublished))]
        public int YearPublished { get; set; }
    }
}
