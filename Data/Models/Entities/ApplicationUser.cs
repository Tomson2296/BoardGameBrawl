using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameBrawl.Data.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(128)]
        public string? FirstName { get; set; }

        [MaxLength(128)]
        public string? LastName { get; set; }

        [MaxLength(256)]
        public string? BGGUsername { get; set; }

        [MaxLength(500)]
        public string? UserDescription { get; set; }

        public DateOnly UserCreatedTime { get; set; }

        public DateOnly UserLastLogin { get; set; }

        public byte[]? UserAvatar { get; set; }

        public List<string>? User_FavouriteBoardgames { get; set; }


        [InverseProperty(nameof(UserSchedule.User))]
        public UserSchedule? UserSchedule { get; set; }

        [InverseProperty(nameof(UserGeolocation.User))]
        public UserGeolocation? UserGeolocation { get; set; }


        [InverseProperty(nameof(UserNotification.Receiver))]
        public ICollection<UserNotification>? Notifications { get; set; }


        [InverseProperty(nameof(UserFriend.User))]
        public ICollection<UserFriend>? UserFriends_User { get; set; }

        [InverseProperty(nameof(UserFriend.Friend))]
        public ICollection<UserFriend>? UserFriends_Friend { get; set; }


        [InverseProperty(nameof(MessageModel.Sender))]
        public ICollection<MessageModel>? Message_Sender { get; set; }

        [InverseProperty(nameof(MessageModel.Receiver))]
        public ICollection<MessageModel>? Message_Receiver { get; set; }

        
        [InverseProperty(nameof(MatchModel.MatchHost))]
        public ICollection<MatchModel>? Matches_Host { get; set; }

        [InverseProperty(nameof(UserRating.User))]
        public ICollection<UserRating>? UserRatings_User { get; set; }


        [InverseProperty(nameof(GroupParticipant.Participant))]
        public ICollection<GroupParticipant>? GroupParticipants_Participant { get; set; }

        [InverseProperty(nameof(TournamentParticipant.Participant))]
        public ICollection<TournamentParticipant>? TournamentParticipants_Participant { get; set; }
    }
}