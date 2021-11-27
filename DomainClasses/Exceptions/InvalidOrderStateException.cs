using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainClasses
{
    public class InvalidOrderStateException : Exception
    {
        private Order _order;

        /// <summary>
        /// Thrown when an order contains an invalid state
        /// </summary>
        public InvalidOrderStateException(Order order)
        {
            this._order = order;
        }

        /// <summary>
        /// Thrown when an order contains an invalid state. Can contain an additional message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="order"></param>
        public InvalidOrderStateException(string message, Order order) : base (message)
        {
            this._order = order;
        }

        /// <summary>
        /// Thrown when an order contains an invalid state. Only contains a message
        /// </summary>
        /// <param name="message"></param>
        public InvalidOrderStateException(string message) : base(message)
        {

        }
    }
}