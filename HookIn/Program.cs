using Controllers;
using DataAccessLayer;
using DomainClasses;
using System;
using System.Collections;
using System.ComponentModel;

namespace HookIn
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new Repository();
            var orderController = new OrderController();
            // Insert order
            //repo.InsertOrder();

            // Create order
            Order order = orderController.CreateNewOrder();

            // Create order items
            var orderItem1 = orderController.UpsertOrderItem(order, 1, 1);
            var orderItem2 = orderController.UpsertOrderItem(order, 2, 5);

            // Get order
            var retrieved = repo.GetOrders();
            foreach (var item in retrieved)
            {
                Console.WriteLine("Order ID: " + item.Id.ToString() + ", Order Total: " + item.Total.ToString());

                foreach (var orderitem in item.OrderItems)
                {
                    Console.WriteLine("Order Items: " + orderitem.OrderId);
                }
            }
            Console.ReadKey();
        }
    }
}
