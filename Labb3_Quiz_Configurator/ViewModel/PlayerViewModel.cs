using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.ViewModel;
using System.Windows.Threading;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel för spelvyn. Hanterar logik relaterad till spelarsessioen.
    public class PlayerViewModel : ViewModelBase
    {
        // Privata fält
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private DispatcherTimer _timer;
        private int _remainingTime;
        private int _currentQuestionIndex = 0; // Håll reda på vilken fråga vi är på
        private int _correctAnswers = 0; // Håller reda på antalet rätta svar
        private int _totalQuestions = 0; // Totalt antal frågor i quizet
        private string _testData;
        private string _question; 
        private string _answerOne;
        private string _answerTwo;
        private string _answerThree;
        private string _answerFour;
        private string _answerOneColor = "Transparent";
        private string _answerTwoColor = "Transparent";
        private string _answerThreeColor = "Transparent";
        private string _answerFourColor = "Transparent";

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

        // Sätt färg för svarsknappar
        public string AnswerOneColor
        {
            get => _answerOneColor;
            set
            {
                _answerOneColor = value;
                RaisePropertyChanged(nameof(AnswerOneColor));
            }
        }

        public string AnswerTwoColor
        {
            get => _answerTwoColor;
            set
            {
                _answerTwoColor = value;
                RaisePropertyChanged(nameof(AnswerTwoColor));
            }
        }

        public string AnswerThreeColor
        {
            get => _answerThreeColor;
            set
            {
                _answerThreeColor = value;
                RaisePropertyChanged(nameof(AnswerThreeColor));
            }
        }

        public string AnswerFourColor
        {
            get => _answerFourColor;
            set
            {
                _answerFourColor = value;
                RaisePropertyChanged(nameof(AnswerFourColor));
            }
        }

        public string TimeRemaining { get; private set; } = "00:00";
        public string ResultMessage { get; private set; } = string.Empty;
        public bool QuizIsOver { get; private set; } = false;

        public bool QuizIsRunning { get; private set; } = false;

        public DelegateCommand RestartQuizCommand { get; }

        public DelegateCommand UpdateButtonCommand { get; } // Add Question command, Remo

        public DelegateCommand AddQuestionCommand { get; }

        public DelegateCommand AnswerCommand { get; }


        // Konstruktorn som tar emot MainWindowViewModel som en parameter.
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            // Tilldelar den inkommande MainWindowViewModel till den privata variabeln.
            this._mainWindowViewModel = mainWindowViewModel;

            QuizIsRunning = true;

            TestData = "Start value: ";

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            RestartQuizCommand = new DelegateCommand(RestartQuiz, CanRestartQuiz);
            UpdateButtonCommand = new DelegateCommand(UpdateButton, CanUpdateButton);
            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            AnswerCommand = new DelegateCommand(OnAnswerClicked);
        }

        // Sätt det aktiva frågepaketet och visa den första frågan
        public void SetActivePack(QuestionPackViewModel? pack)
        {
            if (pack != null && pack.Questions.Any()) // Kontrollera om det finns frågor i paketet
            {
                _activePack = pack;

                // Hämta och visa den första frågan
                ShowQuestion(_currentQuestionIndex);

                // Starta timer för första frågan
                StartTimer(_activePack.TimeLimitInSeconds);
            }
        }

        private void ShowQuestion(int questionIndex)
        {
            if (_activePack == null || _activePack.Questions.Count <= questionIndex)
            {
                return; // Om det inte finns någon fråga på det här indexet, gör inget
            }

            var question = _activePack.Questions[questionIndex];
            var answers = new List<string> { question.CorrectAnswer };
            answers.AddRange(question.IncorrectAnswers.Take(3));

            var random = new Random();
            var shuffledAnswers = answers.OrderBy(a => random.Next()).ToList();

            // Återställ svarsknapparnas färg innan vi visar nya svar
            ResetAnswerColors();

            // Sätt frågan och svaren
            Question = question.Query;
            AnswerOne = shuffledAnswers[0];
            AnswerTwo = shuffledAnswers[1];
            AnswerThree = shuffledAnswers[2];
            AnswerFour = shuffledAnswers[3];

            // Om det finns fler frågor, börja om timern
            StartTimer(_activePack.TimeLimitInSeconds);
        }

        private void ResetAnswerColors()
        {
            AnswerOneColor = "Transparent";
            AnswerTwoColor = "Transparent";
            AnswerThreeColor = "Transparent";
            AnswerFourColor = "Transparent";

            // Uppdatera visningen av färger
            RaisePropertyChanged(nameof(AnswerOneColor));
            RaisePropertyChanged(nameof(AnswerTwoColor));
            RaisePropertyChanged(nameof(AnswerThreeColor));
            RaisePropertyChanged(nameof(AnswerFourColor));
        }

        private void StartTimer(int timeLimitInSeconds)
        {
            _remainingTime = timeLimitInSeconds;
            UpdateTimeDisplay();
            _timer.Start(); // Starta timern
        }

        // Hantera knapptryckning
        private void OnAnswerClicked(object selectedAnswer)
        {
            // Konvertera objektet till ett svar
            string answer = selectedAnswer as string;
            if (answer != null)
            {
                CheckAnswer(answer); // Kontrollera om svaret är rätt eller fel
                _totalQuestions++;
            }
        }

        private void UpdateTimeDisplay()
        {
            TimeRemaining = $"{_remainingTime / 60:D2}:{_remainingTime % 60:D2}";
            RaisePropertyChanged(nameof(TimeRemaining));
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_remainingTime > 0)
            {
                _remainingTime--;
                UpdateTimeDisplay();
            }
            else
            {
                _timer.Stop();
                MoveToNextQuestion(); // Byt till nästa fråga när tiden är slut
            }
        }

        //Byt till nästa fråga
        private void MoveToNextQuestion()
        {
            _currentQuestionIndex++;

            if (_activePack != null && _activePack.Questions.Count > _currentQuestionIndex)
            {
                // Visa nästa fråga om det finns fler frågor
                ShowQuestion(_currentQuestionIndex);
            }
            else
            {
                // Om det inte finns fler frågor, avsluta spelet eller återgå till huvudmenyn.
                EndQuiz();
            }
        }

        private void EndQuiz()
        {
            ResultMessage = $"Quizet är slut! Du fick {_correctAnswers} av {_totalQuestions} rätt!";
            QuizIsRunning = false;
            QuizIsOver = true;
            RaisePropertyChanged(nameof(ResultMessage));
            RaisePropertyChanged(nameof(QuizIsOver));
            RaisePropertyChanged(nameof(QuizIsRunning));
        }

        // Nollställ quizets tillstånd
        private void ResetQuiz()
        {
            _currentQuestionIndex = 0;  // Nollställ frågaindex
            _remainingTime = 0;          // Återställ timer
            TimeRemaining = "00:00";     // Återställ tiddisplay

            Question = string.Empty;
            AnswerOne = string.Empty;
            AnswerTwo = string.Empty;
            AnswerThree = string.Empty;
            AnswerFour = string.Empty;
            ResultMessage = string.Empty;
            QuizIsOver = false;

            RaisePropertyChanged(nameof(TimeRemaining));
            RaisePropertyChanged(nameof(Question));
            RaisePropertyChanged(nameof(AnswerOne));
            RaisePropertyChanged(nameof(AnswerTwo));
            RaisePropertyChanged(nameof(AnswerThree));
            RaisePropertyChanged(nameof(AnswerFour));
            RaisePropertyChanged(nameof(ResultMessage));
            RaisePropertyChanged(nameof(QuizIsOver));
        }

        // Återstarta quizet
        private void RestartQuiz(object obj)
        {
            ResetQuiz();
            // Återgå till att välja ett nytt frågepaket eller börja ett nytt quiz
            TestData = "Start value: ";
        }

        private bool CanRestartQuiz(object obj)
        {
            return QuizIsOver; // Endast möjlig att klicka på om quizet är slut
        }

        // Metod för att hantera klick på svar
        public void CheckAnswer(string selectedAnswer)
        {
            if (selectedAnswer == _activePack.Questions[_currentQuestionIndex].CorrectAnswer)
            {
                // Rätt svar
                _correctAnswers++; // Öka antalet korrekta svar
                // Grönt för rätt svar
                AnswerOneColor = selectedAnswer == AnswerOne ? "Green" : AnswerOneColor;
                AnswerTwoColor = selectedAnswer == AnswerTwo ? "Green" : AnswerTwoColor;
                AnswerThreeColor = selectedAnswer == AnswerThree ? "Green" : AnswerThreeColor;
                AnswerFourColor = selectedAnswer == AnswerFour ? "Green" : AnswerFourColor;
            }
            else
            {
                // Röd för fel svar, grön för rätt svar
                AnswerOneColor = AnswerOne == _activePack.Questions[_currentQuestionIndex].CorrectAnswer ? "Green" : "Red";
                AnswerTwoColor = AnswerTwo == _activePack.Questions[_currentQuestionIndex].CorrectAnswer ? "Green" : "Red";
                AnswerThreeColor = AnswerThree == _activePack.Questions[_currentQuestionIndex].CorrectAnswer ? "Green" : "Red";
                AnswerFourColor = AnswerFour == _activePack.Questions[_currentQuestionIndex].CorrectAnswer ? "Green" : "Red";
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

    }
}
