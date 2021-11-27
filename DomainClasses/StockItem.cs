using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainClasses
{
    public class StockItem
    {
        /// <summary>
        /// Constructs the StockItem object
        /// </summary>
        public StockItem(int id, string name, int stock, double price)
        {
            this.Id = id;
            this.Name = name;
            this.InStock = stock;
            this.Price = price;
        }

        /// <summary>
        /// The ID of the Stock Item
        /// </summary>
        private int _id;

        /// <summary>
        /// The ID of the Stock Item
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The quantity of the stock available for the item
        /// </summary>
        private int _inStock;

        /// <summary>
        /// The quantity of stock available for the item
        /// </summary>
        public int InStock
        {
            get { return _inStock; }
            set { _inStock = value; }
        }

        /// <summary>
        /// The name of the stock item
        /// </summary>
        private string _name;

        /// <summary>
        /// The name of the stock item
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The price of the stock item
        /// </summary>
        private double _price;

        /// <summary>
        /// The price of the stock item
        /// </summary>
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
    }
}