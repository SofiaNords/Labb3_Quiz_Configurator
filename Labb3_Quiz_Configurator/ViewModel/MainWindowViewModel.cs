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

        public MainWindowViewModel()
        {
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
    }
}
