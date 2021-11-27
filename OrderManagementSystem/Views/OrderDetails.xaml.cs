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
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Page
    {
        /// <summary>
        /// Selected order object transferred from the Orders view
        /// </summary>
        private Order _order;

        /// <summary>
        /// Order Application Logic Layer
        /// </summary>
        private OrderController _orderController;

        /// <summary>
        /// Stock Item Application Logic Layer
        /// </summary>
        private StockItemController _stockItemController;

        /// <summary>
        /// Initializes the Order Details view
        /// </summary>
        /// <param name="order"></param>
        public OrderDetails(Order order)
        {
            try
            {
                _orderController = new OrderController();
                _stockItemController = new StockItemController();

                InitializeComponent();

                this._order = _orderController.GetOrder(order.Id);

                dgOrderItems.ItemsSource = _order.OrderItems;
                dgOrder.ItemsSource = new[] { _order };

                if (_order.OrderState.State == "Pending")
                {
                    btnProcess.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates back to the Orders view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_To_Orders(object sender, RoutedEventArgs e)
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
        /// Processes the order to Complete or Rejected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_Order(object sender, RoutedEventArgs e)
        {
            try
            {
                _orderController.ProcessOrder(_order.Id);
                // Refresh order details
                _order = _orderController.GetOrder(_order.Id);
                dgOrder.ItemsSource = null;
                dgOrder.ItemsSource = new[] { _order };
                btnProcess.IsEnabled = false;

                // Update stock amount if the process was set to Complete
                if (_order.OrderState.StateId == 4)
                {
                    foreach (var stockItem in _order.OrderItems)
                    {
                        _stockItemController.UpdateStockItemAmount(_order, stockItem.StockItemId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
