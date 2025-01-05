using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using WeatherApp.UI.Services;
using WeatherApp.UI.Models;

namespace WeatherApp.UI.ViewModels
{
    public class ForecastViewModel : ReactiveObject
    {
        public ObservableCollection<ForecastItem> Forecasts { get; } = new ObservableCollection<ForecastItem>();
        public ReactiveCommand<Unit, Unit> LoadForecastCommand { get; }

        private readonly WeatherService _weatherService;
        private readonly OptionsManager _optionsManager;
        private readonly LocalizationService _localizationService;

        public ForecastViewModel(WeatherService weatherService, OptionsManager optionsManager, LocalizationService localizationService)
        {
            _weatherService = weatherService;
            _optionsManager = optionsManager;
            _localizationService = localizationService;

            // Initialiser avec la ville par défaut
            CityName = _optionsManager.DefaultCity;

            LoadForecastCommand = ReactiveCommand.CreateFromTask(ExecuteLoadForecastAsync,
                this.WhenAnyValue(x => x.CityName, city => !string.IsNullOrWhiteSpace(city)));

            // Abonnement à l'événement CultureChanged si nécessaire
            _localizationService.CultureChanged += OnCultureChanged;
        }

        private string _cityName;
        public string CityName
        {
            get => _cityName;
            set => this.RaiseAndSetIfChanged(ref _cityName, value);
        }

        private async Task ExecuteLoadForecastAsync()
        {
            try
            {
                Forecasts.Clear();
                var forecastData = await _weatherService.GetForecastAsync(CityName);
                if (forecastData != null)
                {
                    foreach (var item in forecastData)
                    {
                        Forecasts.Add(item);
                    }
                }
                else
                {
                    Forecasts.Add(new ForecastItem
                    {
                        DateText = _localizationService.GetString("CityNotFound"),
                        Temp = "",
                        Description = ""
                    });
                }
            }
            catch (Exception ex)
            {
                Forecasts.Add(new ForecastItem
                {
                    DateText = _localizationService.GetString("ErrorLabel"),
                    Temp = "",
                    Description = ex.Message
                });
            }
        }

        // Propriétés traduites

        public string ForecastTitle => _localizationService.GetString("ForecastTitle");
        public string LoadForecastButton => _localizationService.GetString("LoadForecastButton");

        private void OnCultureChanged()
        {
            // Notifier que les propriétés traduites ont changé
            this.RaisePropertyChanged(nameof(ForecastTitle));
            this.RaisePropertyChanged(nameof(LoadForecastButton));
        }

        // Désabonnement de l'événement pour éviter les fuites de mémoire
        ~ForecastViewModel()
        {
            _localizationService.CultureChanged -= OnCultureChanged;
        }
    }
}
