using ReactiveUI;

namespace WeatherApp.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public SearchViewModel SearchVM { get; }
        public MainWindowViewModel()
        {
            var svc = new Services.WeatherService();
            SearchVM = new SearchViewModel(svc);
        }
    }
}
