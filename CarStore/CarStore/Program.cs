using System;
using System.Linq;
using CarStore.DataAccess.Model;
using CarStore.Library.Repository;

namespace CarStore
{
    class Program
    {
        // notes to self:
            // perfect customer order
            // finish location orders
            // fix prices
            // print products for product details
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var helperClass = new Helper();
            Customer customer = new Customer();

            while (true)
            {
                Console.WriteLine("a:\t Place order.");
                Console.WriteLine("b:\t Add a new customer/search customer.");
                Console.WriteLine("c:\t Display details of an order."); 
                Console.WriteLine("d:\t List customer orders."); //incmplete
                Console.WriteLine("e:\t List store location orders."); //incmplete
                Console.Write("q:\t To exit the store:\n");
                var userInput = Console.ReadLine();

                if (userInput == "q")
                {
                    Console.WriteLine("Exiting the store. \nGoodbye, come again!");
                    break;
                }
                else if (userInput == "b")
                {
                    customer = helperClass.AddCustomer();
                }
                else if (userInput == "a")
                {
                    helperClass.PlaceNewOrder(customer);
                }
                else if (userInput == "c")
                {
                    helperClass.DisplayOrderDetails();
                }
                else if (userInput == "d")
                {
                    CustomerRepo customerRepo = new CustomerRepo();
                    customerRepo.DisplayCustomer();
                    Console.WriteLine("Choose the customer ID to display their order history");
                    userInput = Console.ReadLine();
                    int customerId = Int32.Parse(userInput);

                    while (!customerRepo.CReposity.GetAll().Any(s => s.CustomerId == customerId))
                    {
                        Console.WriteLine($"Invalid input \"{customerId}\".");
                        Console.WriteLine("Choose the customer ID to display their order history");
                        userInput = Console.ReadLine();
                        customerId = Int32.Parse(userInput);
                    }
                    helperClass.CustomerOrders(customerId);
                }
            }
        }
    }
}
