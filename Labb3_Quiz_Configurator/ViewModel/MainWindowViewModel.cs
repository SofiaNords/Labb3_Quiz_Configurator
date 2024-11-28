using Labb3_Quiz_Configurator.Command;
using Labb3_Quiz_Configurator.Dialogs;
using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;
        private bool _isFullScreen;

        private JsonManager _jsonManager;

        public object CurrentView 
        {
            get => _currentView;
            set
            {
                _currentView = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEditEnabled)); 
                RaisePropertyChanged(nameof(IsPlayEnabled)); 
            }
        }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; } 
        public bool IsEditEnabled => CurrentView is not ConfigurationView; 
        public bool IsPlayEnabled => CurrentView is not PlayerView; 
       

        public DelegateCommand NewPackCommand { get; }
        public DelegateCommand OpenPackCommand { get; }
        public DelegateCommand SelectQuestionPackCommand { get; }
        public DelegateCommand SwitchToPlayerViewCommand { get; }
        public DelegateCommand SwitchToConfigurationViewCommand { get; }
        public DelegateCommand FullScreenCommand { get; }
        public DelegateCommand ExitCommand { get; }

        public MainWindowViewModel()
        {
            _jsonManager = new JsonManager();

            SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView);

            Packs = new ObservableCollection<QuestionPackViewModel>();

            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            CurrentView = new ConfigurationView();

            NewPackCommand = new DelegateCommand(OpenCreateNewPackDialog);
            
            OpenPackCommand = new DelegateCommand(OpenPackOption);

            SelectQuestionPackCommand = new DelegateCommand(SelectQuestionPack);

            FullScreenCommand = new DelegateCommand(_ => ToggleFullScreen());

            ExitCommand = new DelegateCommand(ExitApplication);

        }

        public async Task LoadQuestionPacksAsync()
        {
            List<QuestionPack> questionPacks = await _jsonManager.LoadQuestionPacks();

            foreach (var pack in questionPacks)
            {
                var packViewModel = new QuestionPackViewModel(pack);
                Packs.Add(packViewModel);
            }
        }

        public void ToggleFullScreen()
        {
            _isFullScreen = !_isFullScreen;
            var window = Application.Current.MainWindow;

            if (_isFullScreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Normal;
                window.Left = 0;
                window.Top = 0;
                window.Width = SystemParameters.PrimaryScreenWidth;
                window.Height = SystemParameters.PrimaryScreenHeight;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                window.Width = 800; 
                window.Height = 450; 
            }
        }

        private void SelectQuestionPack(object selectedPack)
        {
            if (selectedPack is QuestionPackViewModel pack)
            {
                ConfigurationViewModel.ActivePack = pack; 
            }
            CurrentView = new ConfigurationView(); 
        }

        private void SwitchToPlayerView(object obj)
        {
            PlayerViewModel.SetActivePack(ConfigurationViewModel.ActivePack); 
            CurrentView = new PlayerView(); 
        }

        private void SwitchToConfigurationView(object obj)
        {
            CurrentView = new ConfigurationView(); 
        }

        private void OpenPackOption(object obj)
        {
            var dialog = new PackOptionsDialog(ConfigurationViewModel);
            dialog.ShowDialog();
        }

        private void OpenCreateNewPackDialog(object obj)
        {
            ConfigurationViewModel.NewPack = new QuestionPackViewModel(new QuestionPack("<Packname>"));
            var dialog = new CreateNewPackDialog(this);
            dialog.ShowDialog();
        }
        private async void ExitApplication(object obj)
        {
            await SaveQuestionPacks();
            Application.Current.Shutdown();
        }

        public async Task SaveQuestionPacks() 
        {
            JsonManager jsonManager = new JsonManager();

            List<QuestionPack> questionPacks = new List<QuestionPack>();

            foreach (var packViewModel in Packs)
            {
                var questionPack = new QuestionPack(packViewModel.Name, packViewModel.Difficulty, packViewModel.TimeLimitInSeconds);
                questionPack.Questions = new List<Question>();

                foreach (var question in packViewModel.Questions)
                {
                    questionPack.Questions.Add(question);
                }
                questionPacks.Add(questionPack);
            }
            await jsonManager.SaveQuestionPacks(questionPacks);
        }
    }
}