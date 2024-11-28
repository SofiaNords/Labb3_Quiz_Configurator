using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private QuestionPackViewModel? _activePack; 
        private QuestionPackViewModel? _newPack; 
        private Question _selectedQuestion; 
        private bool _isQuestionInputVisible; 

        public QuestionPackViewModel? ActivePack 
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged(nameof(ActivePack)); 
            }
        }
        public QuestionPackViewModel? NewPack 
        {
            get => _newPack;
            set
            {
                _newPack = value;
                RaisePropertyChanged(nameof(NewPack));
            }
        }

        public bool IsQuestionInputVisible
        {
            get => _isQuestionInputVisible;
            set
            {
                _isQuestionInputVisible = value;
                RaisePropertyChanged(); 
            }
        }

        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged(); 
                IsQuestionInputVisible = _selectedQuestion != null; 
                RaisePropertyChanged(nameof(IsRemoveQuestionEnabled));
            }
        }

        public bool IsRemoveQuestionEnabled => SelectedQuestion != null;
        public ObservableCollection<Difficulty> DifficultyOptions { get; } 
        public DelegateCommand? CreatePackCommand { get; } 
        public DelegateCommand DeletePackCommand { get; } 
        public DelegateCommand AddQuestionCommand { get; } 
        public DelegateCommand RemoveQuestionCommand { get; } 
        
        
        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            CreatePackCommand = new DelegateCommand(CreatePack);

            DeletePackCommand = new DelegateCommand(DeletePack);

            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));

            AddQuestionCommand = new DelegateCommand(AddQuestion);
            
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            IsQuestionInputVisible = false; 
        }

        public void AddDefaultPackIfNoPackExists()
        {
            if (!mainWindowViewModel.Packs.Any())
            {
                var myPack = new Model.QuestionPack("Default Question Pack");
                ActivePack = new QuestionPackViewModel(myPack);
                mainWindowViewModel.Packs.Add(ActivePack);
            }
        }

        private void CreatePack(object obj)
        {   
            mainWindowViewModel.Packs.Add(NewPack);

            ActivePack = NewPack;

            if (Application.Current.Windows.OfType<CreateNewPackDialog>().Any())
            {
                Application.Current.Windows.OfType<CreateNewPackDialog>().First().Close();
            }
        }

        private void DeletePack(object obj)
        {
            if (ActivePack != null)
            {
                mainWindowViewModel.Packs.Remove(ActivePack);

                ActivePack = null;
            }
        }

        private void AddQuestion(object obj)
        {
            var newQuestionToAdd = new Question("New Question", "", "", "", "");

            ActivePack.Questions.Add(newQuestionToAdd); 

            SelectedQuestion = newQuestionToAdd; 
        }

        private void RemoveQuestion(object obj)
        {
            if (SelectedQuestion != null)
            {
                ActivePack.Questions.Remove(SelectedQuestion); 
                SelectedQuestion = null; 
                IsQuestionInputVisible = false; 
            }
        }

        public DelegateCommand OpenPackCommand => mainWindowViewModel.OpenPackCommand;
    }
}
