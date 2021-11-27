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
using Controllers;
using MaterialDesignThemes.Wpf;
using OrderManagementSystem.Views;

namespace OrderManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Order Application Logic Layer
        /// </summary>
        private OrderController _orderController;

        /// <summary>
        /// Initializes the Main Window
        /// </summary>
        public MainWindow()
        {
            _orderController = new OrderController();

            InitializeComponent();
            frame.Navigate(new Views.OrderView());
        }

        /// <summary>
        /// Executed when the light/dark mode switch is untoggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Light Mode
                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();

                theme.SetBaseTheme(Theme.Light);
                paletteHelper.SetTheme(theme);

                Application.Current.Resources["CurrentIconColour"] = Brushes.Black;

                Application.Current.Resources["CurrentBackgroundColour"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#f0f0f0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Executed when the light/dark mode switch is toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Dark Mode
                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();

                theme.SetBaseTheme(Theme.Dark);
                paletteHelper.SetTheme(theme);

                Application.Current.Resources["CurrentIconColour"] = Brushes.White;

                Application.Current.Resources["CurrentBackgroundColour"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#3A3B3C");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates back to the Orders page. Warns the user when navigating away from the Add Order or Add Order Item page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentFrame = frame.Content as Page;

                switch (currentFrame.ToString())
                {
                    case "OrderManagementSystem.Views.AddOrder":
                        var addOrderMessage = MessageBox.Show("Going back to the Orders page will delete all unsaved order data. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (addOrderMessage == MessageBoxResult.No)
                        {
                            return;
                        }
                        else
                        {
                            _orderController.DeleteOrderAndOrderItems(AddOrder.NewOrderId);
                        }
                        break;
                    case "OrderManagementSystem.Views.AddOrderItem":
                        var orderItemMessage = MessageBox.Show("Going back to the Orders page will delete all unsaved order data. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (orderItemMessage == MessageBoxResult.No)
                        {
                            return;
                        }
                        else
                        {
                            _orderController.DeleteOrderAndOrderItems(AddOrder.NewOrderId);
                        }
                        break;
                }
                frame.Navigate(new Views.OrderView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manages data when the window closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Handle_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var currentFrame = frame.Content as Page;
                switch (currentFrame.ToString())
                {
                    case "OrderManagementSystem.Views.AddOrder":
                        var addOrderMessage = MessageBox.Show("Closing the application will delete all unsaved order details. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (addOrderMessage == MessageBoxResult.No)
                            e.Cancel = true;
                        else
                            _orderController.DeleteOrderAndOrderItems(AddOrder.NewOrderId);
                        return;
                    case "OrderManagementSystem.Views.AddOrderItem":
                        var orderItemMessage = MessageBox.Show("Closing the application will delete all unsaved order details. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (orderItemMessage == MessageBoxResult.No)
                            e.Cancel = true;
                        else
                            _orderController.DeleteOrderAndOrderItems(AddOrder.NewOrderId);
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
