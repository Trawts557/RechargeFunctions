using RechargeFunctions.Mobile.Models;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class TarjetasPage : ContentPage
    {
        private readonly TarjetaApiService _tarjetaApiService;

        private List<TarjetaDto> _tarjetas = new();
        private List<TarjetaDto> _tarjetasFiltradas = new();

        public TarjetasPage(TarjetaApiService tarjetaApiService)
        {
            InitializeComponent();
            _tarjetaApiService = tarjetaApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarTarjetasAsync();
        }

        private async Task CargarTarjetasAsync()
        {
            try
            {
                _tarjetas = await _tarjetaApiService.ObtenerTarjetasAsync();
                _tarjetasFiltradas = _tarjetas.ToList();
                TarjetasCollectionView.ItemsSource = _tarjetasFiltradas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnBuscarTarjetaTextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = e.NewTextValue?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                _tarjetasFiltradas = _tarjetas.ToList();
            }
            else
            {
                _tarjetasFiltradas = _tarjetas
                    .Where(t =>
                        (t.Nombre?.ToLower().Contains(texto) ?? false) ||
                        (t.UltimosDigitos?.ToLower().Contains(texto) ?? false))
                    .ToList();
            }

            TarjetasCollectionView.ItemsSource = _tarjetasFiltradas;
        }

        private async void OnTarjetaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tarjetaSeleccionada = e.CurrentSelection.FirstOrDefault() as TarjetaDto;

            if (tarjetaSeleccionada == null)
            {
                return;
            }

            ((CollectionView)sender).SelectedItem = null;

            await Shell.Current.GoToAsync($"{nameof(EditarTarjetaPage)}?id={tarjetaSeleccionada.Id}");
        }
    }
}