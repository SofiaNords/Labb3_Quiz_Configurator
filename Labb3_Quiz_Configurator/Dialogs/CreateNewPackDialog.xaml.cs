using Labb3_Quiz_Configurator.ViewModel;
using System.Windows;

namespace Labb3_Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewPackDialog.xaml
    /// </summary>
    public partial class CreateNewPackDialog : Window
    {
        public CreateNewPackDialog(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new ConfigurationViewModel(mainWindowViewModel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}