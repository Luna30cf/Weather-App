using ReactiveUI;
using WeatherApp.UI.Services;

namespace WeatherApp.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public SearchViewModel SearchVM { get; }
        public ForecastViewModel ForecastVM { get; }
        public SettingsViewModel SettingsVM { get; }

        private readonly LocalizationService _localizationService;

        public MainWindowViewModel(WeatherService weatherService, OptionsManager optionsManager, LocalizationService localizationService)
        {
            _localizationService = localizationService;

            SearchVM = new SearchViewModel(weatherService, optionsManager, _localizationService);
            ForecastVM = new ForecastViewModel(weatherService, optionsManager, _localizationService);
            SettingsVM = new SettingsViewModel(optionsManager, _localizationService);
        }
    }
}
