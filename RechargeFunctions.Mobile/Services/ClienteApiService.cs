using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Cliente;
using System.Net.Http.Json;

namespace RechargeFunctions.Mobile.Services
{
    public class ClienteApiService
    {
        private readonly HttpClient _http;

        public ClienteApiService()
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

        public async Task<List<ClienteDto>> BuscarClientesAsync(string term)
        {
            var url = $"Clientes/search?term={Uri.EscapeDataString(term)}";

            var response = await _http.GetAsync(url);

            var rawJson = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine($"URL: {_http.BaseAddress}{url}");
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON: {rawJson}");

            response.EnsureSuccessStatusCode();

            var clientes = await response.Content.ReadFromJsonAsync<List<ClienteDto>>();

            return clientes ?? new List<ClienteDto>();
        }

        public async Task<bool> CrearClienteAsync(CreateClienteRequest request)
        {
            var response = await _http.PostAsJsonAsync("Clientes", request);

            var raw = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW: {raw}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<ClienteDto>> ObtenerClientesAsync()
        {
            var response = await _http.GetAsync("Clientes");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON: {rawJson}");

            response.EnsureSuccessStatusCode();

            var clientes = await response.Content.ReadFromJsonAsync<List<ClienteDto>>();

            return clientes ?? new List<ClienteDto>();
        }

        public async Task<ClienteDto?> ObtenerClientePorIdAsync(int id)
        {
            var response = await _http.GetAsync($"Clientes/{id}");

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON: {rawJson}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ClienteDto>();
        }

        public async Task<bool> ActualizarClienteAsync(int id, UpdateClienteRequest request)
        {
            var response = await _http.PutAsJsonAsync($"Clientes/{id}", request);

            var rawJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"StatusCode: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"RAW JSON: {rawJson}");

            return response.IsSuccessStatusCode;
        }

    }
}