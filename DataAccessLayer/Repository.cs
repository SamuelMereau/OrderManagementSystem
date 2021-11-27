using DomainClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer
{
    public class Repository
    {

        #region Database Connection
        /// <summary>
        /// Connection to database
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Database Connection API: Microsoft.Data.SqlClient
        /// Establish connection to database
        /// </summary>
        public Repository()
        {
            // _connectionString = @"Connection String"
        }
        #endregion

        /// <summary>
        /// Inserts an order in the database
        /// </summary>
        /// <returns>Inserted order object</returns>
        public Order InsertOrder()
        {
            int result = 0;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_InsertOrderHeader", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    connection.Open();
                    result = Convert.ToInt32(command.ExecuteScalar());
                    return GetOrder(result);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves an order record from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retrieved order object</returns>
        public Order GetOrder(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_SelectOrderHeaderById", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    Order order = null;
                    var orderItems = new List<OrderItem>();
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(0);
                            int stateId = reader.GetInt32(1);
                            DateTime datetime = reader.GetDateTime(2);
                            // A new order won't have any stock items, checking for null will prevent any SqlNullValueException errors.
                            int stockItemId = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                            string orderItemName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            double price = reader.IsDBNull(5) ? 0.0 : Convert.ToDouble(reader.GetDecimal(5));
                            int quantity = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);

                            if (stockItemId != -1)
                            {
                                var orderItem = new OrderItem(
                                    orderId,
                                    stockItemId,
                                    orderItemName,
                                    price,
                                    quantity
                                );
                                orderItems.Add(orderItem);
                            } else
                            {
                                orderItems.Clear();
                            }

                            order = new Order(
                                orderId,
                                datetime.ToString("dd/M/yyyy hh:mm:ss tt"),
                                new OrderState(stateId),
                                orderItems
                            );
                        }
                    }
                    return order;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves all order records from the database
        /// </summary>
        /// <returns>List of order objects</returns>
        public IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_SelectOrderHeaders", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    Order order = null;
                    var orderItems = new List<OrderItem>();

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(0);
                            int stateId = reader.GetInt32(1);
                            DateTime datetime = reader.GetDateTime(2);
                            // A new order won't have any stock items, checking for null will prevent any SqlNullValueException errors.
                            // When we get orders for the orders view, only those with order items are listed
                            int stockItemId = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                            string orderItemName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            double price = reader.IsDBNull(5) ? 0.0 : Convert.ToDouble(reader.GetDecimal(5));
                            int quantity = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);

                            if (stockItemId != -1)
                            {
                                var orderItem = new OrderItem(
                                    orderId,
                                    stockItemId,
                                    orderItemName,
                                    price,
                                    quantity
                                );
                                orderItems.Add(orderItem);
                            }
                            else
                            {
                                orderItems.Clear();
                            }

                            order = new Order(
                                orderId,
                                datetime.ToString("dd/M/yyyy hh:mm:ss tt"),
                                new OrderState(stateId),
                                orderItems
                            );

                            orders.Add(order);

                            if (orders.Any(x => x.Id == orderId) == true && orders.Where(s => s.Id == orderId).Count() > 1)
                            {
                                orders.Remove(orders.FirstOrDefault(x => x.Id == orderId));
                            }
                        }
                    }
                    return orders;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Either inserts a new Order item or will update the quantity if the order item already exists
        /// </summary>
        /// <param name="orderItem"></param>
        /// <returns>Number of rows affected</returns>
        public int UpsertOrderItem(OrderItem orderItem)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_UpsertOrderItem", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    command.Parameters.Add("@orderHeaderId", SqlDbType.Int).Value = orderItem.OrderId;
                    command.Parameters.Add("@stockItemId", SqlDbType.Int).Value = orderItem.StockItemId;
                    command.Parameters.Add("@description", SqlDbType.VarChar).Value = orderItem.Name;
                    command.Parameters.Add("@price", SqlDbType.Decimal).Value = orderItem.Price;
                    command.Parameters.Add("@quantity", SqlDbType.Int).Value = orderItem.Quantity;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Updates the state of an order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Number of rows affected</returns>
        public int UpdateOrderState(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_UpdateOrderState", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    command.Parameters.Add("@orderHeaderId", SqlDbType.Int).Value = order.Id;
                    command.Parameters.Add("@stateId", SqlDbType.Int).Value = order.OrderState.StateId;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new InvalidOrderStateException(ex.Message, order);
                }
            }
        }

        /// <summary>
        /// Deletes the order and all order items associated with it
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Number of rows affected</returns>
        public int DeleteOrderAndOrderItems(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_DeleteOrderHeaderAndOrderItems", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    command.Parameters.Add("@orderHeaderId", SqlDbType.Int).Value = orderId;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Deletes an order item from an order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="stockItemId"></param>
        /// <returns>Number of rows affected</returns>
        public int DeleteOrderItem(int orderId, int stockItemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_DeleteOrderItem", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    command.Parameters.Add("@orderHeaderId", SqlDbType.Int).Value = orderId;
                    command.Parameters.Add("@stockItemId", SqlDbType.Int).Value = stockItemId;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        // Stock Items

        /// <summary>
        /// Retrieves all stock items from the database
        /// </summary>
        /// <returns>List of Stock Item objects</returns>
        public IEnumerable<StockItem> GetStockItems()
        {
            var stockItems = new List<StockItem>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_SelectStockItems", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            double price = Convert.ToDouble(reader.GetDecimal(2));
                            int instock = reader.GetInt32(3);

                            var stockItem = new StockItem(
                                id,
                                name,
                                instock,
                                price
                            );

                            stockItems.Add(stockItem);
                        }
                    }
                    return stockItems;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Retrieves a stock item from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retrieved Stock Item object</returns>
        public StockItem GetStockItem(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.sp_SelectStockItemById", connection) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    StockItem stockItem = null;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int stockItemId = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            double price = Convert.ToDouble(reader.GetDecimal(2));
                            int instock = reader.GetInt32(3);

                            stockItem = new StockItem(
                                stockItemId,
                                name,
                                instock,
                                price
                            );
                        }
                    }
                    return stockItem;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Updates the quantity of a stock item
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stockItemId"></param>
        /// <returns>Number of rows affected</returns>
        public int UpdateStockItemAmount(Order order, int stockItemId)
        {
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                var transaction = connection.BeginTransaction("UpdateStockAmountTransaction");
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "dbo.sp_UpdateStockItemAmount @id, @amount";
                    foreach (var orderItem in order.OrderItems)
                    {
                        if (orderItem.StockItemId == stockItemId)
                        {
                            command.Parameters.Add(new SqlParameter("@id", orderItem.StockItemId));
                            command.Parameters.Add(new SqlParameter("@amount", -orderItem.Quantity));
                            result = command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                    }
                    transaction.Commit();
                    return result;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
