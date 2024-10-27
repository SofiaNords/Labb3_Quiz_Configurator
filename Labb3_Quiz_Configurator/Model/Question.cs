namespace Labb3_Quiz_Configurator.Model
{
    // En klass som representerar en fråga i quizet
    public class Question
    {
        // Konstruktorn används för att skapa en ny instans av Question.
        // Den tar emot en fråga (query), ett korrekt svar (correctAnswer)
        // och tre inkorrekta svar...
        public Question(string query, string correctAnswer,
            string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            // Tilldelar egenskapen Query värdet av den givna frågan
            Query = query;
            // Tilldelar egenskapen CorrectAnswer det korrekta svaret.
            CorrectAnswer = correctAnswer;
            // Skapar en array för inkorrekta svar och tilldelar värden.
            IncorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }

        // Egenskap som representerar själva frågan.
        public string Query { get; set; }

        // Egenskap som representerar det korrekta svaret.
        public string CorrectAnswer { get; set; }

        // Egenskap som representerar de inkorrekta svaren.
        public string[] IncorrectAnswers { get; set; }
    }
}
