using Labb3_Quiz_Configurator.Model;
using Labb3_Quiz_Configurator.ViewModel;
using System.Windows;

namespace Labb3_Quiz_Configurator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = (MainWindowViewModel)DataContext;
            await viewModel.SaveQuestionPacks();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowViewModel)DataContext;
            await viewModel.LoadQuestionPacksAsync();
            viewModel.ConfigurationViewModel.AddDefaultPackIfNoPackExists();
        }
    }
}