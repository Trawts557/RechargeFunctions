using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class AgregarClientePage : ContentPage
    {
        private readonly ClienteApiService _clienteApiService;

        public AgregarClientePage(ClienteApiService clienteApiService)
        {
            InitializeComponent();
            _clienteApiService = clienteApiService;
        }

        private async void OnGuardarClienteClicked(object sender, EventArgs e)
        {
            try
            {
                var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
                var apellido = ApellidoEntry.Text?.Trim() ?? string.Empty;
                var apodo = ApodoEntry.Text?.Trim() ?? string.Empty;
                var nic = NicEntry.Text?.Trim() ?? string.Empty;
                var numeroTelefono = NumeroTelefonoEntry.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apodo))
                {
                    await DisplayAlert("Validación", "Debe ingresar el nombre o el apodo.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(nic))
                {
                    await DisplayAlert("Validación", "Debe ingresar el NIC.", "OK");
                    return;
                }

                var request = new CreateClienteRequest
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Apodo = apodo, 
                    Nic = nic,
                    NumeroTelefono = numeroTelefono
                };

                var ok = await _clienteApiService.CrearClienteAsync(request);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo guardar el cliente.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Cliente guardado correctamente.", "OK");

                NombreEntry.Text = string.Empty;
                ApellidoEntry.Text = string.Empty;
                ApodoEntry.Text = string.Empty;
                NicEntry.Text = string.Empty;
                NumeroTelefonoEntry.Text = string.Empty;

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}