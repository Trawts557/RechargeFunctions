using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            builder.Configuration.AddConfiguration(config);

            builder.Services.AddHttpClient<ClienteApiService>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ApiSettings:BaseUrl"]
                    ?? throw new InvalidOperationException("ApiSettings:BaseUrl no está configurado.");

                client.BaseAddress = new Uri(baseUrl);
            });

            builder.Services.AddHttpClient<RecargaApiService>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ApiSettings:BaseUrl"]
                    ?? throw new InvalidOperationException("ApiSettings:BaseUrl no está configurado.");

                client.BaseAddress = new Uri(baseUrl);
            });

            builder.Services.AddHttpClient<TarjetaApiService>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ApiSettings:BaseUrl"]
                    ?? throw new InvalidOperationException("ApiSettings:BaseUrl no está configurado.");

                client.BaseAddress = new Uri(baseUrl);
            });

            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}