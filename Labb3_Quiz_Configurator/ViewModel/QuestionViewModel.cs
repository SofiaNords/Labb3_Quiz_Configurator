using Labb3_Quiz_Configurator.Model;

namespace Labb3_Quiz_Configurator.ViewModel
{
    public class QuestionViewModel : ViewModelBase
    {
        private string query;
        private string correctAnswer;
        private string firstIncorrectAnswer;
        private string secondIncorrectAnswer;
        private string thirdIncorrectAnswer;

        public string Query
        {
            get => query;
            set { query = value; RaisePropertyChanged(); }
        }

        public string CorrectAnswer
        {
            get => correctAnswer;
            set { correctAnswer = value; RaisePropertyChanged(); }
        }

        public string FirstIncorrectAnswer
        {
            get => firstIncorrectAnswer;
            set { firstIncorrectAnswer = value; RaisePropertyChanged(); }
        }

        public string SecondIncorrectAnswer
        {
            get => secondIncorrectAnswer;
            set { secondIncorrectAnswer = value; RaisePropertyChanged(); }
        }

        public string ThirdIncorrectAnswer
        {
            get => thirdIncorrectAnswer;
            set { thirdIncorrectAnswer = value; RaisePropertyChanged(); }
        }

        public Question ToQuestion()
        {
            return new Question(Query, CorrectAnswer, FirstIncorrectAnswer, SecondIncorrectAnswer, ThirdIncorrectAnswer);
        }
    }
}
