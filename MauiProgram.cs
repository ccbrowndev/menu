using menu.ViewModels.Dashboard;
using menu.Views.Dashboard;
using Microsoft.Extensions.Logging;

namespace menu
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

            //views
            builder.Services.AddSingleton<DashboardPage>();

            //view models
            builder.Services.AddSingleton<DashboardPageViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}