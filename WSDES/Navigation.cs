using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WSDES.Pages;
using WSDES.Windows;

namespace WSDES
{
    public static class Navigation
    {
        public static Dictionary<string, Page> AvailablePages { get; set; }
        public static Window CurrentWindow { get; set; }
        public static void Initialize(Window window)
        {
            CurrentWindow = window;

            if (AvailablePages == null)
            {
                AvailablePages = new Dictionary<string, Page>();
            }
            else
            {
                AvailablePages.Clear();
            }

            if (CurrentWindow is LoginWindow)
            {
                AvailablePages.Add("AuthenticationPage", new AuthenticationPage());
                AvailablePages.Add("RegistrationPage", new RegistrationPage());
            }

            if (CurrentWindow is MainWindow) { }
        }

        public static void Navigate(string pageName)
        {
            if (AvailablePages == null || !AvailablePages.ContainsKey(pageName))
            {
                return;
            }

            if (CurrentWindow is LoginWindow) { (CurrentWindow as LoginWindow).Frame.Navigate(AvailablePages[pageName]); }
            else { (CurrentWindow as MainWindow).Frame.Navigate(AvailablePages[pageName]); }
        }
    }
}
