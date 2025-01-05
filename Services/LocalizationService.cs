using System;
using System.Globalization;
using System.Resources;

namespace WeatherApp.UI.Services
{
    public class LocalizationService
    {
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCulture;

        public event Action CultureChanged;

        public LocalizationService()
        {
            // Initialisez le ResourceManager avec le nom de base correct
            _resourceManager = new ResourceManager("WeatherApp.UI.Resources.Strings", typeof(LocalizationService).Assembly);
            _currentCulture = CultureInfo.CurrentUICulture;
        }

        public void SetCulture(string cultureCode)
        {
            try
            {
                var newCulture = new CultureInfo(cultureCode);
                if (!newCulture.Equals(_currentCulture))
                {
                    _currentCulture = newCulture;
                    CultureInfo.DefaultThreadCurrentCulture = newCulture;
                    CultureInfo.DefaultThreadCurrentUICulture = newCulture;
                    CultureChanged?.Invoke();
                }
            }
            catch (CultureNotFoundException)
            {
                // Fallback en anglais si la culture n'est pas trouvée
                _currentCulture = new CultureInfo("en");
                CultureInfo.DefaultThreadCurrentCulture = _currentCulture;
                CultureInfo.DefaultThreadCurrentUICulture = _currentCulture;
                CultureChanged?.Invoke();
            }
        }

        public string GetString(string key)
        {
            try
            {
                var value = _resourceManager.GetString(key, _currentCulture);
                return value ?? key; // Retourne la clé si la valeur n'est pas trouvée
            }
            catch
            {
                return key; // Retourne la clé en cas d'erreur
            }
        }
    }
}
