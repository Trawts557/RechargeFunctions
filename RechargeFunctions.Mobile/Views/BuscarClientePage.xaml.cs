
using RechargeFunctions.Mobile.Models.Cliente;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class BuscarClientePage : ContentPage
    {
        private readonly ClienteApiService _clienteApiService;
        private List<ClienteDto> _clientes = new();
        private List<ClienteDto> _clientesFiltrados = new();

        public BuscarClientePage(ClienteApiService clienteApiService)
        {
            InitializeComponent();
            _clienteApiService = clienteApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarClientesAsync();
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                _clientes = await _clienteApiService.ObtenerClientesAsync();
                _clientesFiltrados = _clientes.ToList();
                ClientesCollectionView.ItemsSource = _clientesFiltrados;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = e.NewTextValue?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                _clientesFiltrados = _clientes.ToList();
            }
            else
            {
                _clientesFiltrados = _clientes
                    .Where(c =>
                        (c.Nombre?.ToLower().Contains(texto) ?? false) ||
                        (c.Apellido?.ToLower().Contains(texto) ?? false) ||
                        (c.Apodo?.ToLower().Contains(texto) ?? false) ||
                        (c.Nic?.ToLower().Contains(texto) ?? false) ||
                        (c.NumeroTelefono?.ToLower().Contains(texto) ?? false))
                    .ToList();
            }

            ClientesCollectionView.ItemsSource = _clientesFiltrados;
        }

        private async void OnClienteSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clienteSeleccionado = e.CurrentSelection.FirstOrDefault() as ClienteDto;

            if (clienteSeleccionado == null)
            {
                return;
            }

            ((CollectionView)sender).SelectedItem = null;

            await Shell.Current.GoToAsync($"{nameof(EditarClientePage)}?id={clienteSeleccionado.Id}");
        }
    }
}