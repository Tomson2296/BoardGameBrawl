using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class MatchModel
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Boardgame))]
        public string? BoardgameId { get; set; }
        
        public BoardgameModel? Boardgame { get; set; }

        [ForeignKey(nameof(MatchHost))]
        public string? MatchHostId { get; set; }

        public ApplicationUser? MatchHost { get; set; }

        public DateTime MatchCreated { get; set; }
        
        public DateTime MatchDate_Start { get; set; }

        public DateTime MatchDate_End { get; set; }

        public int NumberOfPlayers { get; set; }

        public MatchProgress MatchProgress { get; set; }

        public List<string>? Match_Participants { get; set; }

        public List<string>? Match_Ruleset { get; set; }
        
        public List<string>? Match_Results { get; set; }

        //
        // location fields
        //
        
        public string? Location_Latitude { get; set; }

        public string? Location_Longitude { get; set; }

        public byte[]? Location_Image { get; set; }

        [InverseProperty(nameof(TournamentMatch.Match))]
        public ICollection<TournamentMatch>? TournamentMatches_Match { get; set; }
    }
    public enum MatchProgress
    {
        Upcoming,
        Started,
        Finished
    }
}