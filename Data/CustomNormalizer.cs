using BoardGameBrawl.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace BoardGameBrawl.Data
{
    public class CustomNormalizer : ILookupNormalizer
    {
        [return: NotNullIfNotNull(nameof(email))]
        public string? NormalizeEmail(string? email)
        {
            return email?.Normalize().ToLowerInvariant();
        }

        [return: NotNullIfNotNull(nameof(name))]
        public string? NormalizeName(string? name)
        {
            return name?.Normalize().ToLowerInvariant();
        }
    }
}
