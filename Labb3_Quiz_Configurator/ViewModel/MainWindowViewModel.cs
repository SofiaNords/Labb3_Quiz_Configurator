using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // Huvud-ViewModel för applikationens huvudfönster.
    public class MainWindowViewModel : ViewModelBase
    {
        // Egenskap för att hålla aktuell vy (används för vyväxling).
        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEditEnabled)); // Uppdatera Edit-knappen
                RaisePropertyChanged(nameof(IsPlayEnabled)); // Uppdatera Play-knappen
            }
        }
        // Egenskap för att kontrollera om Edit-knappen ska vara aktiv.
        public bool IsEditEnabled => CurrentView is not ConfigurationView;

        // Egenskap för att kontroller om Play-knappen ska vara aktiv.
        public bool IsPlayEnabled => CurrentView is not PlayerView;

        // ObservableCollection som håller en lista av frågepaket.
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        // ViewModels för de olika vyerna.
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }

        // Privat variabel för att hålla det aktiva frågepaketet.
        private QuestionPackViewModel? _activePack;

        // Egenskap som representerar det aktiva frågepaketet.
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged(); // Signalera att ActivePack har ändrats.
                ConfigurationViewModel.RaisePropertyChanged(nameof(ConfigurationViewModel.CurrentPackName)); // Uppdatera ConfigurationViewModel.
            }
        }

        // Kommandon för olika åtgärder.
        public DelegateCommand NewPackCommand { get; }
        public DelegateCommand OpenPackCommand { get; }
        public DelegateCommand SwitchToPlayerViewCommand { get; }
        public DelegateCommand SwitchToConfigurationViewCommand { get; }

        // Konstruktorn för MainWindowViewModel.
        public MainWindowViewModel()
        {
            // Initiera vyer och kommandon
            SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView);

            // Sätt initial vy
            CurrentView = new ConfigurationView();

            // Initiera ViewModels
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            // Sätt ett standardaktigt frågepaket som det aktiva paketet.
            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));

            // Initiera Packs som en tom ObservableCollection.
            Packs = new ObservableCollection<QuestionPackViewModel>();

            // Kommandon för dialoger
            NewPackCommand = new DelegateCommand(OpenCreateNewPackDialog);
            OpenPackCommand = new DelegateCommand(OpenPackOption);
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
            var dialog = new PackOptionsDialog();
            dialog.ShowDialog();
        }

        // Öppna dialog för att skapa ett nytt frågepaket
        private void OpenCreateNewPackDialog(object obj)
        {
            var dialog = new CreateNewPackDialog(this);
            dialog.ShowDialog();
        }
    }
}
