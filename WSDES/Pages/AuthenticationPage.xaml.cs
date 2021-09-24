using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WSDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
        public AuthenticationPage()
        {
            InitializeComponent();
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var authenticationData = IsolatedStorageData.GetAuthenticationData();

            if (authenticationData != null)
            {
                emailBox.Text = authenticationData.Email;
                passwordBox.Password = authenticationData.Password;

                ((dynamic)this.DataContext).Password = authenticationData.Password;
                ((dynamic)this.DataContext).Email = authenticationData.Email;
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
