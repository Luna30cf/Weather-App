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
                var optionsManager = new Services.OptionsManager();
                var localizationService = new Services.LocalizationService();
                localizationService.SetCulture(optionsManager.Language);

                var weatherService = new Services.WeatherService(optionsManager);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(weatherService, optionsManager, localizationService)
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
