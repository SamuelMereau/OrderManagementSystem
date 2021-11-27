using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainClasses
{
    public class Order
    {
        /// <summary>
        /// Constructs a new Order object
        /// </summary>
        public Order(int id, string datetime, OrderState state, List<OrderItem> orderItems)
        {
            this.Id = id;
            this.DateTime = datetime;
            this.OrderState = state;
            this.OrderItems = orderItems;
        }

        /// <summary>
        /// The date and time the order was created
        /// </summary>
        private string _dateTime;

        /// <summary>
        /// The date and time the order was created
        /// </summary>
        public string DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        /// <summary>
        /// The ID of the order
        /// </summary>
        private int _id;

        /// <summary>
        /// The ID of the order
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The state of an order
        /// </summary>
        private OrderState _orderState;

        /// <summary>
        /// The state of an order
        /// </summary>
        public OrderState OrderState
        {
            get { return _orderState; }
            set { _orderState = value; }
        }

        /// <summary>
        /// The total cost of the order
        /// </summary>
        public double Total
        {
            get { return  _orderItems.Sum(item => item.Total); }
        }

        /// <summary>
        /// The list of order items contained in an order
        /// </summary>
        private List<OrderItem> _orderItems;

        /// <summary>
        /// Contains all items within an order
        /// </summary>
        public List<OrderItem> OrderItems
        {
            get { return _orderItems; }
            set { _orderItems = value.FindAll(item => item.OrderId == this.Id); }
        }
    }
}