
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

        public HistorialRecargasPage(RecargaApiService recargaApiService, 
            TarjetaApiService tarjetaApiService, ClienteApiService clienteApiService)
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

                foreach (var r in recargas)
                {
                    if (clientesDict.TryGetValue(r.ClienteId, out var cliente))
                    {
                        r.ClienteNombre = $"{cliente.Nombre} {cliente.Apellido}";
                    }
                    else
                    {
                        r.ClienteNombre = $"Cliente #{r.ClienteId}";
                    }

                    if (tarjetasDict.TryGetValue(r.TarjetaId, out var tarjeta))
                    {
                        r.TarjetaNombre = tarjeta.Nombre;
                    }
                    else
                    {
                        r.TarjetaNombre = $"Tarjeta #{r.TarjetaId}";
                    }
                }

                _recargas = recargas
                    .OrderByDescending(r => r.FechaRecarga)
                    .ToList();

                _recargasFiltradas = _recargas.ToList();
                RecargasCollectionView.ItemsSource = _recargasFiltradas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnBuscarRecargaTextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = e.NewTextValue?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                _recargasFiltradas = _recargas.ToList();
            }
            else
            {
                _recargasFiltradas = _recargas
                    .Where(r =>
                        (r.ClienteNombre?.ToLower().Contains(texto) ?? false) ||
                        (r.TarjetaNombre?.ToLower().Contains(texto) ?? false))
                    .ToList();
            }

            RecargasCollectionView.ItemsSource = _recargasFiltradas;
        }

        private async void OnRecargaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var recargaSeleccionada = e.CurrentSelection.FirstOrDefault() as RecargaDto;

            if (recargaSeleccionada == null)
            {
                return;
            }

            ((CollectionView)sender).SelectedItem = null;

            await Shell.Current.GoToAsync($"{nameof(EditarRecargaPage)}?id={recargaSeleccionada.Id}");
        }
    }
}