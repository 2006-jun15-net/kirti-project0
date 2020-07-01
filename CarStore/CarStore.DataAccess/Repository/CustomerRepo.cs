using System;
using System.Linq;
using CarStore.DataAccess.Model;

namespace CarStore.Library.Repository
{
    /// <summary>
    /// customer repository
    /// </summary>
    public class CustomerRepo
    {
        public readonly IRepository<Customer> CReposity = new GenericRepo<Customer>();

        /// <summary>
        /// display customers
        /// </summary>
        public void DisplayCustomer ()
        {
            var customers = CReposity.GetAll().ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer ID [{customer.CustomerId}] {customer.FirstName} {customer.LastName}");
            }
        }
    }
}
