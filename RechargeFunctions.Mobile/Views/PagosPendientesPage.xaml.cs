using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Models.Recarga;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class PagosPendientesPage : ContentPage
    {
        private readonly RecargaApiService _recargaApiService;
        private readonly ClienteApiService _clienteApiService;
        private readonly TarjetaApiService _tarjetaApiService;

        private List<RecargaDto> _recargasPendientes = new();
        private List<ClienteDto> _clientes = new();
        private List<TarjetaDto> _tarjetas = new();

        public PagosPendientesPage(
            RecargaApiService recargaApiService,
            ClienteApiService clienteApiService,
            TarjetaApiService tarjetaApiService)
        {
            InitializeComponent();
            _recargaApiService = recargaApiService;
            _clienteApiService = clienteApiService;
            _tarjetaApiService = tarjetaApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarPendientesAsync();
        }

        private async Task CargarPendientesAsync()
        {
            try
            {
                _clientes = await _clienteApiService.ObtenerClientesAsync();
                _tarjetas = await _tarjetaApiService.ObtenerTarjetasAsync();

                var recargas = await _recargaApiService.ObtenerRecargasAsync();

                var clientesDict = _clientes.ToDictionary(c => c.Id);
                var tarjetasDict = _tarjetas.ToDictionary(t => t.Id);

                foreach (var recarga in recargas)
                {
                    if (clientesDict.TryGetValue(recarga.ClienteId, out var cliente))
                    {
                        recarga.ClienteNombre = $"{cliente.Nombre} {cliente.Apellido}";
                    }
                    else
                    {
                        recarga.ClienteNombre = $"Cliente #{recarga.ClienteId}";
                    }

                    if (tarjetasDict.TryGetValue(recarga.TarjetaId, out var tarjeta))
                    {
                        recarga.TarjetaNombre = tarjeta.Nombre;
                    }
                    else
                    {
                        recarga.TarjetaNombre = $"Tarjeta #{recarga.TarjetaId}";
                    }
                }

                _recargasPendientes = recargas
                    .Where(r => !r.EstaPagada)
                    .OrderByDescending(r => r.FechaRecarga)
                    .ToList();

                RecargasCollectionView.ItemsSource = _recargasPendientes;

                if (_recargasPendientes.Count == 0)
                {
                    await DisplayAlert("Info", "No hay deudas pendientes.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnMarcarPagadaClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is not Button button)
                {
                    return;
                }

                if (button.CommandParameter is not int recargaId)
                {
                    return;
                }

                bool confirmar = await DisplayAlert(
                    "Confirmación",
                    "¿Desea marcar esta recarga como pagada?",
                    "Sí",
                    "No");

                if (!confirmar)
                {
                    return;
                }

                var ok = await _recargaApiService.MarcarComoPagadaAsync(recargaId);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo marcar la recarga como pagada.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Recarga marcada como pagada.", "OK");
                await CargarPendientesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}