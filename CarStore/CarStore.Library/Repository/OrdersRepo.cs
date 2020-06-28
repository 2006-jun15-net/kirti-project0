using System;
using System.Collections.Generic;
using System.Linq;
using CarStore.DataAccess.Model;

namespace CarStore.Library.Repository
{
    /// <summary>
    /// order repository
    /// </summary>
    public class OrdersRepo
    {
        public readonly IRepository<Orders> OReposity = new GenericRepo<Orders>();

        /// <summary>
        /// print all orders
        /// </summary>
        public void DisplayOrders()
        {
            var orders = OReposity.GetAll().ToList();

            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID [{order.OrderId}], this order was placed by [{order.CustomerId}] customer ID at location ID [{order.LocationId}] on {order.OrderDate}");
            }
        }
    }
}
