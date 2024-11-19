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
    }
}