using System;
using System.Linq;
using CarStore.DataAccess.Model;
using CarStore.Library.Repository;

namespace CarStore
{
    class Program
    {
        /// <summary>
        /// main method
        /// calls methods for each of the functionality
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var helperClass = new Helper();
            Customer customer = new Customer();

            while (true)
            {
                Console.WriteLine("a:\t Place order."); // DONE
                Console.WriteLine("b:\t Add a new customer"); //DONE
                Console.WriteLine("c:\t Display details of an order."); // DONE
                Console.WriteLine("d:\t List customer orders."); // DONE
                Console.WriteLine("e:\t List store location orders."); // DONE
                Console.WriteLine("f:\t Search customer by name."); //DONE
                Console.WriteLine("g:\t Add to location.");
                Console.WriteLine("h:\t Add products.");
                Console.WriteLine("i:\t Add stock.");
                Console.WriteLine("q:\t To exit the store:\n");

                Console.WriteLine("Select an option from above: ");
                var userInput = Console.ReadLine();
                Console.WriteLine();

                if (userInput == "q") //QUIT
                {
                    Console.WriteLine("Exiting the store. \nGoodbye, come again!");
                    break;
                }
                else if (userInput == "a") // PLACE ORDER
                {
                    int id = 0;
                    CustomerRepo customerRepo = new CustomerRepo();

                    if (customerRepo.CReposity.GetAll().FirstOrDefault() == null)
                        Console.WriteLine("There are no customers currently.");
                    else
                    {
                        customerRepo.DisplayCustomer();

                        Console.Write("Enter your customer ID: ");
                        string input = Console.ReadLine();
                        id = Int32.Parse(input);

                        while (id <= 0)
                        {
                            Console.WriteLine($"Invalid input \"{id}\".");
                            Console.Write("Enter your customer ID: ");
                            input = Console.ReadLine();
                            id = Int32.Parse(input);
                        }

                    }

                    if (id > 0)
                    {
                        helperClass.PlaceNewOrder(id);
                    }

                }
                else if (userInput == "b") //ADD CUSTOMER
                {
                    helperClass.AddCustomer(); 
                }
                else if (userInput == "c") //DISPLAY ORDER DETAILS
                {
                    OrdersRepo ordersRepo = new OrdersRepo();
                    ordersRepo.DisplayOrders();

                    Console.Write("\nEnter ID of the order that you would like to display: ");
                    string input = Console.ReadLine();
                    int orderId = Int32.Parse(input);

                    while (!ordersRepo.OReposity.GetAll().Any(s => s.OrderId == orderId))
                    {
                        Console.WriteLine($"Invalid input \"{orderId}\".");
                        Console.Write("Enter ID of the order that you would like to display: ");
                        input = Console.ReadLine();
                        orderId = Int32.Parse(input);
                    }

                    helperClass.DisplayOrderDetails(orderId);
                }
                else if (userInput == "d") //DISPLAY CUSTOMER ORDERS
                {
                    CustomerRepo customerRepo = new CustomerRepo();
                    customerRepo.DisplayCustomer();
                    Console.WriteLine("\nChoose the customer ID to display their order history");
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
                else if (userInput == "e") //DISPLAY location ORDERS
                {
                    LocationRepo locationRepo = new LocationRepo();
                    locationRepo.DisplayLocation();
                    Console.WriteLine("\nChoose the location ID to display their order history");
                    userInput = Console.ReadLine();
                    int locationId = Int32.Parse(userInput);

                    while (!locationRepo.LReposity.GetAll().Any(s => s.LocationId == locationId))
                    {
                        Console.WriteLine($"Invalid input \"{locationId}\".");
                        Console.WriteLine("Choose the location ID to display their order history");
                        userInput = Console.ReadLine();
                        locationId = Int32.Parse(userInput);
                    }
                    helperClass.LocationOrders(locationId);
                }
                else if (userInput == "f") //SEARCH CUSTOMER BY NAME
                {
                    Console.WriteLine("Enter the first name of the customer you would like to search");
                    string name = Console.ReadLine();
                    helperClass.SearCustomerByName(name);
                } 
                else if (userInput == "g") // ADD LOCATION
                {
                    helperClass.AddLocation();
                }
                else if (userInput == "h") //ADD PRODUCTS
                {
                    helperClass.AddProducts();
                }
                else if (userInput == "i") //ADD STOCK OF THE PRODUCTS
                {

                    helperClass.AddStock();
                }
            }
        }
    }
}
