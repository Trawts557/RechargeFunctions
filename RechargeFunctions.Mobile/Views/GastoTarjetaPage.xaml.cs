
using RechargeFunctions.Mobile.Models.Recarga;
using RechargeFunctions.Mobile.Models.Tarjeta;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile.Views
{
    public partial class GastoTarjetaPage : ContentPage
    {
        private readonly RecargaApiService _recargaApiService;
        private readonly TarjetaApiService _tarjetaApiService;

        private List<RecargaDto> _recargas = new();
        private List<TarjetaDto> _tarjetas = new();
        private List<ResumenTarjetaDto> _resumenTarjetas = new();

        private const decimal GananciaPorRecargaPagada = 50m;

        public GastoTarjetaPage(
            RecargaApiService recargaApiService,
            TarjetaApiService tarjetaApiService)
        {
            InitializeComponent();
            _recargaApiService = recargaApiService;
            _tarjetaApiService = tarjetaApiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_recargas.Count == 0 || _tarjetas.Count == 0)
            {
                await CargarDatosAsync();
                ConfigurarFiltros();
            }

            AplicarFiltroYConstruirResumen();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _recargas = await _recargaApiService.ObtenerRecargasAsync();
                _tarjetas = await _tarjetaApiService.ObtenerTarjetasAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void ConfigurarFiltros()
        {
            var meses = new List<string>
    {
        "Enero","Febrero","Marzo","Abril","Mayo","Junio",
        "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
    };

            MesPicker.ItemsSource = meses;
            MesPicker.SelectedIndex = DateTime.Now.Month - 1;

            var anioActual = DateTime.Now.Year;

            var aniosData = _recargas
                .Select(r => r.FechaRecarga.Year)
                .Distinct();

            var aniosFuturos = Enumerable.Range(anioActual, 3);

            var anios = aniosData
                .Union(aniosFuturos)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            if (!anios.Any())
            {
                anios.Add(anioActual);
            }

            AnioPicker.ItemsSource = anios;
            AnioPicker.SelectedItem = anioActual;
        }

        private void OnFiltroChanged(object sender, EventArgs e)
        {
            if (MesPicker.SelectedIndex == -1 || AnioPicker.SelectedItem == null)
            {
                return;
            }

            AplicarFiltroYConstruirResumen();
        }

        private void AplicarFiltroYConstruirResumen()
        {
            if (MesPicker.SelectedIndex == -1 || AnioPicker.SelectedItem == null)
            {
                return;
            }

            int mesSeleccionado = MesPicker.SelectedIndex + 1;
            int anioSeleccionado = (int)AnioPicker.SelectedItem;

            var recargasFiltradas = _recargas
                .Where(r => r.FechaRecarga.Month == mesSeleccionado && r.FechaRecarga.Year == anioSeleccionado)
                .ToList();

            var tarjetasDict = _tarjetas.ToDictionary(t => t.Id);

            _resumenTarjetas = recargasFiltradas
                .GroupBy(r => r.TarjetaId)
                .Select(g =>
                {
                    tarjetasDict.TryGetValue(g.Key, out var tarjeta);

                    var cantidadPagadas = g.Count(r => r.EstaPagada);
                    var cantidadPendientes = g.Count(r => !r.EstaPagada);

                    return new ResumenTarjetaDto
                    {
                        TarjetaId = g.Key,
                        TarjetaNombre = tarjeta?.Nombre ?? $"Tarjeta #{g.Key}",
                        TotalGastado = g.Sum(r => r.MontoRecarga),
                        CantidadRecargas = g.Count(),
                        CantidadPagadas = cantidadPagadas,
                        CantidadPendientes = cantidadPendientes,
                        Ganancias = cantidadPagadas * GananciaPorRecargaPagada
                    };
                })
                .OrderByDescending(r => r.TotalGastado)
                .ToList();

            ResumenTarjetasCollectionView.ItemsSource = _resumenTarjetas;

            var totalGastadoGeneral = _resumenTarjetas.Sum(r => r.TotalGastado);
            var totalGananciasGeneral = _resumenTarjetas.Sum(r => r.Ganancias);
            var totalRecargasGeneral = _resumenTarjetas.Sum(r => r.CantidadRecargas);

            TotalGastadoGeneralLabel.Text = $"Total gastado: {totalGastadoGeneral:F2}";
            TotalGananciasGeneralLabel.Text = $"Ganancias: {totalGananciasGeneral:F2}";
            TotalRecargasGeneralLabel.Text = $"Recargas: {totalRecargasGeneral}";
        }
    }
}