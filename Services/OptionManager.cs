using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WeatherApp.UI.Services
{
    public class OptionsManager
    {
        private readonly string _optionsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.json");

        public string DefaultCity { get; private set; } = "Paris";
        public string Language { get; private set; } = "fr";

        public OptionsManager()
        {
            LoadOptions();
        }

        public void LoadOptions()
        {
            if (!File.Exists(_optionsPath))
            {
                SaveOptions(DefaultCity, Language);
                return;
            }

            var json = File.ReadAllText(_optionsPath);
            JObject obj = JObject.Parse(json);
            DefaultCity = (string)obj["defaultCity"] ?? "Paris";
            Language = (string)obj["lang"] ?? "fr";
        }

        public void SaveOptions(string city, string lang)
        {
            DefaultCity = city;
            Language = lang;

            var obj = new JObject
            {
                ["defaultCity"] = DefaultCity,
                ["lang"] = Language
            };
            File.WriteAllText(_optionsPath, obj.ToString());
        }
    }
}
