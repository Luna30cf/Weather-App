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
        private readonly string _apiKey; // Clé API pour l'accès à OpenWeatherMap.
        public WeatherService()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(path)) throw new FileNotFoundException("config.json introuvable");
            dynamic cfg = JsonConvert.DeserializeObject(File.ReadAllText(path));
            _apiKey = cfg.ApiKey; // Assigne la clé API.
        }
        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return null; //retourner la ville par défault au lieu de null !!!!!


            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric"; // URL de l'API pour récupérer la météo actuelle (formatée avec la ville et la clé API).

            using var cli = new HttpClient();
            var res = await cli.GetStringAsync(url);
            var r = JsonConvert.DeserializeObject<WeatherApiResponse>(res);
            if (r?.main == null || r.weather == null || r.weather.Length == 0) return null;
            return new WeatherData
            {
                Temperature = $"{r.main.temp:0.0}", // Formate la température.
                Description = r.weather[0].description  // Utilise la description météo du premier élément.
            };
        }
        public async Task<ForecastItem[]> GetForecastAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return null; //retourner la ville par défault au lieu de null !!!!!

            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_apiKey}&units=metric"; // URL de l'API pour récupérer la météo actuelle (formatée avec la ville et la clé API).

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
        class WeatherApiResponse
        {
            public MainObj main { get; set; } // Données principales (température, etc.).
            public WeatherObj[] weather { get; set; } // Tableau des conditions météo.
        }
        class MainObj { public float temp { get; set; } }
        class WeatherObj { public string description { get; set; } }

        class ForecastApiResponse
        {
            public ForecastApiItem[] list { get; set; }
        }
        class ForecastApiItem
        {
            public string dt_txt { get; set; }
            public MainObj main { get; set; }
            public WeatherObj[] weather { get; set; }
        }
    }
}
