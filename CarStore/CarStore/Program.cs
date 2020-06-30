using System;
using System.Linq;
using CarStore.DataAccess.Model;
using CarStore.Library.Repository;

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
                Console.WriteLine("a:\t Place order."); // does not calculate price
                Console.WriteLine("b:\t Add a new customer"); //DONE
                Console.WriteLine("c:\t Display details of an order."); // DONE
                Console.WriteLine("d:\t List customer orders."); // DONE
                Console.WriteLine("e:\t List store location orders."); // DONE
                Console.WriteLine("f:\t Search customer by name."); //DONE
                Console.WriteLine("g:\t Add to location, products, and stock.");
                Console.Write("q:\t To exit the store:\n");
                var userInput = Console.ReadLine();

                if (userInput == "q")
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

                    helperClass.PlaceNewOrder(id);
                }
                else if (userInput == "b")
                {
                    helperClass.AddCustomer(); //ADD CUSTOMER
                }
                else if (userInput == "c") //DISPLAY ORDER DETAILS
                {
                    OrdersRepo ordersRepo = new OrdersRepo();
                    ordersRepo.DisplayOrders();

                    Console.Write("Enter ID of the order that you would like to display: ");
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
                else if (userInput == "e") //DISPLAY location ORDERS
                {
                    LocationRepo locationRepo = new LocationRepo();
                    locationRepo.DisplayLocation();
                    Console.WriteLine("Choose the location ID to display their order history");
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
                else if (userInput == "f")
                {
                    Console.WriteLine("Enter the first name of the customer you would like to search");
                    string name = Console.ReadLine();
                    helperClass.SearCustomerByName(name);
                }
                else if (userInput == "g")
                {
                    //helperClass.AddLocation();
                    //helperClass.AddProducts();
                    //helperClass.AddStock();
                }
            }
        }
    }
}
