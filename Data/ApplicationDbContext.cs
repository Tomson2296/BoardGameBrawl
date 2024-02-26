using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            //
            // ApplicationUser table fields List<string> to string connventions
            //

            builder.Entity<ApplicationUser>()
           .Property(e => e.User_FavouriteBoardgames)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());


            //
            // UserSchedule table fields List<string> to string connventions
            //

            builder.Entity<UserSchedule>()
           .Property(e => e.Monday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<UserSchedule>()
           .Property(e => e.Tuesday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<UserSchedule>()
           .Property(e => e.Wednesday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<UserSchedule>()
           .Property(e => e.Thursday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<UserSchedule>()
           .Property(e => e.Friday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<UserSchedule>()
           .Property(e => e.Saturday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<UserSchedule>()
           .Property(e => e.Sunday)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            //
            // BoardgameModel table fields List<string> to string connventions
            //

            builder.Entity<BoardgameModel>()
            .Property(e => e.Boardgame_Categories)
            .HasConversion(new StringListConverter())
            .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<BoardgameModel>()
           .Property(e => e.Boardgame_Mechanics)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            //
            // Match table fields List<string> to string connventions
            //

            builder.Entity<MatchModel>()
            .Property(e => e.Match_Participants)
            .HasConversion(new StringListConverter())
            .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<MatchModel>()
           .Property(e => e.Match_Ruleset)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            builder.Entity<MatchModel>()
           .Property(e => e.Match_Results)
           .HasConversion(new StringListConverter())
           .Metadata.SetValueComparer(new StringListComparer());

            //
            // BoardgameRule configuration - primary keyless juntio table
            //

            builder.Entity<BoardgameRule>()
            .HasKey(br => new { br.BoardgameId, br.MatchmakingRuleId });

            builder.Entity<BoardgameRule>()
                .HasOne(br => br.Boardgame)
                .WithMany(t => t.BoardgameRules_Boardgame)
                .HasForeignKey(br => br.BoardgameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BoardgameRule>()
                .HasOne(br => br.MatchmakingRule)
                .WithMany(m => m.BoardgameRules_MatchmakingRule)
                .HasForeignKey(br => br.MatchmakingRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            //
            // TournamentMatch configuration - primary keyless junction table 
            //

            builder.Entity<TournamentMatch>()
             .HasKey(tm => new { tm.TournamentId, tm.MatchId });

            builder.Entity<TournamentMatch>()
                .HasOne(tm => tm.Tournament)
                .WithMany(t => t.TournamentMatches_Tournament)
                .HasForeignKey(tm => tm.TournamentId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<TournamentMatch>()
                .HasOne(tm => tm.Match)
                .WithMany(m => m.TournamentMatches_Match)
                .HasForeignKey(tm => tm.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            //
            // TournamentParticipant configuration - primary keyless junction table 
            //

            builder.Entity<TournamentParticipant>()
             .HasKey(tp => new { tp.TournamentId, tp.ParticipantId });

            builder.Entity<TournamentParticipant>()
                .HasOne(tp => tp.Tournament)
                .WithMany(t => t.TournamentParticipants_Tournament)
                .HasForeignKey(tp => tp.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TournamentParticipant>()
                .HasOne(tp => tp.Participant)
                .WithMany(u => u.TournamentParticipants_Participant)
                .HasForeignKey(tp => tp.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public virtual DbSet<BoardgameModel> Boardgames { get; set; }
       
        public virtual DbSet<MatchmakingRule> MatchmakingRules { get; set; }
        
        public virtual DbSet<BoardgameRule> BoardgameRules { get; set; }
        
        public virtual DbSet<MatchModel> Matches { get; set; }
     
        public virtual DbSet<MessageModel> Messages { get; set; }

        public virtual DbSet<UserSchedule> UserSchedules { get; set; }

        public virtual DbSet<UserGeolocation> UserGeolocations { get; set; }

        public virtual DbSet<UserFriend> UserFriends { get; set; }
        
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        
        public virtual DbSet<UserRating> UserRatings { get; set; }

        public virtual DbSet<GroupModel> Groups { get; set; }

        public virtual DbSet<GroupParticipant> GroupParticipants { get; set; }
        
        public virtual DbSet<Tournament> Tournaments { get; set; }

        public virtual DbSet<TournamentMatch> TournamentMatches { get; set; }
        
        public virtual DbSet<TournamentParticipant> TournamentParticipants { get; set; }
    }
}