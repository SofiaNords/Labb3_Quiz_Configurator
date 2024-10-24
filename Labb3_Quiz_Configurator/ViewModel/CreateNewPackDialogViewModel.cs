using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.ViewModel;
using System.Collections.ObjectModel;

namespace Labb3_Quiz_Configurator.Dialogs
{
    internal class CreateNewPackDialogViewModel : ViewModelBase
    {
    
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
     
        public CreateNewPackDialogViewModel()
        {
            CreatePackCommand = new DelegateCommand(CreatePack);
            SelectedDifficulty = Difficulty.Medium;
            TimeLimitInSeconds = 30; 
            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));
        }

        private void CreatePack(object obj)
        {
            var newPack = new QuestionPack(PackName, SelectedDifficulty, TimeLimitInSeconds);
        }

    }
    
}