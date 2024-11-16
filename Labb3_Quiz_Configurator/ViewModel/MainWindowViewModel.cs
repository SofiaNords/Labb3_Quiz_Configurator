using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // Huvud-ViewModel för applikationens huvudfönster.
    public class MainWindowViewModel : ViewModelBase
    {
        // Privata fält
        private object _currentView;
        private bool _isFullScreen;

        // Egenskaper
        public object CurrentView // Egenskap för att hålla aktuell vy (används för vyväxling).
        {
            get => _currentView;
            set
            {
                _currentView = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEditEnabled)); // Uppdatera Edit-knappen
                RaisePropertyChanged(nameof(IsPlayEnabled)); // Uppdatera Play-knappen
            }
        }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; } // ObservableCollection som håller en lista av frågepaket.
        public bool IsEditEnabled => CurrentView is not ConfigurationView; // Egenskap för att kontrollera om Edit-knappen ska vara aktiv.
        public bool IsPlayEnabled => CurrentView is not PlayerView; // Egenskap för att kontroller om Play-knappen ska vara aktiv.
       
        // Kommandon för olika åtgärder.
        public DelegateCommand NewPackCommand { get; }
        public DelegateCommand OpenPackCommand { get; }
        public DelegateCommand SelectQuestionPackCommand { get; }
        public DelegateCommand SwitchToPlayerViewCommand { get; }
        public DelegateCommand SwitchToConfigurationViewCommand { get; }
        public DelegateCommand FullScreenCommand { get; }

        // Konstruktorn för MainWindowViewModel.
        public MainWindowViewModel()
        {
            // Initiera vyer och kommandon
            SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView);

            // Initiera Packs som en tom ObservableCollection.
            Packs = new ObservableCollection<QuestionPackViewModel>();

            // Initiera ViewModels
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            // Sätt initial vy
            CurrentView = new ConfigurationView();

            // Kommandon för dialoger
            NewPackCommand = new DelegateCommand(OpenCreateNewPackDialog);
            
            OpenPackCommand = new DelegateCommand(OpenPackOption);

            SelectQuestionPackCommand = new DelegateCommand(SelectQuestionPack);

            FullScreenCommand = new DelegateCommand(_ => ToggleFullScreen());

        }

        public void ToggleFullScreen()
        {
            _isFullScreen = !_isFullScreen;
            var window = Application.Current.MainWindow;

            if (_isFullScreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Normal;
                window.Left = 0;
                window.Top = 0;
                window.Width = SystemParameters.PrimaryScreenWidth;
                window.Height = SystemParameters.PrimaryScreenHeight;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                window.Width = 800; // Sätt tillbaka till din önskade storlek
                window.Height = 450; // Sätt tillbaka till din önskade storlek
            }
        }

        private void SelectQuestionPack(object selectedPack)
        {
            if (selectedPack is QuestionPackViewModel pack)
            {
                ConfigurationViewModel.ActivePack = pack; // Sätt det aktiva paketet i ConfigurationViewModel
            }
            CurrentView = new ConfigurationView(); // Lägg till denna rad
        }

        // Metoder för att växla vyer
        private void SwitchToPlayerView(object obj)
        {
            CurrentView = new PlayerView(); // Skapa en instans av PlayerView
        }

        private void SwitchToConfigurationView(object obj)
        {
            CurrentView = new ConfigurationView(); // Skapa en instans av ConfigurationView
        }

        // Öppna dialog för frågepaketsalternativ
        private void OpenPackOption(object obj)
        {
            var dialog = new PackOptionsDialog(ConfigurationViewModel);
            dialog.ShowDialog();
        }

        // Öppna dialog för att skapa ett nytt frågepaket
        private void OpenCreateNewPackDialog(object obj)
        {
            ConfigurationViewModel.NewPack = new QuestionPackViewModel(new QuestionPack("<Packname>"));
            var dialog = new CreateNewPackDialog(this);
            dialog.ShowDialog();
        }
    }
}
