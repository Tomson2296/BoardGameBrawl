using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities;

public partial class BoardgameModel
{
    [Key]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(256)]
    public string? Name { get; set; }

    public int BGGId { get; set; }

    public byte MinPlayers { get; set; }

    public byte MaxPlayers { get; set; }

    public int YearPublished { get; set; }

    public int PlayingTime { get; set; }

    [MaxLength(256)]
    public string? ImageFile { get; set; }

    public List<string>? Boardgame_Categories { get; set; }

    public List<string>? Boardgame_Mechanics { get; set; }


    [InverseProperty(nameof(Tournament.Boardgame))]
    public ICollection<Tournament>? Tournaments_Boardgame { get; set; }

    [InverseProperty(nameof(MatchModel.Boardgame))]
    public ICollection<MatchModel>? Matches_Boardgame { get; set; }

    [InverseProperty(nameof(UserRating.Boardgame))]
    public ICollection<UserRating>? UserRatings_Boardgame { get; set; }

    [InverseProperty(nameof(BoardgameRule.Boardgame))]
    public ICollection<BoardgameRule>? BoardgameRules_Boardgame { get; set; }
}