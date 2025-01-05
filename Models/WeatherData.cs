namespace WeatherApp.UI.Models
{
    public class WeatherData
    {
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Temperature { get; set; }
        public string Description { get; set; }
        public int Humidity { get; set; }
        public string IconUrl { get; set; }
    }
}
