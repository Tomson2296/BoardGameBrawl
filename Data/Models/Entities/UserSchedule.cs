using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class UserSchedule
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public List<string>? Monday { get; set; } = new List<string>();
       
        public List<string>? Tuesday { get; set; } = new List<string>();

        public List<string>? Wednesday { get; set; } = new List<string>();

        public List<string>? Thursday { get; set; } = new List<string>();

        public List<string>? Friday { get; set; } = new List<string>();

        public List<string>? Saturday { get; set; } = new List<string>();

        public List<string>? Sunday { get; set; } = new List<string>();
    }
}