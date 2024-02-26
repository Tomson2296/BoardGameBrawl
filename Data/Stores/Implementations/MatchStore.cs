#nullable disable
using AutoMapper.QueryableExtensions;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class MatchStore : IMatchStore<MatchModel, BoardgameModel, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration;

        public MatchStore(ApplicationDbContext context, AutoMapper.IConfigurationProvider configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateMatchAsync(MatchModel match, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentException.ThrowIfNullOrEmpty(match.BoardgameId);

            _context.Matches.Add(match);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create match {match.Id}." });
        }

        public async Task<IdentityResult> DeleteMatchAsync(MatchModel match, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            var matchFromDB = await _context.Matches.FindAsync(match.Id);

            if (matchFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find match to deletion process: {match.Id}." });
            }
            else
            {
                _context.Matches.Remove(matchFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete match {match.Id}." });
            }
        }
        public async Task<IdentityResult> UpdateMatchAsync(MatchModel match, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            _context.Matches.Update(match);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update match {match.Id}." });
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //
        // finding methods
        //

        public async Task<MatchModel> FindMatchByIdAsync(string matchId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(matchId);
            return await _context.Matches.AsNoTracking().SingleOrDefaultAsync(m => m.Id.Equals(matchId), cancellationToken);
        }

        public async Task<IEnumerable<MatchModel>> FindAllMatchesByHostId(string hostId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(hostId);

            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchHostId == hostId)
                .ToListAsync();
        }

        //
        // setters methods
        //

        public async Task SetMatchBoardgameAsync(MatchModel match, BoardgameModel boardgame, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(boardgame);
            match.BoardgameId = boardgame.Id;
            await Task.CompletedTask;
        }

        public async Task SetMatchHostUserAsync(MatchModel match, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(user);
            match.MatchHostId = user.Id;
            await Task.CompletedTask;
        }

        public async Task SetMatchEndDateAsync(MatchModel match, DateTime DateEnd, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(DateEnd);
            match.MatchDate_End = DateEnd;
            await Task.CompletedTask;
        }

        public async Task SetMatchNumberOfPlayersAsync(MatchModel match, int numberOfPlayers, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(numberOfPlayers);
            match.NumberOfPlayers = numberOfPlayers;
            await Task.CompletedTask;
        }

        public async Task SetMatchParticipaintsAsync(MatchModel match, List<string> participants, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(participants);
            match.Match_Participants = participants;
            await Task.CompletedTask;
        }

        public async Task SetMatchResultsAsync(MatchModel match, List<string> results, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(results);
            match.Match_Results = results;
            await Task.CompletedTask;
        }

        public async Task SetMatchRulesetAsync(MatchModel match, List<string> ruleset, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(ruleset);
            match.Match_Ruleset = ruleset;
            await Task.CompletedTask;
        }

        public async Task SetMatchStartDateAsync(MatchModel match, DateTime DateStart, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(DateStart);
            match.MatchDate_Start = DateStart;
            await Task.CompletedTask;
        }

        public async Task SetMatchCreatedDateAsync(MatchModel match, DateTime DateCreated, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(DateCreated);
            match.MatchCreated = DateCreated;
            await Task.CompletedTask;
        }

        public async Task SetMatchLocationLatitudeAsync(MatchModel match, string latitude, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentException.ThrowIfNullOrEmpty(latitude);
            match.Location_Latitude = latitude;
            await Task.CompletedTask;
        }

        public async Task SetMatchLocationLongitudeAsync(MatchModel match, string longitude, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentException.ThrowIfNullOrEmpty(longitude);
            match.Location_Longitude = longitude;
            await Task.CompletedTask;
        }

        public async Task SetMatchLocationImageAsync(MatchModel match, byte[] image, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(match);
            ArgumentNullException.ThrowIfNull(image);
            match.Location_Image = image;
            await Task.CompletedTask;
        }

        //
        // DTO related methods
        //

        public async Task<BasicMatchInfoDTO> FindMatchDTOByIdAsync(string matchId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(matchId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.Id == matchId)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllMatchesDTOByBoardgameIdAsync(string boardgameId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(boardgameId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.BoardgameId == boardgameId)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllMatchesDTOByBoardgameIdOrderedAsync(string boardgameId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(boardgameId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.BoardgameId == boardgameId)
                .OrderByDescending(m => m.MatchCreated)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> TakeBatchofMatchesDTOByBoardgameIdAsync(string boardgameId, int pad, int batchSize, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(boardgameId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.BoardgameId == boardgameId)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .Skip(pad*batchSize)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

       
        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesDTOAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchProgress == MatchProgress.Upcoming)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesOrderedDTOAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchProgress == MatchProgress.Upcoming)
                .OrderByDescending(m => m.MatchCreated)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> TakeBatchofUpcomingMatchesDTOAsync(int pad, int batchSize, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(pad);
            ArgumentNullException.ThrowIfNull(batchSize);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchProgress == MatchProgress.Upcoming)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .Skip(pad * batchSize)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesDTOByHostIdAsync(string hostId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(hostId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchProgress == MatchProgress.Upcoming && m.MatchHostId == hostId)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllUpcomingMatchesDTOOrderedByHostIdAsync(string hostId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(hostId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchProgress == MatchProgress.Upcoming && m.MatchHostId == hostId)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .OrderByDescending(m => m.MatchCreated)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BasicMatchInfoDTO>> FindAllStartedMatchesDTOByHostIdAsync(string hostId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(hostId);
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.MatchProgress == MatchProgress.Started && m.MatchHostId == hostId)
                .ProjectTo<BasicMatchInfoDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }
    }
}