using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel för konfigurationen av frågorna
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        // En ObservableCollection för att hålla frågorna
        public ObservableCollection<QuestionViewModel> Questions { get; }

        // Privat variabel för att hålla det aktiva frågepaketet.
        private QuestionPackViewModel? _activePack;

        // Egenskap som representerar det aktiva frågepaketet
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged(); // Signalera att ActivePack har ändrats.
                RaisePropertyChanged(nameof(ActivePackName)); // Uppdatera ActivePackName.
                LoadQuestionsFromActivePack(); // Ladda frågor när ActivePack ändras
            }
        }

        // Egenskap som returnerar namnet på det aktiva frågepaketet eller ett standardnamn
        public string ActivePackName => ActivePack?.Name ?? "Default Question Pack";


        // Variabler för att lagra information om frågepaketet
        private string packName;
        private Difficulty selectedDifficulty;
        private int timeLimitInSeconds;


        // Egenskap för att hantera namn på frågepaketet
        public string PackName
        {
            get => packName;
            set
            {
                packName = value;
                RaisePropertyChanged();
            }
        }


        // Egenskap för att välja svårighetsgrad
        public Difficulty SelectedDifficulty
        {
            get => selectedDifficulty;
            set
            {
                selectedDifficulty = value;
                RaisePropertyChanged();
            }
        }

        // Egenskap för att ställa in tidsgräns i sekunder
        public int TimeLimitInSeconds
        {
            get => timeLimitInSeconds;
            set
            {
                timeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }

        // Samling av svårighetsalternativ
        public ObservableCollection<Difficulty> DifficultyOptions { get; }

        // Kommandon för att skapa och hantera frågor
        public DelegateCommand? CreatePackCommand { get; }


        // Kommandon för att lägga till och ta bort frågor
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }

        // Privat variabel för att hantera en ny fråga
        private QuestionViewModel newQuestion;
        public QuestionViewModel NewQuestion
        {
            get => newQuestion;
            set
            {
                // Avregistrera eventhanterare om det finns en tidigare ny fråga
                if (newQuestion != null)
                    newQuestion.PropertyChanged -= NewQuestion_PropertyChanged;

                newQuestion = value;

                // Registrera eventhanterare för den nya frågan
                if (newQuestion != null)
                    newQuestion.PropertyChanged += NewQuestion_PropertyChanged;

                RaisePropertyChanged(); // Signalera att NewQuestion har ändrats
            }
        }

        // Privat variabel för den valda frågan
        private QuestionViewModel? selectedQuestion;
        public QuestionViewModel? SelectedQuestion
        {
            get => selectedQuestion;
            set
            {
                selectedQuestion = value;
                RaisePropertyChanged(); // Signalera att den valda frågan har ändrats
                UpdateNewQuestion(); // Uppdatera NewQuestion med värden från den valda frågan
                IsQuestionInputVisible = selectedQuestion != null; // Visa inmatningsfältet om en fråga är vald
                RaisePropertyChanged(nameof(IsRemoveQuestionEnabled)); // Uppdatera möjligheten att ta bort en fråga
            }
        }
       

        // Konstruktorn för ConfigurationViewModel
        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            Questions = new ObservableCollection<QuestionViewModel>(); // Initiera Questions

            CreatePackCommand = new DelegateCommand(CreatePack);

            // Initiera ActivePack med ett nytt frågepaket
            if (mainWindowViewModel.Packs.All(p => p.Name != "My Question Pack"))
            {
                var myPack = new Model.QuestionPack("My Question Pack");
                ActivePack = new QuestionPackViewModel(myPack);
            }

            // Ställ in defaultvärden för PackName, SelectedDifficulty och TimeLimitInSeconds
            PackName = "Default Pack Name";
            SelectedDifficulty = Difficulty.Medium; // Sätt standard svårighetsgrad
            TimeLimitInSeconds = 30; // Sätt standard tidsbegränsning

            // Intitiera DifficultyOptions....
            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));

            NewQuestion = new QuestionViewModel(); // Initiera NewQuestion
            NewQuestion.PropertyChanged += NewQuestion_PropertyChanged; // Registrera eventhanterare

            // Initiera kommandon för att lägga till och ta bort frågor
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            LoadQuestionsFromActivePack(); // Ladda frågor från det aktiva frågepaketet
            IsQuestionInputVisible = false; // Dölja inmatningsfält initialt
        }

        // Metod för att skapa frågepaket
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


            // Sätt det nya paketet som aktivt
            ActivePack = newPackViewModel;

            // Stäng dialogen
            if (Application.Current.Windows.OfType<CreateNewPackDialog>().Any())
            {
                Application.Current.Windows.OfType<CreateNewPackDialog>().First().Close();
            }

        }

        // Metod för att ladda frågor från det aktiva frågepaketet
        public void LoadQuestionsFromActivePack()
        {
            if (ActivePack != null)
            {
                // Kontrollera om ActivePack redan finns i mainWindowViewModel.Packs
                if (!mainWindowViewModel.Packs.Contains(ActivePack))
                {
                    mainWindowViewModel.Packs.Add(ActivePack); // Lägg till paketet i listan över frågepaket
                }

                Questions.Clear(); // Rensa befintliga frågor
                foreach (var question in ActivePack.Questions) // Iterera genom frågorna i ActivePack
                {
                    // Lägg till varje fråga som en QuestionViewModel i Questions
                    Questions.Add(new QuestionViewModel
                    {
                        Query = question.Query,
                        CorrectAnswer = question.CorrectAnswer,
                        FirstIncorrectAnswer = question.IncorrectAnswers[0],
                        SecondIncorrectAnswer = question.IncorrectAnswers[1],
                        ThirdIncorrectAnswer = question.IncorrectAnswers[2]
                    });
                }
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

        // Metod för att ta bort en vald fråga
        private void RemoveQuestion(object obj)
        {
            if (SelectedQuestion != null)
            {
                Questions.Remove(SelectedQuestion); // Ta bort den valda frågan från Questions
                SelectedQuestion = null; // Återställ den valda frågan
                IsQuestionInputVisible = false; // Dölja inmatningsfältet
            }
        }

        // Privat variabel för att hantera visibiliteten av inmatningsfältet
        private bool isQuestionInputVisible;
        public bool IsQuestionInputVisible
        {
            get => isQuestionInputVisible;
            set
            {
                isQuestionInputVisible = value;
                RaisePropertyChanged(); // Signalera att IsQuestionInputVisible har ändrats
            }
        }

        // Metod för att lägga till en ny fråga
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

            Questions.Add(newQuestionToAdd); // Lägg till den nya frågan i Questions


            // Lägg även till den nya frågan i ActivePack
            if (ActivePack != null)
            {
                var questionToAdd = new Question(
                    newQuestionToAdd.Query,
                    newQuestionToAdd.CorrectAnswer,
                    newQuestionToAdd.FirstIncorrectAnswer,
                    newQuestionToAdd.SecondIncorrectAnswer,
                    newQuestionToAdd.ThirdIncorrectAnswer
                );

                ActivePack.Questions.Add(questionToAdd); // Lägg till i ActivePack
            }

            SelectedQuestion = newQuestionToAdd; // Sätt den nya frågan som vald
            IsQuestionInputVisible = true; // Visa inmatningsfältet för att redigera frågan
        }

        // Egenskap för att kontrollera om borttagningsknappen ska vara aktiverad
        public bool IsRemoveQuestionEnabled => SelectedQuestion != null;

        // Eventhanterare för att reagera på ändringar i NewQuestion
        private void NewQuestion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SelectedQuestion != null) // Kontrollera om en fråga är vald
            {
                switch (e.PropertyName)
                {
                    case nameof(NewQuestion.Query):
                        SelectedQuestion.Query = NewQuestion.Query; // Uppdatera frågan
                        break;
                    case nameof(NewQuestion.CorrectAnswer):
                        SelectedQuestion.CorrectAnswer = NewQuestion.CorrectAnswer; // Uppdatera korrekt svar
                        break;
                    case nameof(NewQuestion.FirstIncorrectAnswer):
                        SelectedQuestion.FirstIncorrectAnswer = NewQuestion.FirstIncorrectAnswer; // Uppdatera första fel svar
                        break;
                    case nameof(NewQuestion.SecondIncorrectAnswer):
                        SelectedQuestion.SecondIncorrectAnswer = NewQuestion.SecondIncorrectAnswer; // Uppdatera andra fel svar
                        break;
                    case nameof(NewQuestion.ThirdIncorrectAnswer):
                        SelectedQuestion.ThirdIncorrectAnswer = NewQuestion.ThirdIncorrectAnswer; // Uppdatera tredje fel svar
                        break;
                }
            }
        }

        // Kommandon för att öppna frågepaket, hämtas från huvud-ViewModel
        public DelegateCommand OpenPackCommand => mainWindowViewModel.OpenPackCommand;
    }
}
