using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using WeatherApp.UI.Services;
using WeatherApp.UI.Models;

namespace WeatherApp.UI.ViewModels
{
    public class SearchViewModel : ReactiveObject
    {
        private string _cityName;
        public string CityName
        {
            get => _cityName;
            set => this.RaiseAndSetIfChanged(ref _cityName, value);
        }

        private string _temperature;
        public string Temperature
        {
            get => _temperature;
            set => this.RaiseAndSetIfChanged(ref _temperature, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => this.RaiseAndSetIfChanged(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => this.RaiseAndSetIfChanged(ref _longitude, value);
        }

        private int _humidity;
        public int Humidity
        {
            get => _humidity;
            set => this.RaiseAndSetIfChanged(ref _humidity, value);
        }

        private string _iconUrl;
        public string IconUrl
        {
            get => _iconUrl;
            set => this.RaiseAndSetIfChanged(ref _iconUrl, value);
        }

        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadDefaultCityCommand { get; }

        private readonly WeatherService _weatherService;
        private readonly OptionsManager _optionsManager;
        private readonly LocalizationService _localizationService;

        public SearchViewModel(WeatherService weatherService, OptionsManager optionsManager, LocalizationService localizationService)
        {
            _weatherService = weatherService;
            _optionsManager = optionsManager;
            _localizationService = localizationService;

            // Utiliser la ville par défaut initialement
            CityName = _optionsManager.DefaultCity;

            SearchCommand = ReactiveCommand.CreateFromTask(ExecuteSearchAsync,
                this.WhenAnyValue(x => x.CityName, city => !string.IsNullOrWhiteSpace(city)));

            LoadDefaultCityCommand = ReactiveCommand.Create(LoadDefaultCity);

            // Abonnement à l'événement CultureChanged
            _localizationService.CultureChanged += OnCultureChanged;

            // Effectuer une recherche initiale pour la ville par défaut
            Task.Run(async () => await ExecuteSearchAsync());
        }

        private async Task ExecuteSearchAsync()
        {
            try
            {
                var data = await _weatherService.GetWeatherAsync(CityName);
                if (data == null)
                {
                    Temperature = _localizationService.GetString("CityNotFound");
                    Description = "";
                    Latitude = 0;
                    Longitude = 0;
                    Humidity = 0;
                    IconUrl = null;
                }
                else
                {
                    Temperature = data.Temperature;
                    Description = data.Description;
                    Latitude = data.Latitude;
                    Longitude = data.Longitude;
                    Humidity = data.Humidity;
                    IconUrl = data.IconUrl;
                }
            }
            catch (Exception ex)
            {
                Temperature = _localizationService.GetString("ErrorLabel");
                Description = ex.Message;
                Latitude = 0;
                Longitude = 0;
                Humidity = 0;
                IconUrl = null;
            }
        }

        private void LoadDefaultCity()
        {
            CityName = _optionsManager.DefaultCity;
            SearchCommand.Execute().Subscribe();
        }

        // Propriétés pour les chaînes traduites

        public string SearchTitle => _localizationService.GetString("SearchCityTitle");
        public string CityLabel => _localizationService.GetString("CityLabel");
        public string SearchButton => _localizationService.GetString("SearchButton");
        public string TemperatureLabel => _localizationService.GetString("TemperatureLabel");
        public string DescriptionLabel => _localizationService.GetString("DescriptionLabel");
        public string LatitudeLabel => _localizationService.GetString("LatitudeLabel");
        public string LongitudeLabel => _localizationService.GetString("LongitudeLabel");
        public string HumidityLabel => _localizationService.GetString("HumidityLabel");
        public string LoadDefaultCityButton => _localizationService.GetString("LoadDefaultCityButton");

        private void OnCultureChanged()
        {
            // Notifier que les propriétés traduites ont changé
            this.RaisePropertyChanged(nameof(SearchTitle));
            this.RaisePropertyChanged(nameof(CityLabel));
            this.RaisePropertyChanged(nameof(SearchButton));
            this.RaisePropertyChanged(nameof(TemperatureLabel));
            this.RaisePropertyChanged(nameof(DescriptionLabel));
            this.RaisePropertyChanged(nameof(LatitudeLabel));
            this.RaisePropertyChanged(nameof(LongitudeLabel));
            this.RaisePropertyChanged(nameof(HumidityLabel));
            this.RaisePropertyChanged(nameof(LoadDefaultCityButton));
        }

        // Désabonnement de l'événement pour éviter les fuites de mémoire
        ~SearchViewModel()
        {
            _localizationService.CultureChanged -= OnCultureChanged;
        }
    }
}
