using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Tarjeta;
using System.Net.Http.Json;

namespace RechargeFunctions.Mobile.Services
{
    public class TarjetaApiService
    {
        private readonly HttpClient _http;

        public TarjetaApiService()
        {
#if ANDROID
            var baseUrl = "http://10.0.2.2:5144/api/";
#else
            var baseUrl = "http://localhost:5144/api/";
#endif

            _http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<bool> CrearTarjetaAsync(CreateTarjetaRequest request)
        {
            var response = await _http.PostAsJsonAsync("Tarjetas", request);

            var raw = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW: {raw}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<TarjetaDto>> ObtenerTarjetasAsync()
        {
            var response = await _http.GetAsync("Tarjetas");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode TARJETAS: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON TARJETAS: {rawJson}");

            response.EnsureSuccessStatusCode();

            var tarjetas = await response.Content.ReadFromJsonAsync<List<TarjetaDto>>();

            return tarjetas ?? new List<TarjetaDto>();
        }

        public async Task<TarjetaDto?> ObtenerTarjetaPorIdAsync(int id)
        {
            var response = await _http.GetAsync($"Tarjetas/{id}");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode TARJETA POR ID: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON TARJETA POR ID: {rawJson}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TarjetaDto>();
        }

        public async Task<bool> ActualizarTarjetaAsync(int id, UpdateTarjetaRequest request)
        {
            var response = await _http.PutAsJsonAsync($"Tarjetas/{id}", request);

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode UPDATE TARJETA: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON UPDATE TARJETA: {rawJson}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DesactivarTarjetaAsync(int id)
        {
            var response = await _http.DeleteAsync($"Tarjetas/{id}");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode DELETE TARJETA: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON DELETE TARJETA: {rawJson}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActivarTarjetaAsync(int id)
        {
            var response = await _http.PatchAsync($"Tarjetas/{id}/activar", null);

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode ACTIVAR TARJETA: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON ACTIVAR TARJETA: {rawJson}");

            return response.IsSuccessStatusCode;
        }
    }
}