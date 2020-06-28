using System;
using CarStore.DataAccess.Model;
using CarStore.Library.Repository;
//using CarStore.Library.Model;

namespace CarStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var helperClass = new Helper();
            Customer customer = new Customer();

            while (true)
            {
                Console.WriteLine("a:\t Placee order."); //incmplete
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
                    //customer = helperClass.AddCustomer();
                    helperClass.AddProducts();
                }
                else if (userInput == "c")
                {
                    Console.Write("Enter ID of the order that you would like to display: ");
                    string input = Console.ReadLine();
                    int id = Int32.Parse(input);

                    while (id <= 0)
                    {
                        if (id <= 0)
                            Console.WriteLine($"Invalid input \"{id}\".");

                        Console.Write("Enter ID of the order that you would like to display: ");
                        input = Console.ReadLine();
                        id = Int32.Parse(input);
                    }
                    OrdersRepo ordersRepo = new OrdersRepo();
                    ordersRepo.OReposity.GetWithId(id);
                }
                else if (userInput == "d")
                {
                    helperClass.CustomerOrders(); //need to implement
                }
            }
        }
    }
}
