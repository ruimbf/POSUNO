using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace POSUNO.Pages
{
    public sealed partial class MainPage : Page
    {

        private static MainPage _instance;

        public MainPage()
        {
            this.InitializeComponent();
            _instance = this;
        }


        public static MainPage Instance { get { return _instance; } }
        public static MainPage GetInstance()
        { 
            return _instance; 
        }

        public User User { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            User = (User)e.Parameter;
            WelcomeTextBlock.Text = $"Bem vindo: {User.FullName}";

            // defaulr apge
            MyFrame.Navigate(typeof(ProductsPage));
        }


        private async void LogoutImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialogResult dialog = await ConfirmLeaveAsync();
            if (dialog == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(LoginPage));
            }
        }

        private async Task<ContentDialogResult> ConfirmLeaveAsync()
        {
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Confirmação",
                Content = "Deseja sair?",
                PrimaryButtonText = "Sim",
                CloseButtonText = "Não"
            };

            return await confirmDialog.ShowAsync();
        }

        private void CustomersNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(CustomersPage));
        }

        private void ProductsNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(ProductsPage));
        }


    }
}
