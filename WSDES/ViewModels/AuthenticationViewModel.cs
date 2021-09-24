using Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WSDES.ViewModels
{
    public class AuthenticationViewModel : INotifyPropertyChanged
    {
        public AuthenticationViewModel()
        {
            ButtonEnabled = !ButtonEnabled;
        }
        public bool IsRemember { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private bool buttonEnabled;
        public bool ButtonEnabled {
            get { return buttonEnabled; }
            set
            {
                buttonEnabled = value;
                OnPropertyChanged("ButtonEnabled");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
        public ICommand NavigateToRegistrationPageCommand {
            get => new RelayCommand(o => {
                Navigation.Navigate("RegistrationPage");
            });
        }

        public ICommand AuthenticationCommand => new RelayCommand(async o =>
        {
            ButtonEnabled = !ButtonEnabled;

            try
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                    ErrorMessage = string.Empty;

                await Authentication.Login(Email, Password);

                if (IsRemember)
                {
                    App.Current.Properties.Add("email", Email);
                    App.Current.Properties.Add("password", Password);
                    
                    IsolatedStorageData.Isolate();

                    ApplicationMessages.Show("Isolated");
                }
            }
            catch (Exception e)
            {
                ApplicationMessages.Show(e.ToString());
                ErrorMessage = e.Message;
            }

            ButtonEnabled = !ButtonEnabled;
        });


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
