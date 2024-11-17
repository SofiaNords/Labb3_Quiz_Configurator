using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.ViewModel;
using System.Windows.Threading;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel för spelvyn. Hanterar logik relaterad till spelarsessioen.
    public class PlayerViewModel : ViewModelBase
    {
        // En privat variabel för att hålla en referens till MainWindowViewModel.
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private string _testData;
        private string _question;
        private string _answerOne;
        private string _answerTwo;
        private string _answerThree;
        private string _answerFour;

        private QuestionPackViewModel? _activePack; // Det valda frågepaketet

        // En egenskap som returnerar testdata som en sträng
        public string TestData { 
            get => _testData; 
            private set {
                _testData = value;
                RaisePropertyChanged();
            }  
        }

        // En egenskap som returnerar en fråga som en sträng
        public string Question
        {
            get => _question;

            set { 
                _question = value;
                RaisePropertyChanged();
            }
        }

        public string AnswerOne
        {
            get => _answerOne; 
            set {
                _answerOne = value;
                RaisePropertyChanged();
            }
        }

        public string AnswerTwo
        {
            get => _answerTwo; 
            set { 
                _answerTwo = value;
                RaisePropertyChanged();
            }
        }

        public string AnswerThree
        {
            get => _answerThree;
            set
            {
                _answerThree = value;
                RaisePropertyChanged();
            }
        }

        public string AnswerFour
        {
            get => _answerFour;
            set
            {
                _answerFour = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateButtonCommand { get; } // Add Question command, Remo

        public DelegateCommand AddQuestionCommand { get; }


        // Konstruktorn som tar emot MainWindowViewModel som en parameter.
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            // Tilldelar den inkommande MainWindowViewModel till den privata variabeln.
            this.mainWindowViewModel = mainWindowViewModel;

            TestData = "Start value: ";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            //timer.Start();

            UpdateButtonCommand = new DelegateCommand(UpdateButton, CanUpdateButton);
            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
        }

        // Sätt det aktiva frågepaketet och visa den första frågan
        public void SetActivePack(QuestionPackViewModel? pack)
        {
            if (pack != null && pack.Questions.Any()) // Kontrollera om det finns frågor i paketet
            {
                _activePack = pack;
                var firstQuestion = _activePack.Questions.First();

                var answers = new List<string> { firstQuestion.CorrectAnswer };
                answers.AddRange(firstQuestion.IncorrectAnswers.Take(3));

                var random = new Random();
                var shuffledAnswers = answers.OrderBy(a => random.Next()).ToList();


                Question = firstQuestion.Query;
                AnswerOne = shuffledAnswers[0];
                AnswerTwo = shuffledAnswers[1];
                AnswerThree = shuffledAnswers[2];
                AnswerFour = shuffledAnswers[3];
            }
        }

        private bool CanAddQuestion(object? arg)
        {
            return true; // Sätt ett villkor om den ska gå att köra eller inte
        }

        private void AddQuestion(object obj)
        {
            Question += "Här är testfrågan";
        }

        private bool CanUpdateButton(object? arg)
        {
            return true; // Sätt ett villkor om den ska gå att köra eller inte
        }

        private void UpdateButton(object obj)
        {
            TestData += "x";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TestData += "x";
        }
    }
}
