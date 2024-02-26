using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Identity.Client;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IMatchStore<TMatch, TGame, TUser> : IDisposable where TMatch : class where TGame : class where TUser : class
    {
        Task<IdentityResult> CreateMatchAsync(TMatch match, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateMatchAsync(TMatch match, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteMatchAsync(TMatch match, CancellationToken cancellationToken = default);


        Task<TMatch> FindMatchByIdAsync(string matchId, CancellationToken cancellationToken = default);

        //
        // DTO related methods
        //

        Task<BasicMatchInfoDTO> FindMatchDTOByIdAsync(string matchId, CancellationToken cancellationToken = default);


        Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesDTOAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesOrderedDTOAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicMatchInfoDTO>> TakeBatchofUpcomingMatchesDTOAsync(int pad, int batchSize, CancellationToken cancellationToken = default);


        Task<IEnumerable<BasicMatchInfoDTO>> FindAllMatchesDTOByBoardgameIdAsync(string boardgameId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicMatchInfoDTO>> FindAllMatchesDTOByBoardgameIdOrderedAsync(string boardgameId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicMatchInfoDTO>> TakeBatchofMatchesDTOByBoardgameIdAsync(string boardgameId, int pad, int batchSize, CancellationToken cancellationToken = default);


        Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesDTOByHostIdAsync(string hostId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesDTOOrderedByHostIdAsync(string hostId, CancellationToken cancellationToken = default);

        Task<IEnumerable<BasicMatchInfoDTO>> FindAllStartedMatchesDTOByHostIdAsync(string hostId, CancellationToken cancellationToken = default);

        //
        // setter methods
        //

        Task SetMatchBoardgameAsync(TMatch match, TGame boardgame, CancellationToken cancellationToken = default);

        Task SetMatchHostUserAsync(TMatch match, TUser user, CancellationToken cancellationToken = default);
        
        Task SetMatchStartDateAsync(TMatch match, DateTime DateStart, CancellationToken cancellationToken = default);

        Task SetMatchEndDateAsync(TMatch match, DateTime DateEnd, CancellationToken cancellationToken = default);

        Task SetMatchCreatedDateAsync(TMatch match, DateTime DateCreated, CancellationToken cancellationToken = default);

        Task SetMatchNumberOfPlayersAsync(TMatch match, int numberOfPlayers, CancellationToken cancellationToken = default);

        Task SetMatchParticipaintsAsync(TMatch match, List<string> participants, CancellationToken cancellationToken = default);

        Task SetMatchRulesetAsync(TMatch match, List<string> ruleset, CancellationToken cancellationToken = default);

        Task SetMatchResultsAsync(TMatch match, List<string> results, CancellationToken cancellationToken = default);

        Task SetMatchLocationLatitudeAsync(TMatch match, string latitude, CancellationToken cancellationToken = default);

        Task SetMatchLocationLongitudeAsync(TMatch match, string longitude, CancellationToken cancellationToken = default);

        Task SetMatchLocationImageAsync(TMatch match, byte[] image, CancellationToken cancellationToken = default);
    }
}