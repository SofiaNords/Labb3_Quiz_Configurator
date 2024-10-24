using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // Huvud-ViewModel för applikationens huvudfönster där bland annat MenuView hanteras
    class MainWindowViewModel : ViewModelBase
    {
        // ObservableCollection som håller en lista av frågepaket.
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        // ViewModel för konfigurationsvyn.
        public ConfigurationViewModel ConfigurationViewModel { get; }

        // ViewModel för spelarvyn.
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
                ConfigurationViewModel.RaisePropertyChanged("ActivePack"); // Ett exempel på att man måste anropa RaisePropertyChanged från ett annat ställe i koden. 
            }
        }

        public DelegateCommand NewPackCommand { get; }

        public DelegateCommand OpenPackCommand { get; }

        // Konstruktorn för MainWindowViewModel.
        public MainWindowViewModel()
        {
            // Initierar ConfigurationViewModel med en referens till MainWindowViewModel.
            ConfigurationViewModel = new ConfigurationViewModel(this);

            // Intitierar PlayerViewModel med en referens till MainWindowViewModel.
            PlayerViewModel = new PlayerViewModel(this);

            // Sätter ett standardaktigt frågepaket som det aktiva paketet.
            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack")); // Denna ska förmodligen inte sättas i konstruktorn sedan.

            // Initiera Packs som en tom ObservableCollection.
            Packs = new ObservableCollection<QuestionPackViewModel>(); // Detta var ChatGPTS förslag

            // Kommando som anropas när användaren väljer New Question Pack
            NewPackCommand = new DelegateCommand(OpenCreateNewPackDialog);

            // Kommando som anropas när användaren väljer Pack Options
            OpenPackCommand = new DelegateCommand(OpenPackOption);
        }

        private void OpenPackOption(object obj)
        {
            var dialog = new PackOptionsDialog();
            dialog.ShowDialog();
        }

        private void OpenCreateNewPackDialog(object obj)
        {
            var dialog = new CreateNewPackDialog();
            dialog.ShowDialog();
        }
    }
}
