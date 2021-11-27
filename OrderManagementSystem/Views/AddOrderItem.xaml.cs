using Controllers;
using DomainClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddOrderItem.xaml
    /// </summary>
    public partial class AddOrderItem : Page
    {
        /// <summary>
        /// Order Application Logic Layer
        /// </summary>
        private OrderController _orderController;

        /// <summary>
        /// Stock Application Logic Layer
        /// </summary>
        private StockItemController _stockItemController;

        /// <summary>
        /// The current order object
        /// </summary>
        private Order _currentOrder;

        /// <summary>
        /// The stock item selected from the DataGrid view. Must not be null for the Quantity textbox to be enabled
        /// </summary>
        public StockItem _selectedStockItem;

        /// <summary>
        /// Initializes the Add Order Item view
        /// </summary>
        /// <param name="order"></param>
        public AddOrderItem(Order currentOrder)
        {
            try
            {
                _orderController = new OrderController();
                _stockItemController = new StockItemController();
                InitializeComponent();

                this._currentOrder = currentOrder;

                dgStockItems.ItemsSource = _stockItemController.GetStockItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Adds an order item and navigates back to the Add Order view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(QuantityTextBox.Text) < 1)
            {
                MessageBox.Show("The quantity of a stock item must be a positive value greater than zero", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (int.Parse(QuantityTextBox.Text) > _selectedStockItem.InStock)
            {
                var addOrderMessage = MessageBox.Show("The requested quantity is higher than the stock of the selected item. Continuing may mean the order will be Rejected when processed. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (addOrderMessage == MessageBoxResult.No)
                {
                    return;
                }
            }

            try
            {
                _currentOrder = _orderController.UpsertOrderItem(_currentOrder, _selectedStockItem.Id, int.Parse(QuantityTextBox.Text));
                NavigationService.Navigate(new AddOrder(_currentOrder));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates back to the Add Order view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_To_Order(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddOrder(_currentOrder));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Event handler that fires when a DataGrid row is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedStockItem = (StockItem)dgStockItems.SelectedItem;
                SelectedItemLabel.Content = $"Selected: {_selectedStockItem.Name}";
                QuantityTextBox.IsReadOnly = false;
                QuantityTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
