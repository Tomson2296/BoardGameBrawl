using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Crypto.Paddings;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class UserGeolocation
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        
        public ApplicationUser? User { get; set; }

        public string? City { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; } 

        public byte[]? GeolocationImage { get; set; }
    }
}
