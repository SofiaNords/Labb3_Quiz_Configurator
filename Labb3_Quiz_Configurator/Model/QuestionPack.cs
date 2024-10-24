namespace Labb3_Quiz_Configurator.Model
{
    // Enum som definierar svårighetsgrader för quizzen.
    enum Difficulty { Easy, Medium, Hard }

    // Klass som representerar ett paket med frågor (frågepaket).
    class QuestionPack
    {
        // Konstruktorn för att skapa ett nytt frågepaket.
        // Den tar emot ett namn, en svårighetesgrad (standard är Medium)
        // och en tidsgräns i sekunder (standard är 30 sekunder).
        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            // Tilldelar egenskapen Name värdet av det givna namnet
            Name = name;
            // Tilldelar egenskapen Difficulty den valda svårighetsgraden
            Difficulty = difficulty;
            // Tilldelar egenskapen TimeLimitInSeconds den angivna tidsgränsen
            TimeLimitInSeconds = timeLimitInSeconds;
            // Initierar en tom lista för frågor
            Questions = new List<Question>();
        }

        // Egenskap som representerar namnet på frågepaktetet.
        public string Name { get; set; }

        // Egenskap som representerar svårighetsgraden av frågorna i paketet.
        public Difficulty Difficulty { get; set; }

        // Egenskap som representerar tidsgränsen för att svara på frågorna.
        public int TimeLimitInSeconds { get; set; }

        // Egenskap som representerar listan av frågor i paketet.
        public List<Question> Questions { get; set; }
    }
}
