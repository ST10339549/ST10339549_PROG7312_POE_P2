using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MunicipalServiceApp.Application.Abstractions;

namespace MunicipalServiceApp.Infrastructure.Geocoding
{
    /// <summary>
    /// Minimal geocoder using OpenStreetMap Nominatim.
    /// Respects their usage policy by setting a User-Agent and doing single lookups.
    /// </summary>
    public sealed class NominatimGeocodingService : IGeocodingService, IDisposable
    {
        private readonly HttpClient _http;

        public NominatimGeocodingService(HttpClient? http = null)
        {
            _http = http ?? new HttpClient();
            // Identify app (required by Nominatim policy)
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "MunicipalServiceApp/1.0 (+https://github.com/yourname/yourrepo; contact you@example.com)"
            );
        }

        public async Task<GeoValidationResult> ValidateAsync(string rawAddress)
        {
            if (string.IsNullOrWhiteSpace(rawAddress))
                return new GeoValidationResult(false, "Address is required.", "", 0, 0);

            try
            {
                var url =
                    $"https://nominatim.openstreetmap.org/search?format=jsonv2&addressdetails=1&countrycodes=za&limit=1&q={Uri.EscapeDataString(rawAddress)}";

                using var resp = await _http.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                    return new GeoValidationResult(false, $"Geocoding service error ({(int)resp.StatusCode}).", "", 0, 0);

                await using var stream = await resp.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
                    return new GeoValidationResult(false, "Address not found. Please enter a valid municipal address.", "", 0, 0);

                var item = doc.RootElement[0];

                var display = item.TryGetProperty("display_name", out var dn)
                    ? (dn.GetString() ?? rawAddress)
                    : rawAddress;

                var latStr = item.GetProperty("lat").GetString();
                var lonStr = item.GetProperty("lon").GetString();

                if (!double.TryParse(latStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) ||
                    !double.TryParse(lonStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
                {
                    return new GeoValidationResult(false, "Could not parse coordinates for that address.", "", 0, 0);
                }

                if (item.TryGetProperty("address", out var addr) &&
                    addr.TryGetProperty("country_code", out var cc) &&
                    !string.Equals(cc.GetString(), "za", StringComparison.OrdinalIgnoreCase))
                {
                    return new GeoValidationResult(false, "Address is outside South Africa.", "", 0, 0);
                }

                return new GeoValidationResult(true, "", display, lat, lon);
            }
            catch (Exception ex)
            {
                return new GeoValidationResult(false,
                    "Unable to validate address right now. Please try again later. " + ex.Message,
                    "", 0, 0);
            }
        }

        public void Dispose() => _http.Dispose();
    }
}
