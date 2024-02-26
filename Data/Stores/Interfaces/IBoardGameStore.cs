using BoardGameBrawl.Data.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IBoardGameStore<TGame> : IDisposable where TGame : class
    {
        Task<IdentityResult> CreateAsync(TGame game, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateAsync(TGame game, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteAsync(TGame game, CancellationToken cancellationToken = default);

        

        Task<TGame> FindBoardGameByIdAsync(string gameId, CancellationToken cancellationToken = default);

        Task<TGame> FindBoardGameByBGGIdAsync(int BGGId, CancellationToken cancellationToken = default);

        Task<TGame> FindBoardGameByBGNameAsync(string boardgameName, CancellationToken cancellationToken = default);

        //
        // DTO methods
        //

        Task<IEnumerable<BoardgameDTO>> GetBoardgamesDTOByFilterAsync(string filter, CancellationToken cancellationToken = default);

        //
        // getter and setter methods
        //

        Task<string?> GetBoardGameNameAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardGameNameAsync(TGame game, string name, CancellationToken cancellationToken = default);

        Task<int?> GetBoardGameBGGIdAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardGameBGGIdAsync(TGame game, int BGGId, CancellationToken cancellationToken = default);

        Task<byte?> GetBoardGameMinPlayersAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardGameMinPlayersAsync(TGame game, byte minPlayers, CancellationToken cancellationToken = default);

        Task<byte?> GetBoardGameMaxPlayersAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardGameMaxPlayersAsync(TGame game, byte maxPlayers, CancellationToken cancellationToken = default);

        Task<int?> GetBoardGameYearPublishedAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardGameYearPublishedAsync(TGame game, int yearPublished, CancellationToken cancellationToken = default);
        
        Task<string?> GetBoardGameImageFileNameAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardGameImageFileNameAsync(TGame game, string Imagefile, CancellationToken cancellationToken = default);

        Task<List<string?>> GetBoardgameCategoriesAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardgameCategoriesAsync(TGame game, List<string> categories, CancellationToken cancellationToken = default);

        Task<List<string?>> GetBoardgameMechanicsAsync(TGame game, CancellationToken cancellationToken = default);

        Task SetBoardgameMechanicsAsync(TGame game, List<string> mechanics, CancellationToken cancellationToken = default);


        Task<bool> CheckIsBoardgameExistsInDBAsync(int BGGId, CancellationToken cancellationToken = default);
    }
}
