using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Models.Recarga;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class HistorialRecargasPage : ContentPage
    {
        private readonly RecargaApiService _recargaApiService;
        private readonly TarjetaApiService _tarjetaApiService;
        private readonly ClienteApiService _clienteApiService;

        private List<RecargaDto> _recargas = new();
        private List<RecargaDto> _recargasFiltradas = new();
        private List<TarjetaDto> _tarjetas = new();
        private List<ClienteDto> _clientes = new();

        public HistorialRecargasPage(
            RecargaApiService recargaApiService,
            TarjetaApiService tarjetaApiService,
            ClienteApiService clienteApiService)
        {
            InitializeComponent();
            _recargaApiService = recargaApiService;
            _tarjetaApiService = tarjetaApiService;
            _clienteApiService = clienteApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarRecargasAsync();
        }

        private async Task CargarRecargasAsync()
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

                _recargas = OrdenarRecargasPorFechaDesc(recargas);
                _recargasFiltradas = _recargas.ToList();

                RecargasCollectionView.ItemsSource = null;
                RecargasCollectionView.ItemsSource = _recargasFiltradas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private static List<RecargaDto> OrdenarRecargasPorFechaDesc(IEnumerable<RecargaDto> recargas)
        {
            return recargas
                .OrderByDescending(r => r.FechaRecarga)
                .ToList();
        }

        private void OnBuscarRecargaTextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = e.NewTextValue?.Trim().ToLowerInvariant() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                _recargasFiltradas = _recargas.ToList();
            }
            else
            {
                _recargasFiltradas = _recargas
                    .Where(r =>
                        (r.ClienteNombre?.ToLowerInvariant().Contains(texto) ?? false) ||
                        (r.TarjetaNombre?.ToLowerInvariant().Contains(texto) ?? false))
                    .OrderByDescending(r => r.FechaRecarga)
                    .ToList();
            }

            RecargasCollectionView.ItemsSource = null;
            RecargasCollectionView.ItemsSource = _recargasFiltradas;
        }

        private async void OnRecargaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var recargaSeleccionada = e.CurrentSelection.FirstOrDefault() as RecargaDto;

                if (recargaSeleccionada == null)
                {
                    return;
                }

                ((CollectionView)sender).SelectedItem = null;

                await Shell.Current.GoToAsync($"{nameof(EditarRecargaPage)}?id={recargaSeleccionada.Id}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}