using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb3_Quiz_Configurator.ViewModel
{
    // Bas-ViewModel-klass som implementerar INotifyPropertyChanged
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Event som används för att signalera att en egenskap har ändrats.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Metod för att höja (raise) PropertyChanged-eventet.
        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            // Anropar eventet om det finns några lyssnare.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
