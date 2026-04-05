using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class AgregarTarjetaPage : ContentPage
    {
        private readonly TarjetaApiService _tarjetaApiService;

        public AgregarTarjetaPage(TarjetaApiService tarjetaApiService)
        {
            InitializeComponent();
            _tarjetaApiService = tarjetaApiService;
        }

        private async void OnGuardarTarjetaClicked(object sender, EventArgs e)
        {
            try
            {
                var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
                var ultimosDigitos = UltimosDigitosEntry.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    await DisplayAlert("Validación", "Debe ingresar el nombre de la tarjeta.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ultimosDigitos))
                {
                    await DisplayAlert("Validación", "Debe ingresar los últimos dígitos.", "OK");
                    return;
                }

                if (ultimosDigitos.Length < 3 || !ultimosDigitos.All(char.IsDigit))
                {
                    await DisplayAlert("Validación", "Los últimos dígitos deben ser 3 números o mas.", "OK");
                    return;
                }

                var request = new CreateTarjetaRequest
                {
                    Nombre = nombre,
                    UltimosDigitos = ultimosDigitos
                };

                var ok = await _tarjetaApiService.CrearTarjetaAsync(request);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo guardar la tarjeta.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Tarjeta guardada correctamente.", "OK");

                NombreEntry.Text = string.Empty;
                UltimosDigitosEntry.Text = string.Empty;

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}