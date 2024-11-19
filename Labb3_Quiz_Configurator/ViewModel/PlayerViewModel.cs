using Labb3_Quiz_Configurator.Command;
using System.Windows.Threading;

namespace Labb3_Quiz_Configurator.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private DispatcherTimer _timer;
        private int _remainingTime;
        private int _currentQuestionIndex = 0; 
        private int _correctAnswers = 0; 
        private int _totalQuestions = 0; 
        private bool _quizIsRunning;
        private bool _quizIsOver;
        private string _question; 
        private string _answerOne;
        private string _answerTwo;
        private string _answerThree;
        private string _answerFour;
        private string _answerOneColor = "LightGray";
        private string _answerTwoColor = "LightGray";
        private string _answerThreeColor = "LightGray";
        private string _answerFourColor = "LightGray";

        private QuestionPackViewModel? _activePack; 

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

        public bool QuizIsRunning
        {
            get => _quizIsRunning;
            set
            {
                if (_quizIsRunning != value)
                {
                    _quizIsRunning = value;
                    RaisePropertyChanged(nameof(QuizIsRunning));
                }
            }
        }

        public bool QuizIsOver
        {
            get => _quizIsOver;
            set
            {
                if (_quizIsOver != value)
                {
                    _quizIsOver = value;
                    RaisePropertyChanged(nameof(QuizIsOver));
                }
            }
        }

        public DelegateCommand RestartQuizCommand { get; }

        public DelegateCommand AnswerCommand { get; }


        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;

            QuizIsRunning = true;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            RestartQuizCommand = new DelegateCommand(RestartQuiz);
            AnswerCommand = new DelegateCommand(OnAnswerClicked);
        }

        public void SetActivePack(QuestionPackViewModel? pack)
        {
            if (pack != null && pack.Questions.Any()) 
            {
                _activePack = pack;

                ShowQuestion(_currentQuestionIndex);

                StartTimer(_activePack.TimeLimitInSeconds);
            }
        }

        private void ShowQuestion(int questionIndex)
        {
            if (_activePack == null || _activePack.Questions.Count <= questionIndex)
            {
                return; 
            }

            var question = _activePack.Questions[questionIndex];
            var answers = new List<string> { question.CorrectAnswer };
            answers.AddRange(question.IncorrectAnswers.Take(3));

            var random = new Random();
            var shuffledAnswers = answers.OrderBy(a => random.Next()).ToList();

            ResetAnswerColors();

            Question = question.Query;
            AnswerOne = shuffledAnswers[0];
            AnswerTwo = shuffledAnswers[1];
            AnswerThree = shuffledAnswers[2];
            AnswerFour = shuffledAnswers[3];

            StartTimer(_activePack.TimeLimitInSeconds);
        }

        private void ResetAnswerColors()
        {
            AnswerOneColor = "LightGray";
            AnswerTwoColor = "LightGray";
            AnswerThreeColor = "LightGray";
            AnswerFourColor = "LightGray";

            RaisePropertyChanged(nameof(AnswerOneColor));
            RaisePropertyChanged(nameof(AnswerTwoColor));
            RaisePropertyChanged(nameof(AnswerThreeColor));
            RaisePropertyChanged(nameof(AnswerFourColor));
        }

        private void StartTimer(int timeLimitInSeconds)
        {
            _remainingTime = timeLimitInSeconds;
            UpdateTimeDisplay();
            _timer.Start(); 
        }

        private void UpdateTimeDisplay()
        {
            TimeRemaining = $"{_remainingTime / 60:D2}:{_remainingTime % 60:D2}";
            RaisePropertyChanged(nameof(TimeRemaining));
        }

        private void OnAnswerClicked(object selectedAnswer)
        {
            string answer = selectedAnswer as string;
            if (answer != null)
            {
                CheckAnswer(answer); 
            }
        }

        public void CheckAnswer(string selectedAnswer)
        {
            string correctColor = "Green";
            string incorrectColor = "Red";

            if (selectedAnswer == _activePack.Questions[_currentQuestionIndex].CorrectAnswer)
            {
                _correctAnswers++; 

                if (selectedAnswer == AnswerOne)
                {
                    AnswerOneColor = correctColor;
                }
                else if (selectedAnswer == AnswerTwo)
                {
                    AnswerTwoColor = correctColor;
                }
                else if (selectedAnswer == AnswerThree)
                {
                    AnswerThreeColor = correctColor;
                }
                else if (selectedAnswer == AnswerFour)
                {
                    AnswerFourColor = correctColor;
                }
            }
            else
            {
                if (selectedAnswer == AnswerOne)
                {
                    AnswerOneColor = incorrectColor;
                }
                else if (selectedAnswer == AnswerTwo)
                {
                    AnswerTwoColor = incorrectColor;
                }
                else if (selectedAnswer == AnswerThree)
                {
                    AnswerThreeColor = incorrectColor;
                }
                else if (selectedAnswer == AnswerFour)
                {
                    AnswerFourColor = incorrectColor;
                }

                if (AnswerOne == _activePack.Questions[_currentQuestionIndex].CorrectAnswer)
                {
                    AnswerOneColor = correctColor;
                }
                else if (AnswerTwo == _activePack.Questions[_currentQuestionIndex].CorrectAnswer)
                {
                    AnswerTwoColor = correctColor;
                }
                else if (AnswerThree == _activePack.Questions[_currentQuestionIndex].CorrectAnswer)
                {
                    AnswerThreeColor = correctColor;
                }
                else if (AnswerFour == _activePack.Questions[_currentQuestionIndex].CorrectAnswer)
                {
                    AnswerFourColor = correctColor;
                }
            }

            RaisePropertyChanged(nameof(AnswerOneColor));
            RaisePropertyChanged(nameof(AnswerTwoColor));
            RaisePropertyChanged(nameof(AnswerThreeColor));
            RaisePropertyChanged(nameof(AnswerFourColor));
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
                MoveToNextQuestion(); 
            }
        }

        private void MoveToNextQuestion()
        {
            _currentQuestionIndex++;
            _totalQuestions++;

            if (_activePack != null && _activePack.Questions.Count > _currentQuestionIndex)
            {
                ShowQuestion(_currentQuestionIndex);
            }
            else
            {
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

        private void RestartQuiz(object obj)
        {
            ResetQuiz();

            if (_activePack != null && _activePack.Questions.Any())
            {
                _currentQuestionIndex = 0; 
                ShowQuestion(_currentQuestionIndex); 
                StartTimer(_activePack.TimeLimitInSeconds);
            }
        }
        private void ResetQuiz()
        {
            _currentQuestionIndex = 0; 
            _totalQuestions = 0;
            _correctAnswers = 0;
            _remainingTime = 0;
            TimeRemaining = "00:00"; 

            Question = string.Empty;
            AnswerOne = string.Empty;
            AnswerTwo = string.Empty;
            AnswerThree = string.Empty;
            AnswerFour = string.Empty;
            ResultMessage = string.Empty;
            
            QuizIsOver = false;
            QuizIsRunning = true;

            RaisePropertyChanged(nameof(TimeRemaining));
            RaisePropertyChanged(nameof(Question));
            RaisePropertyChanged(nameof(AnswerOne));
            RaisePropertyChanged(nameof(AnswerTwo));
            RaisePropertyChanged(nameof(AnswerThree));
            RaisePropertyChanged(nameof(AnswerFour));
            RaisePropertyChanged(nameof(ResultMessage));
            RaisePropertyChanged(nameof(QuizIsOver));
        }
    }
}
