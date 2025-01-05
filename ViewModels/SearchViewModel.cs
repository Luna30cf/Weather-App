using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using WeatherApp.UI.Services;

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

        public ReactiveCommand<Unit, Unit> SearchCommand { get; }

        private readonly WeatherService _svc;

        public SearchViewModel(WeatherService svc)
        {
            _svc = svc;
            SearchCommand = ReactiveCommand.CreateFromTask(ExecuteSearchAsync,
                this.WhenAnyValue(x => x.CityName, c => !string.IsNullOrWhiteSpace(c)));
        }

        private async Task ExecuteSearchAsync()
        {
            try
            {
                var data = await _svc.GetWeatherAsync(CityName);
                if (data == null)
                {
                    Temperature = "Ville introuvable";
                    Description = "";
                }
                else
                {
                    Temperature = data.Temperature + " °C";
                    Description = data.Description;
                }
            }
            catch (Exception ex)
            {
                Temperature = "Erreur";
                Description = ex.Message;
            }
        }
    }
}
