using ReactiveUI;
using System;
using System.Reactive;
using WeatherApp.UI.Services;

namespace WeatherApp.UI.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IDisposable
    {
        private string _defaultCity;
        public string DefaultCity
        {
            get => _defaultCity;
            set => this.RaiseAndSetIfChanged(ref _defaultCity, value);
        }

        private string _language;
        public string Language
        {
            get => _language;
            set => this.RaiseAndSetIfChanged(ref _language, value);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        private readonly OptionsManager _optionsManager;
        private readonly LocalizationService _localizationService;

        public SettingsViewModel(OptionsManager optionsManager, LocalizationService localizationService)
        {
            _optionsManager = optionsManager;
            _localizationService = localizationService;

            // Charger les options actuelles
            DefaultCity = _optionsManager.DefaultCity;
            Language = _optionsManager.Language;

            SaveCommand = ReactiveCommand.Create(SaveOptions);

            // S'abonner à l'événement CultureChanged
            _localizationService.CultureChanged += OnCultureChanged;
        }

        private void SaveOptions()
        {
            try
            {
                _optionsManager.SaveOptions(DefaultCity, Language);
                _localizationService.SetCulture(Language);
                Console.WriteLine($"Langue changée en : {Language}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'enregistrement des options : {ex.Message}");
            }
        }

        // Propriétés pour les chaînes traduites
        public string SettingsTitle => _localizationService.GetString("SettingsTitle");
        public string DefaultCityLabel => _localizationService.GetString("DefaultCityLabel");
        public string LanguageLabel => _localizationService.GetString("LanguageLabel");
        public string SaveButton => _localizationService.GetString("SaveButton");

        private void OnCultureChanged()
        {
            // Notifier l'interface utilisateur que les propriétés traduites ont changé
            this.RaisePropertyChanged(nameof(SettingsTitle));
            this.RaisePropertyChanged(nameof(DefaultCityLabel));
            this.RaisePropertyChanged(nameof(LanguageLabel));
            this.RaisePropertyChanged(nameof(SaveButton));
        }

        public void Dispose()
        {
            _localizationService.CultureChanged -= OnCultureChanged;
        }
    }
}
