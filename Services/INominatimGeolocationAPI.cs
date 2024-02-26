using BoardGameBrawl.Data.Models.API_XML;

namespace BoardGameBrawl.Services
{
    public interface INominatimGeolocationAPI
    {
        public Task<GeolocationData> GetGeolocationDataByStreetNameAsync(string street, string cityName);
        public Task<GeolocationData> GetGeolocationDataByCityNameAsync(string cityName);
    }
}
