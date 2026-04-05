using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class CrearRecargaPage : ContentPage
    {
        private readonly ClienteApiService _clienteApiService;
        private readonly TarjetaApiService _tarjetaApiService;
        private readonly RecargaApiService _recargaApiService;

        private List<ClienteDto> _clientes = new();
        private List<ClienteDto> _clientesFiltrados = new();
        private List<TarjetaDto> _tarjetas = new();

        private ClienteDto? _clienteSeleccionado;

        public CrearRecargaPage(
            ClienteApiService clienteApiService,
            TarjetaApiService tarjetaApiService,
            RecargaApiService recargaApiService)
        {
            InitializeComponent();
            _clienteApiService = clienteApiService;
            _tarjetaApiService = tarjetaApiService;
            _recargaApiService = recargaApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_clientes.Count == 0 || _tarjetas.Count == 0)
            {
                await CargarDatosAsync();
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _clientes = await _clienteApiService.ObtenerClientesAsync();
                _tarjetas = await _tarjetaApiService.ObtenerTarjetasAsync();

                _clientesFiltrados = new List<ClienteDto>();
                ClientePicker.ItemsSource = _clientesFiltrados;

                TarjetaPicker.ItemsSource = _tarjetas;

                System.Diagnostics.Debug.WriteLine($"Clientes cargados: {_clientes.Count}");
                System.Diagnostics.Debug.WriteLine($"Tarjetas cargadas: {_tarjetas.Count}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnClienteSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = e.NewTextValue?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                _clientesFiltrados = new List<ClienteDto>();
                ClientePicker.ItemsSource = _clientesFiltrados;
                return;
            }

            _clientesFiltrados = _clientes
                .Where(c =>
                    (c.Nombre?.ToLower().Contains(texto) ?? false) ||
                    (c.Apellido?.ToLower().Contains(texto) ?? false) ||
                    (c.Apodo?.ToLower().Contains(texto) ?? false) ||
                    (c.Nic?.ToLower().Contains(texto) ?? false) ||
                    (c.NumeroTelefono?.ToLower().Contains(texto) ?? false))
                .ToList();

            ClientePicker.ItemsSource = _clientesFiltrados;
        }

        private void OnClientePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var cliente = ClientePicker.SelectedItem as ClienteDto;

            if (cliente == null)
            {
                return;
            }

            _clienteSeleccionado = cliente;
            ClienteSeleccionadoLabel.Text = cliente.DisplayText;
        }

        private async void OnGuardarRecargaClicked(object sender, EventArgs e)
        {
            try
            {
                var tarjetaSeleccionada = TarjetaPicker.SelectedItem as TarjetaDto;
                var montoTexto = MontoEntry.Text?.Trim() ?? string.Empty;

                if (_clienteSeleccionado == null)
                {
                    await DisplayAlert("Validación", "Debe seleccionar un cliente.", "OK");
                    return;
                }

                if (tarjetaSeleccionada == null)
                {
                    await DisplayAlert("Validación", "Debe seleccionar una tarjeta.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(montoTexto))
                {
                    await DisplayAlert("Validación", "Debe ingresar el monto.", "OK");
                    return;
                }

                if (!decimal.TryParse(montoTexto, out decimal monto))
                {
                    await DisplayAlert("Validación", "Debe ingresar un monto válido.", "OK");
                    return;
                }

                if (monto <= 0)
                {
                    await DisplayAlert("Validación", "El monto debe ser mayor que cero.", "OK");
                    return;
                }

                var request = new CreateRecargaRequest
                {
                    ClienteId = _clienteSeleccionado.Id,
                    TarjetaId = tarjetaSeleccionada.Id,
                    MontoRecarga = monto,
                    EstaPagada = EstaPagadaSwitch.IsToggled
                };

                var ok = await _recargaApiService.CrearRecargaAsync(request);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo guardar la recarga.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Recarga guardada correctamente.", "OK");

                LimpiarFormulario();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void LimpiarFormulario()
        {
            _clienteSeleccionado = null;
            ClienteSeleccionadoLabel.Text = "Ninguno";

            ClienteSearchBar.Text = string.Empty;
            _clientesFiltrados = new List<ClienteDto>();
            ClientePicker.ItemsSource = _clientesFiltrados;
            ClientePicker.SelectedItem = null;

            TarjetaPicker.SelectedItem = null;
            MontoEntry.Text = string.Empty;
            EstaPagadaSwitch.IsToggled = false;
        }
    }
}