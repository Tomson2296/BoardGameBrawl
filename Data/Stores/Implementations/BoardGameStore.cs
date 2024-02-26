#nullable disable

using AutoMapper.QueryableExtensions;
using BoardGameBrawl;
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class BoardGameStore : IBoardGameStore<BoardgameModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configuration;

        public BoardGameStore(ApplicationDbContext context, AutoMapper.IConfigurationProvider configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            _context.Boardgames.Add(game);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                    ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create boardgame {game.Name}." });
        }

        public async Task<IdentityResult> DeleteAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            var gameFromDB = await _context.Boardgames.FindAsync(game.Id);

            if (gameFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find boardgame to deletion process: {game.Name}." });
            }
            else
            {
                _context.Boardgames.Remove(gameFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete boardgame {game.Name}." });
            }
        }

        public async Task<BoardgameModel> FindBoardGameByIdAsync(string gameId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(gameId);
            return await _context.Boardgames.AsNoTracking().SingleOrDefaultAsync(g => g.Id.Equals(gameId), cancellationToken);
        }

        public async Task<BoardgameModel> FindBoardGameByBGGIdAsync(int BGGId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(BGGId);
            return await _context.Boardgames.AsNoTracking().SingleOrDefaultAsync(g => g.BGGId.Equals(BGGId), cancellationToken);
        }

        public async Task<BoardgameModel> FindBoardGameByBGNameAsync(string boardgameName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(boardgameName);
            return await _context.Boardgames.AsNoTracking().SingleOrDefaultAsync(g => g.Name.Equals(boardgameName), cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            _context.Boardgames.Update(game);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                    ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update boardgame {game.Name}." });
        }

        public async Task<string> GetBoardGameNameAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.Name);
        }

        public async Task SetBoardGameNameAsync(BoardgameModel game, string name, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(name);
            game.Name = name;
            await Task.CompletedTask;
        }

        public async Task<byte?> GetBoardGameMinPlayersAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.MinPlayers);
        }

        public async Task<byte?> GetBoardGameMaxPlayersAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.MaxPlayers);
        }

        public async Task<int?> GetBoardGameYearPublishedAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.YearPublished);
        }

        public async Task<string> GetBoardGameImageFileNameAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.ImageFile);
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

        public async Task<bool> CheckIsBoardgameExistsInDBAsync(int BGGId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var foundBoardgame = await _context.Boardgames.AsNoTracking().SingleOrDefaultAsync(b => b.BGGId == BGGId, cancellationToken);
            
            if (foundBoardgame != default)
                return true;
            else
                return false;
        }

        public async Task SetBoardGameMinPlayersAsync(BoardgameModel game, byte minPlayers, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(minPlayers);
            game.MinPlayers = minPlayers;
            await Task.CompletedTask;
        }

        public async Task SetBoardGameMaxPlayersAsync(BoardgameModel game, byte maxPlayers, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(maxPlayers);
            game.MaxPlayers = maxPlayers;
            await Task.CompletedTask;
        }

        public async Task SetBoardGameYearPublishedAsync(BoardgameModel game, int yearPublished, CancellationToken cancellationToken = default)    
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(yearPublished);
            game.YearPublished = yearPublished;
            await Task.CompletedTask;
        }

        public async Task SetBoardGameImageFileNameAsync(BoardgameModel game, string imagefile, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentException.ThrowIfNullOrEmpty(imagefile);
            game.ImageFile = imagefile;
            await Task.CompletedTask;
        }

        public async Task<List<string>> GetBoardgameCategoriesAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.Boardgame_Categories);
        }

        public async Task SetBoardgameCategoriesAsync(BoardgameModel game, List<string> categories, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(categories);
            game.Boardgame_Categories = categories;
            await Task.CompletedTask;
        }

        public async Task<List<string>> GetBoardgameMechanicsAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.Boardgame_Mechanics);
        }

        public async Task SetBoardgameMechanicsAsync(BoardgameModel game, List<string> mechanics, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(mechanics);
            game.Boardgame_Mechanics = mechanics;
            await Task.CompletedTask;
        }

        public async Task<int?> GetBoardGameBGGIdAsync(BoardgameModel game, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            return await Task.FromResult(game.BGGId); 
        }

        public async Task SetBoardGameBGGIdAsync(BoardgameModel game, int BGGId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(BGGId);
            game.BGGId = BGGId;
            await Task.CompletedTask;
        }

        //
        // DTO methods
        //

        public async Task<IEnumerable<BoardgameDTO>> GetBoardgamesDTOByFilterAsync(string filter, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(filter);
            return await _context.Boardgames
                .AsNoTracking()
                .Where(b => b.Name.Contains(filter))
                .ProjectTo<BoardgameDTO>(_configuration)
                .ToListAsync(cancellationToken);
        }
    }
}