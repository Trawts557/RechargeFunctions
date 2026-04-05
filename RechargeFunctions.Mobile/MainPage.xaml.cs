
using RechargeFunctions.Mobile.Views;

namespace RechargeFunctions.Mobile
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            
        }

        private async void OnRealizarRecargaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CrearRecargaPage));
        }

        private async void OnAgregarTarjetaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarTarjetaPage));
        }

        private async void OnAgregarClienteClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarClientePage));
        }

        private async void OnPagosPendientesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PagosPendientesPage));
        }

        private async void OnHistorialRecargasClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HistorialRecargasPage));
        }

        private async void OnListaClientesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(BuscarClientePage));
        }

        private async void OnGastoTarjetaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(GastoTarjetaPage));
        }

        private async void OnListaTarjetasClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TarjetasPage));
        }

    }
}