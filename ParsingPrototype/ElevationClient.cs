using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParsingPrototype
{
    public static class ElevationClient
    {
        // A single HttpClient is meant to be created once and reused for
        // the whole app's lifetime, not created fresh per call. Creating
        // many short-lived HttpClients can exhaust the machine's network
        // sockets under load - a well-known C# gotcha, not something
        // specific to this project.
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<double?> GetElevationAsync(double latitude, double longitude)
        {
            string url = $"https://api.opentopodata.org/v1/eudem25m?locations={latitude:F6},{longitude:F6}";

            string json = await httpClient.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement result = doc.RootElement.GetProperty("results")[0];
            JsonElement elevationElement = result.GetProperty("elevation");

            // EU-DEM has no data over the sea - the API returns null
            // rather than a fake 0, and this needs to pass that through
            // rather than silently treating "no data" as sea level.
            if (elevationElement.ValueKind == JsonValueKind.Null)
                return null;

            return elevationElement.GetDouble();
        }
    }
}
