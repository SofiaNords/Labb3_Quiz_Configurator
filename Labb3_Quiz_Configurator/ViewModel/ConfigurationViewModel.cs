using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel för konfigurationen av frågorna
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        // Privat variabel för att hålla det aktiva frågepaketet.
        private QuestionPackViewModel? _activePack;

        // Egenskap som representerar det aktiva frågepaketet
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged(nameof(ActivePack)); // Signalera att ActivePack har ändrats.
                RaisePropertyChanged(nameof(ActivePackName)); // Uppdatera ActivePackName.
                Debug.WriteLine("ActivePack has been updated to: " + (_activePack?.Name ?? "null"));
            }
        }

        // Egenskap som returnerar namnet på det aktiva frågepaketet eller ett standardnamn
        //public string ActivePackName => ActivePack?.Name ?? "Default Name";
        public string ActivePackName
        {
            get
            {
                var packName = _activePack?.Name ?? "Default Question Pack";
                Debug.WriteLine($"ActivePackName: {packName}");
                return packName;
            }
        }

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

        // Privat variabel för den valda frågan
        private Question _selectedQuestion;
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
       

        // Konstruktorn för ConfigurationViewModel
        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            CreatePackCommand = new DelegateCommand(CreatePack);

            // Initiera ActivePack med ett nytt frågepaket
            if (mainWindowViewModel.Packs.All(p => p.Name != "My Question Pack"))
            {
                var myPack = new Model.QuestionPack("My Question Pack");
                ActivePack = new QuestionPackViewModel(myPack);
                mainWindowViewModel.Packs.Add(ActivePack);
            }

            // Ställ in defaultvärden för PackName, SelectedDifficulty och TimeLimitInSeconds
            PackName = "Default Pack Name";
            SelectedDifficulty = Difficulty.Medium; // Sätt standard svårighetsgrad
            TimeLimitInSeconds = 30; // Sätt standard tidsbegränsning

            // Intitiera DifficultyOptions....
            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));

            // Initiera kommandon för att lägga till och ta bort frågor
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            //LoadQuestionsFromActivePack(); // Ladda frågor från det aktiva frågepaketet
            IsQuestionInputVisible = false; // Dölja inmatningsfält initialt
        }

        // Metod för att skapa frågepaket
        private void CreatePack(object obj)
        {   
            // Kontrollera att mainWindowViewModel inte är null
            if (mainWindowViewModel == null)
            {
                MessageBox.Show("Det gick inte att skapa ett nytt frågepaket. Vänligen försök igen senare", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Avbryt operationen
            }

            // Skapa ett nytt QuestionPack
            var newPack = new QuestionPack(PackName, SelectedDifficulty, TimeLimitInSeconds);

            // Skapa en QuestionPackViewModel från QuestionPack
            var newPackViewModel = new QuestionPackViewModel(newPack);

            // Lägg till den nya QuestionPackViewModel till listan av paket
            mainWindowViewModel.Packs.Add(newPackViewModel);
            var debucheck = "debug";

            // Sätt det nya paketet som aktivt
            ActivePack = newPackViewModel;

            // Stäng dialogen
            if (Application.Current.Windows.OfType<CreateNewPackDialog>().Any())
            {
                Application.Current.Windows.OfType<CreateNewPackDialog>().First().Close();
            }
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
            var newQuestionToAdd = new Question("", "", "", "", "");

            ActivePack.Questions.Add(newQuestionToAdd); // Lägg till den nya frågan i Questions
            
            SelectedQuestion = newQuestionToAdd; // Sätt den nya frågan som vald
        }

        // Egenskap för att kontrollera om borttagningsknappen ska vara aktiverad
        public bool IsRemoveQuestionEnabled => SelectedQuestion != null;


        // Kommandon för att öppna frågepaket, hämtas från huvud-ViewModel
        public DelegateCommand OpenPackCommand => mainWindowViewModel.OpenPackCommand;
    }
}
