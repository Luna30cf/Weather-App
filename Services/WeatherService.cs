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
        public WeatherService()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(path)) throw new FileNotFoundException("config.json introuvable");
            dynamic cfg = JsonConvert.DeserializeObject(File.ReadAllText(path));
            _apiKey = cfg.ApiKey;
        }
        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
            using var cli = new HttpClient();
            var res = await cli.GetStringAsync(url);
            var r = JsonConvert.DeserializeObject<WeatherApiResponse>(res);
            if (r?.main == null || r.weather == null || r.weather.Length == 0) return null;
            return new WeatherData
            {
                Temperature = $"{r.main.temp:0.0}",
                Description = r.weather[0].description
            };
        }
        class WeatherApiResponse
        {
            public MainObj main { get; set; }
            public WeatherObj[] weather { get; set; }
        }
        class MainObj { public float temp { get; set; } }
        class WeatherObj { public string description { get; set; } }
    }
}
