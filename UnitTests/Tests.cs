using Controllers;
using DataAccessLayer;
using DomainClasses;
using NUnit.Framework;

namespace UnitTests
{
    public class Tests
    {
        private OrderController _orderController;
        private Repository _repo;

        /// <summary>
        /// Intialize the Order Controller
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _orderController = new OrderController();
            _repo = new Repository();
        }

        /// <summary>
        /// Test 5: Proccessing an Order
        /// </summary>
        [Test]
        public void ProcessOrder()
        {
            // Create order
            Order order = _orderController.CreateNewOrder();

            // Create order items
            var orderItem1 = _orderController.UpsertOrderItem(order, 1, 1);
            var orderItem2 = _orderController.UpsertOrderItem(order, 2, 5);

            Order submittedOrder = _orderController.SubmitOrder(order.Id);

            // Process order
            Order processedOrder = _orderController.ProcessOrder(submittedOrder.Id);

            // Confirm the order state Id has been updated to 4
            Assert.AreEqual(4, processedOrder.OrderState.StateId);

            // Confirm the state text updates to "Complete"
            Assert.AreEqual("Complete", processedOrder.OrderState.State);

            // Confirm the state change is reflected in the OrderHeaders table in the database
            Assert.AreEqual(4, _orderController.GetOrder(order.Id).OrderState.StateId);
        }

        /// <summary>
        /// Test 1: Add Order
        /// </summary>
        [Test]
        public void AddOrder()
        {
            // Create order
            Order order = _orderController.CreateNewOrder();

            // Create order items
            var orderItem1 = _orderController.UpsertOrderItem(order, 1, 1);
            var orderItem2 = _orderController.UpsertOrderItem(order, 2, 5);

            // Confirm the order object has been correctly instantiated
            if (order == null)
            {
                Assert.Fail("The order was not correctly instantiated");
            }

            // Confirm the identification of the order is present
            if (order.Id == 0)
            {
                Assert.Fail("The ID property does not match test data");
            }

            // Confirm a new row has been created in the OrderHeaders database table
            if (_orderController.GetOrder(order.Id).GetType() != typeof(Order)) 
            {
                Assert.Fail("The order was not found in the database");
            }

            // Confirm the state of a new order
            Assert.AreEqual(1, order.OrderState.StateId);
        }

        /// <summary>
        /// Test 2: Adding an Order Item
        /// </summary>
        [Test]
        public void AddOrderItem()
        {
            // Create order
            Order order = _orderController.CreateNewOrder();

            // Create order items
            var orderItem1 = _orderController.UpsertOrderItem(order, 1, 1);
            var orderItem2 = _orderController.UpsertOrderItem(order, 2, 5);

            Order orderWithItems = _orderController.GetOrder(order.Id);

            // Confirm the OrderId refers to the correct parent order
            Assert.AreEqual(2, orderWithItems.OrderItems.Count);

            foreach (var orderItem in orderWithItems.OrderItems)
            {
                Assert.AreEqual(orderWithItems.Id, orderItem.OrderId);
            }

            // Confirm a new row has been created in the OrderItems database, and the OrderItem object has been instantiated correctly
            foreach (var orderItem in orderWithItems.OrderItems)
            {
                if (orderItem.GetType() != typeof(OrderItem))
                {
                    Assert.Fail("An order item is not of OrderItem type");
                }
            }

            // Confirm the values in an order are present and calculated correctly
            Assert.AreEqual(1, orderWithItems.OrderItems.ToArray()[0].StockItemId);
            Assert.AreEqual("Table", orderWithItems.OrderItems.ToArray()[0].Name);
            Assert.AreEqual(1, orderWithItems.OrderItems.ToArray()[0].Quantity);
            Assert.AreEqual(100, orderWithItems.OrderItems.ToArray()[0].Total);

            Assert.AreEqual(2, orderWithItems.OrderItems.ToArray()[1].StockItemId);
            Assert.AreEqual("Chair", orderWithItems.OrderItems.ToArray()[1].Name);
            Assert.AreEqual(5, orderWithItems.OrderItems.ToArray()[1].Quantity);
            Assert.AreEqual(125, orderWithItems.OrderItems.ToArray()[1].Total);

            Assert.AreEqual(2, orderWithItems.OrderItems.Count);
            Assert.AreEqual(225, orderWithItems.Total);
        }

        /// <summary>
        /// Test 3: Submitting an Order
        /// </summary>
        [Test]
        public void SubmitOrder()
        {
            // Create order
            Order order = _orderController.CreateNewOrder();

            // Create order items
            var orderItem1 = _orderController.UpsertOrderItem(order, 1, 1);
            var orderItem2 = _orderController.UpsertOrderItem(order, 2, 5);

            // Submit order
            Order submittedOrder = _orderController.SubmitOrder(order.Id);

            // Confirm the order state ID has been updated to 2
            Assert.AreEqual(2, submittedOrder.OrderState.StateId);

            // Confirm the state text updates to "Pending"
            Assert.AreEqual("Pending", submittedOrder.OrderState.State);

            // Confirm the state change is reflected in the OrderHeaders table in the database
            Assert.AreEqual(2, _orderController.GetOrder(order.Id).OrderState.StateId);
        }
    }
}