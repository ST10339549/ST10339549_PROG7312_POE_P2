using System.Threading.Tasks;

namespace MunicipalServiceApp.Application.Abstractions
{
    /// <summary>
    /// Result of validating/geocoding a user-entered address.
    /// </summary>
    public sealed record GeoValidationResult(
        bool Success,
        string ErrorMessage,
        string NormalizedAddress,
        double Latitude,
        double Longitude
    );

    /// <summary>
    /// Geocoding/validation service contract.
    /// Implemented by Infrastructure.Geocoding.NominatimGeocodingService.
    /// </summary>
    public interface IGeocodingService
    {
        Task<GeoValidationResult> ValidateAsync(string rawAddress);
    }
}
