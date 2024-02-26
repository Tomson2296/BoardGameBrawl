using BoardGameBrawl.Data.Models.API_XML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BoardGameBrawl.Services
{
    public interface IBGGAPIService
    {
        /// <summary>
        /// Gets information about specific Boardgame on BoardGameGeek website via BoardGameGeek XML API v2.
        /// </summary>
        /// <param name="BGGBoardGameID"></param>
        /// <returns></returns>
        public Task<BoardgameItem> GetBGGBoardGameInfo(int BGGBoardGameID);

        /// <summary>
        /// Gets information about User collection on BoardGameGeek website via BoardGameGeek XML API v2.
        /// </summary>
        /// <param name="BGGUserName"></param>
        /// <returns></returns>
        public Task<BoardGameCollection> GetUserBGGCollectionInfo(string BGGUserName);
    }
}
