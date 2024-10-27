using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    internal class CreateNewPackDialogViewModel : ViewModelBase
    {
        // Egenskaper för packinformation
        private string packName;
        private Difficulty selectedDifficulty;
        private int timeLimitInSeconds;


        public string PackName
        {
            get => packName;
            set { 
                packName = value;
                RaisePropertyChanged();
            }
        }


        public Difficulty SelectedDifficulty
        {
            get => selectedDifficulty;
            set 
            { 
                selectedDifficulty = value;
                RaisePropertyChanged();
            }
        }


        public int TimeLimitInSeconds
        {
            get => timeLimitInSeconds;
            set 
            { 
                timeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Difficulty> DifficultyOptions { get; }

        public DelegateCommand? CreatePackCommand { get; }

        private MainWindowViewModel mainWindowViewModel;

        public CreateNewPackDialogViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            CreatePackCommand = new DelegateCommand(CreatePack);
            SelectedDifficulty = Difficulty.Medium;
            TimeLimitInSeconds = 30; 
            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));
        }

        private void CreatePack(object obj)
        {
            if (mainWindowViewModel == null)
            {
                MessageBox.Show("Det gick inte att skapa ett nytt frågepaket. Vänligen försök igen senare", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Avbryt operationen
            }
                
            // Skapa ett nytt QuestionPack
            var newPack = new QuestionPack(PackName, SelectedDifficulty, TimeLimitInSeconds);

            // Skapa en QuestionPackViewModel från QuestionPack
            var newPackViewModel = new QuestionPackViewModel(newPack);

            // Lägg till den nya QuestionPackViewModel i mainWindowViewModel.Packs
            mainWindowViewModel.Packs.Add(newPackViewModel);

            // Sätt det nya paketet som aktivt
            mainWindowViewModel.ActivePack = newPackViewModel;

            // Stäng dialogen
            if (Application.Current.Windows.OfType<CreateNewPackDialog>().Any())
            {
                Application.Current.Windows.OfType<CreateNewPackDialog>().First().Close();
            }

        }
    }  
}