using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WSDES.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateToLoginPageCommand => new RelayCommand(o =>
        {
            Navigation.Navigate("AuthenticationPage");
        });

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
