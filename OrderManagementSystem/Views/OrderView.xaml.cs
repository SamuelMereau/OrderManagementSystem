using Controllers;
using DomainClasses;
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

namespace OrderManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : Page
    {
        /// <summary>
        /// Order Application Logic Layer
        /// </summary>
        private OrderController _orderController;

        /// <summary>
        /// Initializes the Order View view
        /// </summary>
        public OrderView()
        {
            try
            {
                _orderController = new OrderController();

                InitializeComponent();

                dgOrders.ItemsSource = _orderController.GetOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates to the Add Order view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddOrder());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates to the Order Details view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = dgOrders.SelectedItem as Order;

                NavigationService.Navigate(new OrderDetails(order));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
