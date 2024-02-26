#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BoardGameBrawl.Data.Stores.Implementations
{
    public class UserGeolocationStore : IUserGeolocationStore<UserGeolocation, ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public UserGeolocationStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateGeolocationDataAsync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            ArgumentException.ThrowIfNullOrEmpty(geolocation.UserId);

            _context.UserGeolocations.Add(geolocation);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not create geolocationData {geolocation.Id}." });
        }

        public async Task<IdentityResult> DeleteGeolocationDataAsync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            var geolocationdataFromDB = await _context.UserGeolocations.FindAsync(geolocation.Id);

            if (geolocationdataFromDB == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Could not find geolocationData to deletion process: {geolocation.Id}." });
            }
            else
            {
                _context.UserGeolocations.Remove(geolocationdataFromDB);
                var affectedRows = await _context.SaveChangesAsync(cancellationToken);
                return affectedRows > 0
                        ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete geolocationData {geolocation.Id}." });
            }
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

        public async Task<UserGeolocation> FindGeolocationDataByIdAsync(string geolocationId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(geolocationId);
            return await _context.UserGeolocations.AsNoTracking().SingleOrDefaultAsync(s => s.Id.Equals(geolocationId), cancellationToken);
        }

        public async Task<UserGeolocation> FindGeolocationDataByUserIdAsync(string userID, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(userID);
            return await _context.UserGeolocations.AsNoTracking().SingleOrDefaultAsync(s => s.UserId.Equals(userID), cancellationToken);
        }

        public async Task<string> GetCityAsync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            return await Task.FromResult(geolocation.City);
        }

        public async Task<string> GetLatitudesync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            return await Task.FromResult(geolocation.Latitude);
        }

        public async Task<string> GetLongitudeAsync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            return await Task.FromResult(geolocation.Longitude);
        }

        public async Task SetCityAsync(UserGeolocation geolocation, string city, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            ArgumentNullException.ThrowIfNull(city);
            geolocation.City = city;
            await Task.CompletedTask;
        }

        public async Task SetLatitudeAsync(UserGeolocation geolocation, string latitude, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            ArgumentNullException.ThrowIfNull(latitude);
            geolocation.Latitude = latitude;
            await Task.CompletedTask;
        }

        public async Task SetLongitudeAsync(UserGeolocation geolocation, string longitude, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            ArgumentNullException.ThrowIfNull(longitude);
            geolocation.Longitude = longitude;
            await Task.CompletedTask;
        }

        public async Task SetUserByAsync(UserGeolocation geolocation, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            ArgumentNullException.ThrowIfNull(user);
            geolocation.UserId = user.Id;
            await Task.CompletedTask;
        }

        public async Task SetGeolocationImageAsync(UserGeolocation geolocation, byte[] image, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            ArgumentNullException.ThrowIfNull(image);
            geolocation.GeolocationImage = image;
            await Task.CompletedTask;
        }

        public async Task<byte[]> GetGeolocationImageAsync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            return await Task.FromResult(geolocation.GeolocationImage);
        }

        public async Task<IdentityResult> UpdateGeolocationDataAsync(UserGeolocation geolocation, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(geolocation);
            _context.UserGeolocations.Update(geolocation);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
            ? IdentityResult.Success
                    : IdentityResult.Failed(new IdentityError() { Description = $"Could not update geolocationData {geolocation.Id}." });
        }

        public async Task<bool> CheckIfUserEverProvidedDataAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            var element = await _context.UserGeolocations
                .AsNoTracking()
                .Where(u => u.UserId == user.Id)
                .Select(u => u.City)
                .SingleOrDefaultAsync(cancellationToken);

            if (element != default)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckIfValuesProvidedAreTheSame(UserGeolocation geolocation, ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            var geolocationInDB = await FindGeolocationDataByUserIdAsync(user.Id);
            if (geolocation.Equals(geolocationInDB))
                return true;
            else
                return false;
        }

        public async Task<bool> CheckIfUserHaveImageDataAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            var imageData = await _context.UserGeolocations
                .AsNoTracking()
                .Where(u => u.UserId == user.Id)
                .Select(u => u.GeolocationImage)
                .SingleOrDefaultAsync(cancellationToken);

            if (imageData == default)
                return false;
            else
                return true;
        }
    }
}