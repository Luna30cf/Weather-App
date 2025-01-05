using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using WeatherApp.UI.Models;
using WeatherApp.UI.Services;
using System.Linq;


namespace WeatherApp.UI.ViewModels
{
    public class ForecastViewModel : ReactiveObject
    {
        private string _cityName;
        public string CityName
        {
            get => _cityName;
            set => this.RaiseAndSetIfChanged(ref _cityName, value);
        }

        public ObservableCollection<ForecastItem> Forecasts { get; } = new ObservableCollection<ForecastItem>();
        public ReactiveCommand<Unit, Unit> LoadForecastCommand { get; }
        private readonly WeatherService _svc;


        public ForecastViewModel(WeatherService svc)
        {
            _svc = svc;
            LoadForecastCommand = ReactiveCommand.CreateFromTask(LoadForecastAsync);
        }

        private async Task LoadForecastAsync()
        {
            Forecasts.Clear();
            try
            {
                var list = await _svc.GetForecastAsync(CityName);
                if (list != null)
                {
                    foreach (var it in list)
                        Forecasts.Add(it);
                }
            }
            catch (Exception ex)
            {
                Forecasts.Add(new ForecastItem { DateText = "Erreur", Temp = ex.Message });
            }
        }
    }
}
