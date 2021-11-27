using Controllers;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        /// <summary>
        /// Order Application Logic Layer
        /// </summary>
        private OrderController _orderController;

        /// <summary>
        /// The created Order object
        /// </summary>
        private Order _order;

        /// <summary>
        /// The ID of the newly created order
        /// </summary>
        public static int NewOrderId;

        /// <summary>
        /// Initializes the Add Order view
        /// </summary>
        public AddOrder(Order order = null)
        {
            try
            {
                _orderController = new OrderController();
                if (order == null)
                {
                    _order = _orderController.CreateNewOrder();
                }
                else
                {
                    _order = order;
                }

                NewOrderId = _order.Id;

                InitializeComponent();

                // Initialize Order Details
                IdLabel.Content = _order.Id;
                DateTimeLabel.Content = _order.DateTime;
                TotalCostLabel.Content = _order.Total.ToString("C0");
                OrderStateLabel.Content = _order.OrderState.State;

                // Fill out DataGrid table
                dgOrderItems.ItemsSource = _order.OrderItems;

                if (_order.OrderItems.Count > 0)
                {
                    OrderItemInfo.Visibility = Visibility.Hidden;
                    btnSubmit.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Adds an order and navigates back to the Orders view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new OrderView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates to the Add Order Item view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var NewOrderItem = new AddOrderItem(_order);
                NavigationService.Navigate(NewOrderItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Submits an order to Pending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderStateLabel.Content = _orderController.SubmitOrder(_order.Id).OrderState.State;
                btnSubmit.IsEnabled = false;
                btnAddOrderItem.IsEnabled = false;
                foreach (var item in FindVisualChildren<Button>(this))
                {
                    if (item.Name == "btnDeleteItem")
                    {
                        item.IsEnabled = false;
                    }
                }
                btnAddToOrders.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Deletes an order item from an order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderItem selectedItem = (OrderItem)dgOrderItems.SelectedItem;
                _orderController.DeleteOrderItem(NewOrderId, selectedItem.StockItemId);
                // Refresh the page details
                _order = _orderController.GetOrder(NewOrderId);
                TotalCostLabel.Content = _order.Total.ToString("C0");
                dgOrderItems.ItemsSource = _order.OrderItems;

                if (_order.OrderItems.Count == 0)
                {
                    OrderItemInfo.Visibility = Visibility.Visible;
                    btnSubmit.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Locates elements in the UI when created as a template (i.e the Delete order item button/s)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns>List of objects with the specified type</returns>
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}
