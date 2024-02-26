using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameBrawl.Services
{
    public interface IGoogleStaticMapsService
    {
        // getting byte array of image get from google static maps API 
        public Task<byte[]> GetStaticMapUsingGeolocationDataAsync(string latitude, string longitude, string zoom, string imgWidth, string imgHeight);
    }
}
