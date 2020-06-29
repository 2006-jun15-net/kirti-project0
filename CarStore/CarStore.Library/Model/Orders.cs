using System;
namespace CarStore.Library.Model
{
    public class Orders
    {
        /// <summary>
        /// orders object: id, location, customer, price, date of order
        /// </summary>
        public Orders()
        {
        }

        /// <summary>
        /// order id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// location of where the order must be placed
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// customers id to associate the order with
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// cost of the order
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// date and time it was ordered at
        /// </summary>
        public DateTime OrderDate { get; set; }

    }
}
