using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel för konfigurationsvyn, ärver från ViewModelBase.
    public class ConfigurationViewModel : ViewModelBase
    {
        // Referens till MainWindowViewModel.
        private readonly MainWindowViewModel? mainWindowViewModel;

        // Egenskap för att kontrollera om ta bort-knappen ska vara aktiv.
        public bool IsRemoveQuestionEnabled
        {
            get => SelectedQuestion != null; // Aktiv om en fråga är vald.
        }

        private MainWindowViewModel? _mainWindowViewModel;

        // Samling av frågor, observerbar för att uppdatera UI automatiskt.
        public ObservableCollection<QuestionViewModel> Questions { get; }

        // Egenskap för att få namnet på det aktiva frågepaketet
        public string CurrentPackName => mainWindowViewModel.ActivePack?.Name ?? "Default Question Pack";

        // Kommando för att lägga till en fråga.
        public DelegateCommand AddQuestionCommand { get; }

        // Kommando för att ta bort en fråga.
        public DelegateCommand RemoveQuestionCommand { get; }

        // Konstruktorn som tar emot MainWindowViewModel.
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            // Initiera samlingen av frågor och andra variabler.
            Questions = new ObservableCollection<QuestionViewModel>();
            NewQuestion = new QuestionViewModel();
            NewQuestion.PropertyChanged += NewQuestion_PropertyChanged; // Registrera händelsehanterare för NewQuestion.

            // Initiera kommandon.
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            // Dölja inmatningsfält initialt.
            IsQuestionInputVisible = false;
        }

        // Metod för att ta bort den valda frågan.
        private void RemoveQuestion(object obj)
        {
            if (SelectedQuestion != null)
            {
                // Ta bort den valda frågan från Questions-samlingen.
                Questions.Remove(SelectedQuestion);

                // Sätt SelectedQuestion till null för att uppdatera UI.
                SelectedQuestion = null;

                // Göm inmatningsfälten.
                IsQuestionInputVisible = false;
            }
        }

        // Hanterar förändringar i NewQuestion och uppdaterar SelectedQuestion.
        private void NewQuestion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SelectedQuestion != null)
            {
                // Uppdatera SelectedQuestion med värden från NewQuestion baserat på förändrat egenskap.
                switch (e.PropertyName)
                {
                    case nameof(NewQuestion.Query):
                        SelectedQuestion.Query = NewQuestion.Query;
                        break;
                    case nameof(NewQuestion.CorrectAnswer):
                        SelectedQuestion.CorrectAnswer = NewQuestion.CorrectAnswer;
                        break;
                    case nameof(NewQuestion.FirstIncorrectAnswer):
                        SelectedQuestion.FirstIncorrectAnswer = NewQuestion.FirstIncorrectAnswer;
                        break;
                    case nameof(NewQuestion.SecondIncorrectAnswer):
                        SelectedQuestion.SecondIncorrectAnswer = NewQuestion.SecondIncorrectAnswer;
                        break;
                    case nameof(NewQuestion.ThirdIncorrectAnswer):
                        SelectedQuestion.ThirdIncorrectAnswer = NewQuestion.ThirdIncorrectAnswer;
                        break;
                }
            }
        }

        // Kontrollerar synligheten av inmatningsfälten.
        private bool isQuestionInputVisible;
        public bool IsQuestionInputVisible
        {
            get => isQuestionInputVisible;
            set
            {
                isQuestionInputVisible = value;
                RaisePropertyChanged(); // Utröna förändring.
            }
        }

        // Den valda frågan som kan redigeras.
        private QuestionViewModel? selectedQuestion;
        public QuestionViewModel? SelectedQuestion
        {
            get => selectedQuestion;
            set
            {
                selectedQuestion = value;
                RaisePropertyChanged(); // Utröna förändring.
                UpdateNewQuestion(); // Uppdatera NewQuestion med valda frågans värden.
                IsQuestionInputVisible = selectedQuestion != null; // Bestäm synlighet av inmatningsfält.
                RaisePropertyChanged(nameof(IsRemoveQuestionEnabled)); // Utröna förändring för ta bort-knappen.
            }
        }

        // Uppdaterar NewQuestion med värden från den valda frågan.
        private void UpdateNewQuestion()
        {
            if (SelectedQuestion != null)
            {
                NewQuestion.Query = SelectedQuestion.Query;
                NewQuestion.CorrectAnswer = SelectedQuestion.CorrectAnswer;
                NewQuestion.FirstIncorrectAnswer = SelectedQuestion.FirstIncorrectAnswer;
                NewQuestion.SecondIncorrectAnswer = SelectedQuestion.SecondIncorrectAnswer;
                NewQuestion.ThirdIncorrectAnswer = SelectedQuestion.ThirdIncorrectAnswer;
            }
        }

        // Instans av NewQuestion som ska bindas till UI.
        private QuestionViewModel newQuestion;
        public QuestionViewModel NewQuestion
        {
            get => newQuestion;
            set
            {
                newQuestion = value;
                RaisePropertyChanged(); // Utröna förändring.
            }
        }

        // Lägger till en ny fråga med förinställt innehåll.
        private void AddQuestion(object obj)
        {
            var newQuestionToAdd = new QuestionViewModel
            {
                Query = "New Question",
                CorrectAnswer = "",
                FirstIncorrectAnswer = "",
                SecondIncorrectAnswer = "",
                ThirdIncorrectAnswer = ""
            };

            Questions.Add(newQuestionToAdd); // Lägg till i samlingen.
            SelectedQuestion = newQuestionToAdd; // Välj den nya frågan.
            IsQuestionInputVisible = true; // Gör inmatningsfält synliga.
        }

        // Getter för att hämta den aktiva frågepaket.
        public QuestionPackViewModel? ActivePack => mainWindowViewModel.ActivePack;

        // Exponera OpenPackCommand från MainWindowViewModel.
        public DelegateCommand OpenPackCommand => mainWindowViewModel.OpenPackCommand;
    }
}
