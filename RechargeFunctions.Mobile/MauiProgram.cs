
using Microsoft.Extensions.Logging;
using RechargeFunctions.Mobile.Services;

namespace RechargeFunctions.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient<ClienteApiService>(client =>
            {
                client.BaseAddress = new Uri("https://recharge-api-3off.onrender.com/api/");
            });

            builder.Services.AddHttpClient<RecargaApiService>(client =>
            {
                client.BaseAddress = new Uri("https://recharge-api-3off.onrender.com/api/");
            });

            builder.Services.AddHttpClient<TarjetaApiService>(client =>
            {
                client.BaseAddress = new Uri("https://recharge-api-3off.onrender.com/api/");
            });

            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
