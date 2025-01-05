using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WeatherApp.UI.ViewModels;
using WeatherApp.UI.Views;

namespace WeatherApp.UI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var weatherService = new Services.WeatherService();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new SearchViewModel(weatherService)
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
