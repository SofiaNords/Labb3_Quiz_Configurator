using Labb3_Quiz_Configurator.ViewModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.Dialogs
{
    public partial class CreateNewPackDialog : Window
    {
        public CreateNewPackDialog(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel.ConfigurationViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}