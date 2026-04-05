using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Models.Recarga;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    [QueryProperty(nameof(RecargaId), "id")]
    public partial class EditarRecargaPage : ContentPage
    {
        private readonly RecargaApiService _recargaApiService;
        private readonly ClienteApiService _clienteApiService;
        private readonly TarjetaApiService _tarjetaApiService;

        private List<ClienteDto> _clientes = new();
        private List<TarjetaDto> _tarjetas = new();

        public string RecargaId { get; set; } = string.Empty;

        public EditarRecargaPage(
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

            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                if (!int.TryParse(RecargaId, out int recargaId))
                {
                    await DisplayAlert("Error", "Id de recarga inválido.", "OK");
                    return;
                }

                _clientes = await _clienteApiService.ObtenerClientesAsync();
                _tarjetas = await _tarjetaApiService.ObtenerTarjetasAsync();

                ClientePicker.ItemsSource = _clientes;
                TarjetaPicker.ItemsSource = _tarjetas;

                var recarga = await _recargaApiService.ObtenerRecargaPorIdAsync(recargaId);

                if (recarga == null)
                {
                    await DisplayAlert("Error", "No se encontró la recarga.", "OK");
                    return;
                }

                ClientePicker.SelectedItem = _clientes.FirstOrDefault(c => c.Id == recarga.ClienteId);
                TarjetaPicker.SelectedItem = _tarjetas.FirstOrDefault(t => t.Id == recarga.TarjetaId);
                MontoEntry.Text = recarga.MontoRecarga.ToString("F2");
                EstaPagadaSwitch.IsToggled = recarga.EstaPagada;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(RecargaId, out int recargaId))
                {
                    await DisplayAlert("Error", "Id de recarga inválido.", "OK");
                    return;
                }

                var clienteSeleccionado = ClientePicker.SelectedItem as ClienteDto;
                var tarjetaSeleccionada = TarjetaPicker.SelectedItem as TarjetaDto;
                var montoTexto = MontoEntry.Text?.Trim() ?? string.Empty;

                if (clienteSeleccionado == null)
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

                var request = new UpdateRecargaRequest
                {
                    ClienteId = clienteSeleccionado.Id,
                    TarjetaId = tarjetaSeleccionada.Id,
                    Monto = monto,
                    EstaPagada = EstaPagadaSwitch.IsToggled
                };

                var ok = await _recargaApiService.ActualizarRecargaAsync(recargaId, request);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo actualizar la recarga.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Recarga actualizada correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}