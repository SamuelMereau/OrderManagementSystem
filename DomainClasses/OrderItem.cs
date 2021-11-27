using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainClasses
{
    public class OrderItem
    {
        /// <summary>
        /// Constructs an OrderItem
        /// </summary>
        public OrderItem(int orderId, int stockItemId, string name, double price, int quantity)
        {
            this.OrderId = orderId;
            this.StockItemId = stockItemId;
            this.Name = name;
            this._price = price;
            this._quantity = quantity;
        }

        /// <summary>
        /// The name of an order item
        /// </summary>
        private string _name;

        /// <summary>
        /// The name of an order item
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The ID of the order the order item is associated with
        /// </summary>
        private int _orderId;

        /// <summary>
        /// The ID of the order the order item is associated with
        /// </summary>
        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        /// <summary>
        /// The price of an order item
        /// </summary>
        private double _price;

        /// <summary>
        /// The price of the order item
        /// </summary>
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// The quantity of an order item
        /// </summary>
        private int _quantity;

        /// <summary>
        /// The quantity of the order item
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// The stock item ID of the order item
        /// </summary>
        private int _stockItemId;

        /// <summary>
        /// The stock item ID of the order item
        /// </summary>
        public int StockItemId
        {
            get { return _stockItemId; }
            set { _stockItemId = value; }
        }

        /// <summary>
        /// The total cost of the order item (Price * Quantity)
        /// </summary>
        public double Total
        {
            get { return this._price * this._quantity; }
        }
    }
}