using CleanAndWinApp.Data;
using Microsoft.Extensions.Logging;
using Model;
using ZstdSharp.Unsafe;

namespace CleanAndWinApp
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<Data.BrowserService>();
            builder.Services.AddSingleton<Data.BrowserDimension>();
            
            builder.Services.AddSingleton<Services.ProductService>();
            builder.Services.AddSingleton<LocationService>();


            return builder.Build();
        }
    }
}