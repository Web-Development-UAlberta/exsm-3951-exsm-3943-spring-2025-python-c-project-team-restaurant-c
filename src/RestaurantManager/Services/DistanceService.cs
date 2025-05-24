using System.Text.Json;

namespace RestaurantManager.Services;


public class DistanceService
{
    private readonly HttpClient _httpClient;

    public DistanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpApp/1.0");
    }

    public async Task<double?> GetDrivingDistanceAsync(string location1, string location2)
    {
        (double Lat, double Lon)? coord1 = await GetCoordinatesAsync(location1);
        (double Lat, double Lon)? coord2 = await GetCoordinatesAsync(location2);

        if (coord1 == null || coord2 == null)
        {
            Console.WriteLine("Could not geocode one or both locations.");
            return null;
        }

        string url = $"https://router.project-osrm.org/route/v1/driving/" +
                     $"{coord1.Value.Lon},{coord1.Value.Lat};" +
                     $"{coord2.Value.Lon},{coord2.Value.Lat}?overview=false";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);

        double distance = doc.RootElement
            .GetProperty("routes")[0]
            .GetProperty("distance")
            .GetDouble();

        return distance / 1000;
    }

    public async Task<(double Lat, double Lon)?> GetCoordinatesAsync(string location)
    {
        var encodedLocation = Uri.EscapeDataString(location);
        var url = $"https://nominatim.openstreetmap.org/search?q={encodedLocation}&format=json&limit=1";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var results = doc.RootElement;

        if (results.GetArrayLength() == 0)
            return null;

        var lat = double.Parse(results[0].GetProperty("lat").GetString()!);
        var lon = double.Parse(results[0].GetProperty("lon").GetString()!);

        return (lat, lon);
    }
}