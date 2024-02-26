using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class UserRating
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Boardgame))]
        public string? BoardgameId { get; set; }

        public BoardgameModel? Boardgame { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        
        public ApplicationUser? User { get; set; }

        public int Rating { get; set; }
    }
}