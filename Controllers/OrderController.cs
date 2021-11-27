using DataAccessLayer;
using DomainClasses;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace Controllers
{
    public class OrderController
    {
        /// <summary>
        /// The Data Access Layer
        /// </summary>
        private Repository _repo;

        /// <summary>
        /// Instantiates the Repository object
        /// </summary>
        public OrderController()
        {
            _repo = new Repository();
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of order objects</returns>
        public IEnumerable<Order> GetOrders()
        {
            try
            {
                return _repo.GetOrders();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets an order by ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrder(int orderId)
        {
            try
            {
                return _repo.GetOrder(orderId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <returns>New order object</returns>
        public Order CreateNewOrder()
        {
            try
            {
                return _repo.InsertOrder();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts a new order item in an order, or updates the quantity if it already exists
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stockItemId"></param>
        /// <param name="quantity"></param>
        /// <returns>Order object</returns>
        public Order UpsertOrderItem(Order order, int stockItemId, int quantity)
        {
            try
            {
                OrderItem orderItem = order.OrderItems.FirstOrDefault(x => x.StockItemId == stockItemId);

                if (orderItem == null)
                {
                    StockItem stockItem = _repo.GetStockItem(stockItemId);
                    orderItem = new OrderItem(order.Id, stockItemId, stockItem.Name, stockItem.Price, quantity);
                } else
                {
                    orderItem.Quantity += quantity;
                }

                _repo.UpsertOrderItem(orderItem);

                return _repo.GetOrder(order.Id);
            }   
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Changes the state of the order to Pending
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Updated order object</returns>
        public Order SubmitOrder(int orderId)
        {
            try
            {
                Order order = _repo.GetOrder(orderId);
                order.OrderState.Submit();
                _repo.UpdateOrderState(order);
                return order;
            }
            catch (Exception ex)
            {
                throw new InvalidOrderStateException(ex.Message);
            }
        }

        /// <summary>
        /// Changes the state of the order to Complete or Rejected based on stock
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Updated order object</returns>
        public Order ProcessOrder(int orderId)
        {
            try
            { 
                Order order = _repo.GetOrder(orderId);
                foreach (var item in order.OrderItems)
                {
                    if (item.Quantity > _repo.GetStockItem(item.StockItemId).InStock)
                    {
                        order.OrderState.Reject();
                        _repo.UpdateOrderState(order);
                        return _repo.GetOrder(order.Id);
                    }
                }
                order.OrderState.Complete();
                _repo.UpdateOrderState(order);
                return _repo.GetOrder(order.Id);
            }
            catch (Exception ex)
            {
                throw new InvalidOrderStateException(ex.Message);
            }
        }

        /// <summary>
        /// Deletes all data associated with an order
        /// </summary>
        /// <param name="orderId"></param>
        public void DeleteOrderAndOrderItems(int orderId)
        {
            try
            {
                _repo.DeleteOrderAndOrderItems(orderId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes an order item from an order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="stockId"></param>
        public void DeleteOrderItem(int orderId, int stockId)
        {
            try
            {
                _repo.DeleteOrderItem(orderId, stockId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
