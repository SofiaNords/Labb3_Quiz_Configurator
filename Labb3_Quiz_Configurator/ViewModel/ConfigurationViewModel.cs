using Labb3_Quiz_Configurator.ViewModel;

namespace Labb3_Quiz_Configurator.Views
{
    // ViewModel för konfigurationsvyn. Den ärver från ViewModelBase.
    class ConfigurationViewModel : ViewModelBase
    {
        // En privat variabel för att hålla en referens till MainWindowViewModel.
        private readonly MainWindowViewModel? mainWindowViewModel;

        // Konstruktorn som tar emot MainWindowViewModel som en parameter.
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            // Tilldelar den inkommande MainWindowViewModel till den privata variabeln.
            this.mainWindowViewModel = mainWindowViewModel;
        }

        // En egenskap som är en QuestionPackViewModel som också heter ActivePack så om man läser av den så hämtar den mainWindowViewModel Active Pack som en genväng när vi bindar att komma åt den
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }


        // Lägg till en 

    }
}
