using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace BoardGameBrawl.Data.Stores.Interfaces
{
    public interface IUserGeolocationStore<TGeo, TUser> : IDisposable where TGeo : class where TUser : class
    {
        Task<IdentityResult> CreateGeolocationDataAsync(TGeo geolocation, CancellationToken cancellationToken = default);

        Task<IdentityResult> DeleteGeolocationDataAsync(TGeo geolocation, CancellationToken cancellationToken = default);

        Task<IdentityResult> UpdateGeolocationDataAsync(TGeo geolocation, CancellationToken cancellationToken = default); 

        
        Task<TGeo> FindGeolocationDataByIdAsync(string geolocationId, CancellationToken cancellationToken = default);

        Task<TGeo> FindGeolocationDataByUserIdAsync(string userID, CancellationToken cancellationToken = default);


        Task SetUserByAsync(TGeo geolocation, TUser user, CancellationToken cancellationToken = default);


        Task SetCityAsync(TGeo geolocation, string city, CancellationToken cancellationToken = default);

        Task<string> GetCityAsync(TGeo geolocation, CancellationToken cancellationToken = default);

        Task SetLatitudeAsync(TGeo geolocation, string latitude, CancellationToken cancellationToken = default);

        Task<string> GetLatitudesync(TGeo geolocation, CancellationToken cancellationToken = default);

        Task SetLongitudeAsync(TGeo geolocation, string longitude, CancellationToken cancellationToken = default);

        Task<string> GetLongitudeAsync(TGeo geolocation, CancellationToken cancellationToken = default);

        Task SetGeolocationImageAsync(TGeo geolocation, byte[] image, CancellationToken cancellationToken = default);

        Task<byte[]> GetGeolocationImageAsync(TGeo geolocation, CancellationToken cancellationToken = default);


        Task<bool> CheckIfUserEverProvidedDataAsync(TUser user, CancellationToken cancellationToken = default);

        Task<bool> CheckIfUserHaveImageDataAsync(TUser user, CancellationToken cancellationToken = default);

        Task<bool> CheckIfValuesProvidedAreTheSame(TGeo geolocation, TUser user, CancellationToken cancellationToken = default);

    }
}
