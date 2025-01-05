using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.UI.Models;

namespace WeatherApp.UI.Services

{
    public class WeatherService
    {
        private readonly string _apiKey;
        private readonly OptionsManager _optionsManager;

        public WeatherService(OptionsManager optionsManager)
        {
            _optionsManager = optionsManager;
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(configPath))
                throw new FileNotFoundException("Le fichier config.json (avec la clé API) est introuvable.");

            dynamic config = JsonConvert.DeserializeObject(File.ReadAllText(configPath));
            _apiKey = config.ApiKey;
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return null;

            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang={_optionsManager.Language}";

            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            var apiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(response);
            if (apiResponse?.main == null || apiResponse?.weather == null || apiResponse.weather.Length == 0)
                return null;

            return new WeatherData
            {
                CityName = apiResponse.name,
                Latitude = apiResponse.coord.lat,
                Longitude = apiResponse.coord.lon,
                Temperature = $"{apiResponse.main.temp:0.0} °C",
                Description = apiResponse.weather[0].description,
                Humidity = apiResponse.main.humidity,
                IconUrl = $"https://openweathermap.org/img/wn/{apiResponse.weather[0].icon}@2x.png"
            };
        }

        public async Task<ForecastItem[]> GetForecastAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_apiKey}&units=metric&lang={_optionsManager.Language}";
            using var cli = new HttpClient();
            var res = await cli.GetStringAsync(url);
            var r = JsonConvert.DeserializeObject<ForecastApiResponse>(res);
            if (r?.list == null) return null;
            var list = r.list
                .Where(x => x.dt_txt.Contains("12:00:00"))
                .Take(5)
                .Select(x => new ForecastItem
                {
                    DateText = x.dt_txt,
                    Temp = $"{x.main.temp:0.0} °C",
                    Description = x.weather.FirstOrDefault()?.description
                })
                .ToArray();
            return list; // Retourne le tableau des prévisions.
        }

        // Classes de parsing internes
        private class WeatherApiResponse
        {
            public string name { get; set; }
            public Coord coord { get; set; }
            public MainObj main { get; set; }
            public WeatherObj[] weather { get; set; }
        }

        private class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        private class MainObj
        {
            public float temp { get; set; }
            public int humidity { get; set; }
        }

        private class WeatherObj
        {
            public string description { get; set; }
            public string icon { get; set; }
        }

        private class ForecastApiResponse
        {
            public ForecastApiItem[] list { get; set; }
        }

        private class ForecastApiItem
        {
            public string dt_txt { get; set; }
            public MainObj main { get; set; }
            public WeatherObj[] weather { get; set; }
        }
    }
}
