using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class Tournament
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public TournamentType Type { get; set; }

        public byte MaxNumberOfPlayers { get; set; }

        public DateTime CreationTime { get; set; }

        [ForeignKey(nameof(Boardgame))]
        public string? BoardgameId { get; set; }

        public BoardgameModel? Boardgame { get; set; }

        [InverseProperty(nameof(TournamentParticipant.Tournament))]
        public ICollection<TournamentParticipant>? TournamentParticipants_Tournament { get; set; }

        [InverseProperty(nameof(TournamentMatch.Tournament))]
        public ICollection<TournamentMatch>? TournamentMatches_Tournament { get; set; }
    }

    public enum TournamentType
    {
        SingleElimination,
        DoubleElimination,
        Swiss
    }
}
