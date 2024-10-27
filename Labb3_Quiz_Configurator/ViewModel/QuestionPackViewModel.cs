using Labb3_Quiz_Configurator.Model;
using System.Collections.ObjectModel;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // ViewModel som representerar ett frågepaket.
    public class QuestionPackViewModel : ViewModelBase
    {
        // En privat variabel för att hålla referensen till modellen QuestionPack
        private readonly QuestionPack model;

        // Egenskap som representerar listan av frågor i paketet.
        public ObservableCollection<Question> Questions { get; }

        // Konstruktorn tar en QuestionPack som parameter
        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            Questions = new ObservableCollection<Question>(model.Questions); // Initiera Questions här
        }

        // Egenskap som representerar namnet på frågepaketet.
        public string Name
        {
            get => model.Name; // Hämtar namnet från modellen.
            set
            {
                model.Name = value; // Sätter namnet i modellen.
                RaisePropertyChanged(); // Signalera att namnet har ändrats.
            }
        }

        // Egenskap för svårighetsgrad
        public Difficulty Difficulty
        {
            get => model.Difficulty; // Hämtar svårighetsgraden från modellen.
            set
            {
                model.Difficulty = value; // Sätter svårighetsgraden i modellen.
                RaisePropertyChanged(); // Signalera att svårighetsgraden har ändrats.
            }
        }

        // Egenskap för tidsgräns i sekunder
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds; // Hämtar tidsgränsen från modellen.
            set
            {
                model.TimeLimitInSeconds = value; // Sätter tidsgränsen i modellen.
                RaisePropertyChanged(); // Signalera att tidsgränsen har ändrats.
            }
        }
    }
}
