using MailKit;

namespace BoardGameBrawl.Data
{
    public class BoardgameClaim
    {
        public string ClaimType = "BoardGameModerationPermission";
        
        public int? BoardGameId { get; set; }
    }
}