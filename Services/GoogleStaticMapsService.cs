using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace BoardGameBrawl.Services
{
    public class GoogleStaticMapsService : IGoogleStaticMapsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GoogleStaticMapsService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public GoogleStaticMapsService(
            IHttpClientFactory httpClientFactory,
            ILogger<GoogleStaticMapsService> logger,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
            )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _hostingEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<byte[]> GetStaticMapUsingGeolocationDataAsync(string latitude, string longitude, string zoom, string imgWidth, string imgHeight)
        {
            // code usage example 
            // https://maps.googleapis.com/maps/api/staticmap?center=Berkeley,CA&zoom=14&size=400x400&key=&signature=YOUR_SIGNATURE
            //

            string API_signature = _configuration.GetSection("GoogleStaticMapsAPI").Value!;
            string apiUrl = $"https://maps.googleapis.com/maps/api/staticmap?center={latitude},{longitude}&zoom={zoom}&size={imgWidth}x{imgHeight}" +
                $"&markers=size:medium%7Ccolor:red%7C{latitude},{longitude}&key={API_signature}";
            string userAgent = _hostingEnvironment.ApplicationName;

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("GoogleStaticMapsAPI");
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

                HttpResponseMessage responseMessage = await httpClient.GetAsync(apiUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        byte[] imageData;
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            imageData = memoryStream.ToArray();
                        }
                        return imageData;
                    }
                }
                else
                {
                    _logger.LogError($"Error: {responseMessage.StatusCode}");
                    return [];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return [];
            }
        }
    }
}
