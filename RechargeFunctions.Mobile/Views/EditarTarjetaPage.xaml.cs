using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    [QueryProperty(nameof(TarjetaId), "id")]
    public partial class EditarTarjetaPage : ContentPage
    {
        private readonly TarjetaApiService _tarjetaApiService;

        public string TarjetaId { get; set; } = string.Empty;

        private TarjetaDto? _tarjetaActual;

        public EditarTarjetaPage(TarjetaApiService tarjetaApiService)
        {
            InitializeComponent();
            _tarjetaApiService = tarjetaApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarTarjetaAsync();
        }

        private async Task CargarTarjetaAsync()
        {
            try
            {
                if (!int.TryParse(TarjetaId, out int tarjetaId))
                {
                    await DisplayAlert("Error", "Id de tarjeta inválido.", "OK");
                    return;
                }

                var tarjeta = await _tarjetaApiService.ObtenerTarjetaPorIdAsync(tarjetaId);

                if (tarjeta == null)
                {
                    await DisplayAlert("Error", "No se encontró la tarjeta.", "OK");
                    return;
                }

                _tarjetaActual = tarjeta;

                NombreEntry.Text = tarjeta.Nombre;
                UltimosDigitosEntry.Text = tarjeta.UltimosDigitos;
                EstadoLabel.Text = $"Estado: {tarjeta.EstadoTexto}";

                CambiarEstadoButton.Text = tarjeta.IsActive
                    ? "Desactivar Tarjeta"
                    : "Activar Tarjeta";
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
                if (!int.TryParse(TarjetaId, out int tarjetaId))
                {
                    await DisplayAlert("Error", "Id de tarjeta inválido.", "OK");
                    return;
                }

                var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
                var ultimosDigitos = UltimosDigitosEntry.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    await DisplayAlert("Validación", "Debe ingresar el nombre.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ultimosDigitos))
                {
                    await DisplayAlert("Validación", "Debe ingresar los últimos dígitos.", "OK");
                    return;
                }

                if (ultimosDigitos.Length < 4 || !ultimosDigitos.All(char.IsDigit))
                {
                    await DisplayAlert("Validación", "Los últimos dígitos deben tener al menos 4 números.", "OK");
                    return;
                }

                var request = new UpdateTarjetaRequest
                {
                    Nombre = nombre,
                    UltimosDigitos = ultimosDigitos
                };

                var ok = await _tarjetaApiService.ActualizarTarjetaAsync(tarjetaId, request);

                if (!ok)
                {
                    await DisplayAlert("Error", "No se pudo actualizar la tarjeta.", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Tarjeta actualizada correctamente.", "OK");
                await CargarTarjetaAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnCambiarEstadoClicked(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(TarjetaId, out int tarjetaId))
                {
                    await DisplayAlert("Error", "Id de tarjeta inválido.", "OK");
                    return;
                }

                if (_tarjetaActual == null)
                {
                    await DisplayAlert("Error", "No se pudo determinar el estado actual de la tarjeta.", "OK");
                    return;
                }

                bool esActiva = _tarjetaActual.IsActive;

                bool confirmar = await DisplayAlert(
                    "Confirmación",
                    esActiva
                        ? "¿Desea desactivar esta tarjeta?"
                        : "¿Desea activar esta tarjeta?",
                    "Sí",
                    "No");

                if (!confirmar)
                {
                    return;
                }

                bool ok;

                if (esActiva)
                {
                    ok = await _tarjetaApiService.DesactivarTarjetaAsync(tarjetaId);
                }
                else
                {
                    ok = await _tarjetaApiService.ActivarTarjetaAsync(tarjetaId);
                }

                if (!ok)
                {
                    await DisplayAlert(
                        "Error",
                        esActiva
                            ? "No se pudo desactivar la tarjeta."
                            : "No se pudo activar la tarjeta.",
                        "OK");
                    return;
                }

                await DisplayAlert(
                    "Éxito",
                    esActiva
                        ? "Tarjeta desactivada correctamente."
                        : "Tarjeta activada correctamente.",
                    "OK");

                await CargarTarjetaAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}