using POSUNO.Components;
using POSUNO.Dialogs;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace POSUNO.Pages
{

    public sealed partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Customer> Customers { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadCustomersAsync();
        }

        private async void LoadCustomersAsync()
        {
            Loader loader = new Loader("Espere por favor...");
            loader.Show();
            Response response = await ApiService.GetListAsync<Customer>("Customers");
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Customer> customers = (List<Customer>)response.Result;
            Customers = new ObservableCollection<Customer>(customers);
            RefreshList();

        }


        private void RefreshList()
        {
            CustomersListView.ItemsSource = null;
            CustomersListView.Items.Clear();
            CustomersListView.ItemsSource = Customers;
        }


        private async void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            CustomerDialog dialog = new CustomerDialog(customer);
            await dialog.ShowAsync();
            if (!customer.WasSaved)
            { 
                return; 
            }

            customer.User = MainPage.Instance.User;

            Loader loader = new Loader("Espere por favor...");
            loader.Show();
            Response response = await ApiService.PostAsync("Customers", customer);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Erro");
                await messageDialog.ShowAsync();
                return;
            }

            Customer newCustomer = (Customer)response.Result;
            Customers.Add(newCustomer);
            RefreshList();
                    }

        private async void EditCustomer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Customer customer = Customers[CustomersListView.SelectedIndex];  // obter cliente selecionado
            customer.IsEdit = true;
            CustomerDialog dialog = new CustomerDialog(customer);
            await dialog.ShowAsync();

            if (!customer.WasSaved)
            {
                return;
            }

            customer.User = MainPage.Instance.User;

            Loader loader = new Loader("Espere por favor...");
            loader.Show();
            Response response = await ApiService.PutAsync("Customers", customer, customer.Id);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Erro");
                await messageDialog.ShowAsync();
                return;
            }

            Customer newCustomer = (Customer)response.Result;
            Customer oldCustomer = Customers.FirstOrDefault(c => c.Id == newCustomer.Id);
            oldCustomer = newCustomer;
            RefreshList();
        }

    }
}
