using RechargeFunctions.Mobile.Views;

namespace RechargeFunctions.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(BuscarClientePage), typeof(BuscarClientePage));
            Routing.RegisterRoute(nameof(AgregarTarjetaPage), typeof(AgregarTarjetaPage));
            Routing.RegisterRoute(nameof(AgregarClientePage), typeof(AgregarClientePage));
            Routing.RegisterRoute(nameof(EditarClientePage), typeof(EditarClientePage));
            Routing.RegisterRoute(nameof(CrearRecargaPage), typeof(CrearRecargaPage));
            Routing.RegisterRoute(nameof(PagosPendientesPage), typeof(PagosPendientesPage));
            Routing.RegisterRoute(nameof(HistorialRecargasPage), typeof(HistorialRecargasPage));
            Routing.RegisterRoute(nameof(EditarRecargaPage), typeof(EditarRecargaPage));
            Routing.RegisterRoute(nameof(GastoTarjetaPage), typeof(GastoTarjetaPage));
            Routing.RegisterRoute(nameof(TarjetasPage), typeof(TarjetasPage));
            Routing.RegisterRoute(nameof(EditarTarjetaPage), typeof(EditarTarjetaPage));



        }
    }
}
