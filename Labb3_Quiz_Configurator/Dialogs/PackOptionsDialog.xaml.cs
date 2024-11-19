using Labb3_Quiz_Configurator.ViewModel;
using System.Windows;
using Labb3_Quiz_Configurator.Enums;

namespace Labb3_Quiz_Configurator.Dialogs
{
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }

        public PackOptionsDialog(ConfigurationViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
