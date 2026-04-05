
using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    [QueryProperty(nameof(ClienteId), "id")]
    public partial class EditarClientePage : ContentPage
    {
        private readonly ClienteApiService _clienteApiService;

        public string ClienteId { get; set; } = string.Empty;

        public EditarClientePage(ClienteApiService clienteApiService)
        {
            InitializeComponent();
            _clienteApiService = clienteApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarClienteAsync();
        }

        private async Task CargarClienteAsync()
        {
            try
            {
                if (!int.TryParse(ClienteId, out int clienteId))
                {
                    await DisplayAlert("Error", "Id de cliente inválido.", "OK");
                    return;
                }

                var cliente = await _clienteApiService.ObtenerClientePorIdAsync(clienteId);

                if (cliente == null)
                {
                    await DisplayAlert("Error", "No se encontró el cliente.", "OK");
                    return;
                }

                NombreEntry.Text = cliente.Nombre;
                ApellidoEntry.Text = cliente.Apellido;
                ApodoEntry.Text = cliente.Apodo;
                NicEntry.Text = cliente.Nic;
                NumeroTelefonoEntry.Text = cliente.NumeroTelefono;
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
                if (!int.TryParse(ClienteId, out int clienteId))
                {
                    await DisplayAlert("Error", "Id de cliente inválido.", "OK");
                    return;
                }

                var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
                var apellido = ApellidoEntry.Text?.Trim() ?? string.Empty;
                var apodo = ApodoEntry.Text?.Trim() ?? string.Empty;
                var nic = NicEntry.Text?.Trim() ?? string.Empty;
                var numeroTelefono = NumeroTelefonoEntry.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombre) && (string.IsNullOrWhiteSpace(apodo)))
                {
                    await DisplayAlert("Validación", "Debe ingresar el nombre o apodo.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(nic))
                {
                    await DisplayAlert("Validación", "Debe ingresar el NIC.", "OK");
                    return;
                }

                var request = new UpdateClienteRequest
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Apodo = apodo,
                    Nic = nic,
                    NumeroTelefono = numeroTelefono
                };

                var ok = await _clienteApiService.ActualizarClienteAsync(clienteId, request);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo actualizar el cliente.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Cliente actualizado correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}