using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;

namespace Labb3_Quiz_Configurator.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        public ConfigurationViewModel ConfigurationViewModel { get; }

        public PlayerViewModel PlayerViewModel { get; }

        private QuestionPackViewModel? _activePack;

		public QuestionPackViewModel? ActivePack
		{
			get => _activePack;
			set
			{ 
				_activePack = value;
				RaisePropertyChanged();
			}
		}

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);
            
            PlayerViewModel = new PlayerViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
        }

    }
}
