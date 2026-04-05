using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Recarga;
using System.Net.Http.Json;

namespace RechargeFunctions.Mobile.Services
{
    public class RecargaApiService
    {
        private readonly HttpClient _http;

        public RecargaApiService()
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

        public async Task<bool> CrearRecargaAsync(CreateRecargaRequest request)
        {
            var response = await _http.PostAsJsonAsync("Recargas", request);

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON: {rawJson}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<RecargaDto>> ObtenerRecargasAsync()
        {
            var response = await _http.GetAsync("Recargas");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode RECARGAS: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON RECARGAS: {rawJson}");

            response.EnsureSuccessStatusCode();

            var recargas = await response.Content.ReadFromJsonAsync<List<RecargaDto>>();

            return recargas ?? new List<RecargaDto>();
        }

        public async Task<RecargaDto?> ObtenerRecargaPorIdAsync(int id)
        {
            var response = await _http.GetAsync($"Recargas/{id}");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode RECARGA POR ID: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON RECARGA POR ID: {rawJson}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<RecargaDto>();
        }

        public async Task<bool> ActualizarRecargaAsync(int id, UpdateRecargaRequest request)
        {
            var response = await _http.PutAsJsonAsync($"Recargas/{id}", request);

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode UPDATE RECARGA: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON UPDATE RECARGA: {rawJson}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> MarcarComoPagadaAsync(int recargaId)
        {
            var response = await _http.PatchAsync($"Recargas/{recargaId}/pagar", null);

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode PAGAR: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON PAGAR: {rawJson}");

            return response.IsSuccessStatusCode;
        }
    }
}