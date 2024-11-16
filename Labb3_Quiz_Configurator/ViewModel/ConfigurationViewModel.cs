using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel för konfigurationen av frågorna
    public class ConfigurationViewModel : ViewModelBase
    {
        // Privata fält
        private readonly MainWindowViewModel mainWindowViewModel;
        private QuestionPackViewModel? _activePack; 
        private QuestionPackViewModel? _newPack; 
        private Question _selectedQuestion; 
        private bool _isQuestionInputVisible; 

        // Offentliga egenskaper
        public QuestionPackViewModel? ActivePack // Egenskap som representerar det aktiva frågepaketet
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged(nameof(ActivePack)); // Signalera att ActivePack har ändrats.
            }
        }
        public QuestionPackViewModel? NewPack // Egenskap som representerar det aktiva frågepaketet
        {
            get => _newPack;
            set
            {
                _newPack = value;
                RaisePropertyChanged(nameof(NewPack)); // Signalera att ActivePack har ändrats.
            }
        }
        public ObservableCollection<Difficulty> DifficultyOptions { get; } // Samling av svårighetsalternativ
        public DelegateCommand? CreatePackCommand { get; } // Kommandon för att skapa och hantera frågor
        public DelegateCommand DeletePackCommand { get; } // Kommando för att kunna radera frågepaket
        public DelegateCommand AddQuestionCommand { get; } // Kommandon för att lägga till rågor
        public DelegateCommand RemoveQuestionCommand { get; } // Kommandon för att ta bort frågor
        public bool IsQuestionInputVisible
        {
            get => _isQuestionInputVisible;
            set
            {
                _isQuestionInputVisible = value;
                RaisePropertyChanged(); // Signalera att IsQuestionInputVisible har ändrats
            }
        }
        public bool IsRemoveQuestionEnabled => SelectedQuestion != null; // Egenskap för att kontrollera om borttagningsknappen ska vara aktiverad

        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged(); // Signalera att den valda frågan har ändrats
                IsQuestionInputVisible = _selectedQuestion != null; // Visa inmatningsfältet om en fråga är vald
                RaisePropertyChanged(nameof(IsRemoveQuestionEnabled)); // Uppdatera möjligheten att ta bort en fråga
            }
        }

        // Konstruktorn för ConfigurationViewModel som tar en parameter av typen MainWindowViewModel
        // som används för att interagera med huvudfönstrets ViewModel
        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            // this refererar till den aktuealla iunstansen av objektet, alltså det objekt som just nu är instansierat
            // och som metoden eller konstruktorn tillhör
            this.mainWindowViewModel = mainWindowViewModel;

            CreatePackCommand = new DelegateCommand(CreatePack);

            DeletePackCommand = new DelegateCommand(DeletePack);

            // Skapa ett nytt frågepaket med namnet "Default Question Pack"
            var myPack = new Model.QuestionPack("Default Question Pack");

            // Sätt det nyskapade frågepaketet som ActivePack
            ActivePack = new QuestionPackViewModel(myPack);

            // Lägg till det nya frågepaketet i listan över frågepaket
            mainWindowViewModel.Packs.Add(ActivePack);

            // Intitiera DifficultyOptions....
            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));

            // Initiera kommandon för att lägga till och ta bort frågor
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            IsQuestionInputVisible = false; // Dölja inmatningsfält initialt
        }

        // Metod för att skapa frågepaket
        private void CreatePack(object obj)
        {   
            // Lägg till den nya QuestionPackViewModel till listan av paket
            mainWindowViewModel.Packs.Add(NewPack);

            // Sätt det nya paketet som aktivt
            ActivePack = NewPack;

            // Stäng dialogen
            if (Application.Current.Windows.OfType<CreateNewPackDialog>().Any())
            {
                Application.Current.Windows.OfType<CreateNewPackDialog>().First().Close();
            }
        }

        // Metod för att radera frågepaket
        private void DeletePack(object obj)
        {
            if (ActivePack != null)
            {
                mainWindowViewModel.Packs.Remove(ActivePack);

                ActivePack = null;
            }
        }


        // Metod för att lägga till en ny fråga
        private void AddQuestion(object obj)
        {
            var newQuestionToAdd = new Question("New Question", "", "", "", "");

            ActivePack.Questions.Add(newQuestionToAdd); // Lägg till den nya frågan i Questions

            SelectedQuestion = newQuestionToAdd; // Sätt den nya frågan som vald
        }


        // Metod för att ta bort en vald fråga
        private void RemoveQuestion(object obj)
        {
            if (SelectedQuestion != null)
            {
                ActivePack.Questions.Remove(SelectedQuestion); // Ta bort den valda frågan från Questions
                SelectedQuestion = null; // Återställ den valda frågan
                IsQuestionInputVisible = false; // Dölja inmatningsfältet
            }
        }


        // TODO: Kommandon för att öppna frågepaket, hämtas från huvud-ViewModel. Se över om vi kan ta bort denna då den verkar överflödig. Blir dock Binding Error om jag tar bort den nu.
        public DelegateCommand OpenPackCommand => mainWindowViewModel.OpenPackCommand;
    }
}
