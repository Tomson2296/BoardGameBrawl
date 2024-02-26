#nullable disable

using BoardGameBrawl;
using BoardGameBrawl.Data.Models.API_XML;
using System.Xml.Serialization;

namespace BoardGameBrawl.Services
{
    public class BGGAPIService : IBGGAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BGGAPIService> _logger;

        public BGGAPIService(IHttpClientFactory httpClientFactory, ILogger<BGGAPIService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<BoardgameItem> GetBGGBoardGameInfo(int BGGBoardGameID)
        {
            string apiUrl = $"https://boardgamegeek.com/xmlapi2/thing?type=boardgame&stats=1&videos=1&id={BGGBoardGameID}";

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("BoardGameGeekClient");
                HttpResponseMessage responseMessage = await httpClient.GetAsync(apiUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    while (responseMessage.StatusCode.Equals(StatusCodes.Status202Accepted))
                    {
                        // Sometimes calling API returns 202 code in order to prepare 
                        // response message - make 1000ms delay and make another request
                        await Task.Delay(1000);
                        responseMessage = await httpClient.GetAsync(apiUrl);
                    }

                    using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var serializer = new XmlSerializer(typeof(BoardgameItem));
                        using (var reader = new StreamReader(stream))
                        {
                            var BoardgameInfo = (BoardgameItem)serializer.Deserialize(stream);
                            return BoardgameInfo;
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Error: {responseMessage.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<BoardGameCollection> GetUserBGGCollectionInfo(string BGGUserName)
        {
            string lowerCasedUserName = BGGUserName.ToLower();
            string apiUrl = $"https://boardgamegeek.com//xmlapi2/collection?username={lowerCasedUserName}&excludesubtype=boardgameexpansion";

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("BoardGameGeekClient");
                HttpResponseMessage responseMessage = await httpClient.GetAsync(apiUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    while (responseMessage.StatusCode.Equals(StatusCodes.Status202Accepted))
                    {
                        // Sometimes calling API returns 202 code in order to prepare 
                        // response message - make 1000ms delay and make another request
                        await Task.Delay(1000);
                        responseMessage = await httpClient.GetAsync(apiUrl);
                    }

                    using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var serializer = new XmlSerializer(typeof(BoardGameCollection));
                        using (var reader = new StreamReader(stream))
                        {
                            var BoardgameCollection = (BoardGameCollection)serializer.Deserialize(stream);
                            return BoardgameCollection;
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Error: {responseMessage.StatusCode}");
                    return new BoardGameCollection();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BoardGameCollection();
            }
        }
    }
}