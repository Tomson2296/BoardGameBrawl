#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System.Xml.Serialization;

namespace BoardGameBrawl.Services
{
    public class NominatimGeolocationAPI : INominatimGeolocationAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NominatimGeolocationAPI> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NominatimGeolocationAPI(IHttpClientFactory httpClientFactory, ILogger<NominatimGeolocationAPI> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _hostingEnvironment = webHostEnvironment;
        }

        public async Task<GeolocationData> GetGeolocationDataByCityNameAsync(string cityName)
        {
            string apiUrl = $"https://nominatim.openstreetmap.org/search?format=xml&q={cityName}";
            string userAgent = _hostingEnvironment.ApplicationName;

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("NominativGeolocationAPIClient");
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                
                // abide usage policy -> 1 request per 1 second 
                // for strict educational and testing purposes in application 
                await Task.Delay(1500);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(apiUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var serializer = new XmlSerializer(typeof(GeolocationData));
                        using (var reader = new StreamReader(stream))
                        {
                            GeolocationData geolocationData = (GeolocationData)serializer.Deserialize(stream);
                            return geolocationData;
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

        public async Task<GeolocationData> GetGeolocationDataByStreetNameAsync(string street, string cityName)
        {
            string apiUrl = $"https://nominatim.openstreetmap.org/search?format=xml&q={street},{cityName}";
            string userAgent = _hostingEnvironment.ApplicationName;

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("NominativGeolocationAPIClient");
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

                // abide usage policy -> 1 request per 1 second 
                // for strict educational and testing purposes in application 
                await Task.Delay(1500);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(apiUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var serializer = new XmlSerializer(typeof(GeolocationData));
                        using (var reader = new StreamReader(stream))
                        {
                            GeolocationData geolocationData = (GeolocationData)serializer.Deserialize(stream);
                            return geolocationData;
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
    }
}
