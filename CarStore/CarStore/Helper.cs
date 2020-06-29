using System;
using System.Collections.Generic;
using System.Linq;
using CarStore.DataAccess.Model;
using CarStore.Library.Repository;
using Microsoft.EntityFrameworkCore;

namespace CarStore
{
    public class Helper
    {
        public Customer AddCustomer ()
        {
            CustomerRepo customerRepo = new CustomerRepo();

            Console.WriteLine("Are you a new customer? enter 'y' for yes and 'n' for no.");
            var userInput = Console.ReadLine();

            if (userInput == "y")
            {
                
                Console.Write("Enter first-name of the customer: ");
                string fName = Console.ReadLine();
                fName = CheckString(fName, "first", 26);

                Console.Write("Enter last-name of the customer: ");
                string lName = Console.ReadLine();
                lName = CheckString(lName, "last", 26);

                Console.Write("Enter user-name of the customer: ");
                string userName = Console.ReadLine();
                userName = CheckString(userName, "user", 26);

                var newCustomer = new DataAccess.Model.Customer
                {
                    FirstName = fName,
                    LastName = lName,
                    UserName = userName
                };
                customerRepo.CReposity.Add(newCustomer);
                customerRepo.CReposity.Save();
                Console.WriteLine("You have been added to the database successfully!");

                customerRepo.DisplayCustomer();

                Console.Write("Enter your ID: ");
                string input = Console.ReadLine();
                int id = Int32.Parse(input);

                while (id <= 0)
                {
                    Console.WriteLine($"Invalid input, cannot be negative \"{id}\".");
                    Console.Write("Enter your ID: ");
                    input = Console.ReadLine();
                    id = Int32.Parse(input);
                }

                return customerRepo.CReposity.GetWithId(id);

            }
            else if (userInput == "n")
            {
                if (customerRepo.CReposity.GetAll().FirstOrDefault() == null)
                    Console.WriteLine("There are no customers currently.");
                else
                {
                    customerRepo.DisplayCustomer();

                    Console.Write("Enter your ID: ");
                    string input = Console.ReadLine();
                    int id = Int32.Parse(input);

                    while (id <= 0)
                    {
                        Console.WriteLine($"Invalid input \"{id}\".");
                        Console.Write("Enter your ID: ");
                        input = Console.ReadLine();
                        id = Int32.Parse(input);
                    }

                    return customerRepo.CReposity.GetWithId(id);
                }
            }
            else
            {
                Console.WriteLine($"Invalid input \"{userInput}\".");
            }
            return null;
        }

        public string CheckString (string name, string print, int size)
        {
            while (String.IsNullOrWhiteSpace(name) || name.Length > size)
            {
                Console.WriteLine($"Invalid input \"{name}\" try again!.");
                Console.Write($"Enter {print}-name: ");
                name = Console.ReadLine();
            }
            return name;
        }

        public void CustomerOrders (int id)
        {

            OrdersRepo ordersRepo = new OrdersRepo();
            if (ordersRepo.OReposity.GetAll().Any(s => s.CustomerId == id))
            {
                Console.WriteLine(id);

                List<Orders> listOrders = ordersRepo.OReposity
                    .GetAll().Where(e => e.CustomerId == id).ToList();

                foreach (var order in listOrders)
                {
                    ordersRepo.DisplayOrdersAtIndex(order.OrderId);
                }
            }
            else
                Console.WriteLine($"There are no orders for customer ID [{id}]");
        }

        public void LocationOrders (int id)
        {

        }

        public void PlaceNewOrder (Customer customer)
        {
            Decimal orderTotal = 1;
            LocationRepo locationRepo = new LocationRepo();
            ProductRepo productRepo = new ProductRepo();
            StockRepo stockRepo = new StockRepo();
            Product product = new Product();
            OrdersRepo ordersRepo = new OrdersRepo();
            using var context = new Project0Context(GenericRepo<Location>.Options);
            Dictionary<Product, int> productDictionary = new Dictionary<Product, int>();

            Console.WriteLine("Below is a list of all the location you can place your order to: ");

            locationRepo.DisplayLocation(); //print locations

            // prompt user to enter location id
            Console.Write("Enter the ID of the Location you would like to place your order to: ");
            string userInput = Console.ReadLine();
            int locationID = Int32.Parse(userInput);

            while (!locationRepo.LReposity.GetAll().Any(l => l.LocationId == locationID))
            {
                Console.WriteLine($"Invalid input, cannot be negative \"{locationID}\".");
                Console.Write("Enter the ID of the Location you would like to place your order to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }

            if (locationRepo.LReposity.GetAll().Any(s => s.LocationId == locationID))
            {
                // gather all the stocks at that location for each product
                var stock = context.Stock
                    .Include(c => c.Product)
                    .Where(e => e.LocationId == locationID)
                    .ToList();

                // display stocks for each product
                Console.WriteLine("Below is the list of all the stock choosen location");
                foreach (var item in stock)
                    Console.WriteLine($"Product ID [{item.Product.ProductId}], product name is {item.Product.ProductName} and its price is {item.Product.Price}.");

                while (true)
                {
                    Console.WriteLine("Below is the list of all the products you can add to the location: ");
                    productRepo.DisplayProducts(); // display products

                    // prompt user to select the product id
                    Console.Write("Enter ID of the product that you would like to add to the selected location: ");
                    userInput = Console.ReadLine();
                    int productID = Int32.Parse(userInput);

                    while (!productRepo.PReposity.GetAll().Any(l => l.ProductId == productID))
                    {
                        Console.WriteLine($"Invalid input \"{productID}\".");
                        Console.Write("Enter the ID of the product you would like to add to the selected location: ");
                        userInput = Console.ReadLine();
                        locationID = Int32.Parse(userInput);
                    }


                    var getProduct = productRepo.PReposity.GetWithId(productID);

                    Console.WriteLine($"Enter the number of {getProduct.ProductName} do you want to add to your shopping cart? ");
                    userInput = Console.ReadLine();
                    int numProducts = Int32.Parse(userInput);

                    while (numProducts <= 0)
                    {
                        Console.WriteLine($"Invalid input \"{numProducts}\".");
                        Console.WriteLine($"Enter the number of {getProduct.ProductName} do you want to add to your shopping cart? ");
                        userInput = Console.ReadLine();
                        numProducts = Int32.Parse(userInput);
                    }

                    Stock inventory = stock.First(s => s.Product.ProductId == productID);

                    if (inventory.Inventory == 0)
                        Console.WriteLine($"{product.ProductName} is out of stock, try a different location.");

                    else if (numProducts > inventory.Inventory)
                        Console.WriteLine($"Does not have enough stock for {product.ProductName}, try less quantity of this product.");

                    else
                    {
                        Console.WriteLine("Your order has been placed!");
                        productDictionary.Add(product, numProducts);
                        inventory.Inventory = inventory.Inventory - numProducts;
                        context.Update(inventory);
                        context.SaveChanges();
                    }


                    if (productDictionary.Count != 0)
                    {
                        //calculate grand total of the order
                        foreach (var orderedProduct in productDictionary.Keys)
                            orderTotal = (orderTotal + (orderedProduct.Price * productDictionary[orderedProduct]));

                        DateTime date = DateTime.Now;
                        Console.WriteLine("The total cost of your order is: " + orderTotal);
                        Orders saveOrder = new Orders
                        {
                            LocationId = locationID,
                            CustomerId = customer.CustomerId,
                            Price = orderTotal,
                            OrderDate = date
                        };
                        ordersRepo.OReposity.Add(saveOrder);
                        ordersRepo.OReposity.Save();

                    }
                    else
                        Console.WriteLine("There are no products in the order, so the order cannot be placed");

                        Console.WriteLine("Would you like to continue shopping? Enter 'y' for yes and 'n' for no");
                        userInput = Console.ReadLine();
                        if (userInput == "n")
                            break;
                }
            }
        }

        public void DisplayOrderDetails ()
        {
            OrdersRepo ordersRepo = new OrdersRepo();
            ordersRepo.DisplayOrders();

            Console.Write("Enter ID of the order that you would like to display: ");
            string input = Console.ReadLine();
            int orderId = Int32.Parse(input);

            while (! ordersRepo.OReposity.GetAll().Any(s => s.OrderId == orderId))
            {
                Console.WriteLine($"Invalid input \"{orderId}\".");
                Console.Write("Enter ID of the order that you would like to display: ");
                input = Console.ReadLine();
                orderId = Int32.Parse(input);
            }

            using var context = new Project0Context(GenericRepo<Product>.Options);
            var order = context.Orders
                .Include(c => c.Location)
                .ThenInclude(s => s.Customer)
                    .First(l => l.OrderId == orderId);

            Console.WriteLine($"Order ID [{order.OrderId}] was placed by customer ID [{order.CustomerId}] at location ID [{order.LocationId}] on {order.OrderDate}, where the total was {order.Price} ");
        }

        public void AddStock ()
        {
            LocationRepo locationRepo = new LocationRepo();
            ProductRepo productRepo = new ProductRepo();
            StockRepo stockRepo = new StockRepo();

            Console.WriteLine("Below is a list of all the location you can add products to: ");
            locationRepo.DisplayLocation();

            Console.Write("Enter ID of the location you would like to add products to: ");
            string userInput = Console.ReadLine();
            int locationID = Int32.Parse(userInput);

            while (!locationRepo.LReposity.GetAll().Any(l => l.LocationId == locationID))
            {
                Console.WriteLine($"Invalid input, cannot be negative \"{locationID}\".");
                Console.Write("Enter the ID of the location you would like to add products to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }

            Location locationName = locationRepo.LReposity.GetWithId(locationID);
            Console.WriteLine($"you have selected {locationName} location");

            Console.WriteLine("Below is the list of all the products you can add to the location: ");
            productRepo.DisplayProducts();

            Console.Write("Enter ID of the product that you would like to add to the selected location: ");
            userInput = Console.ReadLine();
            int productID = Int32.Parse(userInput);

            while (!productRepo.PReposity.GetAll().Any(l => l.ProductId == productID))
            {
                Console.WriteLine($"Invalid input, cannot be negative \"{productID}\".");
                Console.Write("Enter the ID of the product you would like to add to the selected location: ");
                userInput = Console.ReadLine();
                productID = Int32.Parse(userInput);
            }

            Console.Write("Enter quantity of the product:");
            userInput = Console.ReadLine();
            int productQuantity = Int32.Parse(userInput);

            while (productQuantity <= 0)
            {
                Console.WriteLine($"Invalid input, cannot be negative \"{productQuantity}\".");
                Console.Write("Enter quantity of the product:");
                userInput = Console.ReadLine();
                productQuantity = Int32.Parse(userInput);
            }

          
            using var context = new Project0Context(GenericRepo<Product>.Options);

            var product = context.Product
                .Include(s => s.Stock)
                    .First(e => e.ProductId == productID);

            product.Stock.Add(new Stock { Location = locationName, Inventory = productQuantity });
            context.SaveChanges();
            stockRepo.DisplayStock();
        }

        public void AddLocation()
        {

            LocationRepo locationRepo = new LocationRepo();

            Console.WriteLine("Enter new location name: ");
            string name = Console.ReadLine();
            name = CheckString(name, "location", 255);

            Location newLocation = new Location
            {
                LocationName = name
            };
            locationRepo.LReposity.Add(newLocation);
            locationRepo.LReposity.Save();

            Console.WriteLine("Location was successfully added!");
            locationRepo.DisplayLocation();
        }

        public void AddProducts()
        {
            ProductRepo productRepo = new ProductRepo();

            Console.WriteLine("Enter a car name");
            string name = Console.ReadLine();
            name = CheckString(name, "car", 255);

            Console.WriteLine("Enter a price for the car:");
            string input = Console.ReadLine();
            decimal price = Decimal.Parse(input);

            while (price <= 0)
            {
                Console.WriteLine($"Invalid input, cannot be negative \"{price}\".");
                Console.WriteLine("Enter a price for the car:");
                input = Console.ReadLine();
                price = Decimal.Parse(input);
            }

            Product newProduct = new Product
            {
                ProductName = name,
                Price = price
            };

            productRepo.PReposity.Add(newProduct);
            productRepo.PReposity.Save();

            Console.WriteLine("Product was successfully added!");
            productRepo.DisplayProducts();

        }
    }
}
