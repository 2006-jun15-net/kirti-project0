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
        public void AddCustomer()
        {
            CustomerRepo customerRepo = new CustomerRepo();

            Console.Write("Enter first-name of the customer: ");
            string fName = Console.ReadLine();
            fName = CheckString(fName, "first", 26);

            Console.Write("Enter last-name of the customer: ");
            string lName = Console.ReadLine();
            lName = CheckString(lName, "last", 26);

            var newCustomer = new DataAccess.Model.Customer
            {
                FirstName = fName,
                LastName = lName,
            };
            customerRepo.CReposity.Add(newCustomer);
            customerRepo.CReposity.Save();
            Console.WriteLine("You have been added to the database successfully!");

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
                List<Orders> listOrders = ordersRepo.OReposity
                    .GetAll()
                    .Where(e => e.CustomerId == id)
                    .ToList();

                foreach (var order in listOrders)
                {
                    DisplayOrderDetails(order.OrderId);
                }
            }
            else
                Console.WriteLine($"There are no orders for customer ID [{id}]");
        }

        public void LocationOrders (int id)
        {
            OrdersRepo ordersRepo = new OrdersRepo();
            if (ordersRepo.OReposity.GetAll().Any(s => s.LocationId == id))
            {
                List<Orders> listOrders = ordersRepo.OReposity
                    .GetAll()
                    .Where(e => e.LocationId == id)
                    .ToList();

                foreach (var order in listOrders)
                {
                    DisplayOrderDetails(order.OrderId);
                }
            }
            else
                Console.WriteLine($"There are no orders associated with location ID [{id}]");
        }

        public void PlaceNewOrder (int id)
        {
            // declerations
            Decimal orderTotal = 1;
            LocationRepo locationRepo = new LocationRepo();
            ProductRepo productRepo = new ProductRepo();
            StockRepo stockRepo = new StockRepo();
            Product product = new Product();
            OrdersRepo ordersRepo = new OrdersRepo();
            using var context = new Project0Context(GenericRepo<Location>.Options);
            Dictionary<Product, int> productDictionary = new Dictionary<Product, int>();

            // print locations
            Console.WriteLine("Below is a list of all the location you can place your order to: ");
            locationRepo.DisplayLocation(); 

            // select location
            Console.Write("Enter the ID of the Location you would like to place your order to: ");
            string userInput = Console.ReadLine();
            int locationID = Int32.Parse(userInput);
            while (!locationRepo.LReposity.GetAll().Any(l => l.LocationId == locationID))
            {
                Console.WriteLine($"Invalid input \"{locationID}\".");
                Console.Write("Enter the ID of the Location you would like to place your order to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }

            // gather all the stocks at that location for each product
            var curLocation = locationRepo.LReposity.GetWithId(locationID);
            var stock = context.Stock
                .Include(c => c.Product)
                .Where(e => e.LocationId == locationID)
                .ToList();

            while (true)
            {
                //Console.WriteLine("Below is the list of all the products you can add to the location: ");
                //// productRepo.DisplayProducts(); // display products

                // display stocks for each product
                Console.WriteLine("Below is the list of all the inventory of each product at choosen store location");
                foreach (var item in stock)
                    Console.WriteLine($"Product ID [{item.Product.ProductId}], product name is {item.Product.ProductName} and its price is {item.Product.Price}. There are {item.Inventory} items remaining in stock.");

                // product id
                Console.Write("Enter ID of the product that you would like to add: ");
                userInput = Console.ReadLine();
                int productID = Int32.Parse(userInput);
                while (!productRepo.PReposity.GetAll().Any(l => l.ProductId == productID))
                {
                    Console.WriteLine($"Invalid input \"{productID}\".");
                    Console.Write("Enter the ID of the product you would like to add: ");
                    userInput = Console.ReadLine();
                    locationID = Int32.Parse(userInput);
                }

                var getProduct = productRepo.PReposity.GetWithId(productID);

                // product quantity
                Console.WriteLine($"Enter the number of {getProduct.ProductName} do you want to add to your shopping cart? ");
                userInput = Console.ReadLine();
                int numProducts = Int32.Parse(userInput);

                if (numProducts == 0)
                {
                    Console.WriteLine("Exiting the ordering process.");
                    break;
                    
                }

                while (numProducts < 0)
                {
                    Console.WriteLine($"Invalid input \"{numProducts}\".");
                    Console.WriteLine($"How many number of {getProduct.ProductName} do you want to add to your shopping cart? ");
                    userInput = Console.ReadLine();
                    numProducts = Int32.Parse(userInput);
                }

                Stock inventory = stock.First(s => s.Product.ProductId == productID);
                if (inventory.Inventory == 0)
                    Console.WriteLine($"{getProduct.ProductName} is out of stock, try a different location.");

                else if (numProducts > inventory.Inventory)
                    Console.WriteLine($"This location does not have enough stock for {getProduct.ProductName}, try less quantity of this product or check different store.");

                else
                {
                    Console.WriteLine("Your order is being processed!");
                    productDictionary.Add(getProduct, numProducts);
                    inventory.Inventory = inventory.Inventory - numProducts;
                    context.Update(inventory);
                    context.SaveChanges();
                }

                if (productDictionary.Count != 0)
                {
                    //calculate grand total of the order
                    foreach (var orderedProduct in productDictionary.Keys)
                    {
                        orderTotal = (orderTotal + (orderedProduct.Price * productDictionary[orderedProduct]));
                    }

                    DateTime date = DateTime.Now;
                    Console.WriteLine("The total cost of your order is: " + orderTotal);
                    Orders saveOrder = new Orders
                    {
                        LocationId = curLocation.LocationId, // can swap this with locationID
                        CustomerId = id,
                        Price = orderTotal,
                        OrderDate = date
                    };
                    ordersRepo.OReposity.Add(saveOrder);
                    ordersRepo.OReposity.Save();

                    saveOrder = ordersRepo.OReposity.GetAll().First(s => s.OrderDate.Equals(date));

                    foreach (var item in productDictionary.Keys)
                    {
                        var saveProduct = context.Product
                            .Include(c => c.OrderLine)
                            .First(s => s.ProductId == item.ProductId);

                        saveProduct.OrderLine.Add(new OrderLine
                        {
                            Order = saveOrder,
                            TotalPrice = productDictionary[item],
                        });
                    }

                    context.SaveChanges(); //save changes to the orderline

                }
                else
                    Console.WriteLine("There are no products in the order, so the order cannot be placed");

                    Console.WriteLine("Would you like to continue shopping? Enter 'y' for yes and 'n' for no");
                    userInput = Console.ReadLine();
                    if (userInput == "n")
                        break;
            }
            
        }

        public void DisplayOrderDetails (int orderId)
        {
            OrdersRepo ordersRepo = new OrdersRepo();
            if (ordersRepo.OReposity.GetAll().Any(o => o.OrderId == orderId))
            {

                using var context = new Project0Context(GenericRepo<Orders>.Options);

                var order = context.Orders
                    .Include(c => c.OrderLine)
                        .ThenInclude(k => k.Product)
                        .First(l => l.OrderId == orderId);

                Console.WriteLine($"Order [{order.OrderId}] was placed by customer ID [{order.CustomerId}] at location ID [{order.LocationId}] on {order.OrderDate}, where the total was {order.Price} ");

                foreach (var item in order.OrderLine)
                {
                    Console.WriteLine($"For {item.Product.ProductName} the price is {item.Product.Price}");
                }
                context.Dispose();
            }
            else
                Console.WriteLine($"There are no orders associated with ID: {orderId}");

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
                Console.WriteLine($"Invalid input \"{locationID}\".");
                Console.Write("Enter the ID of the location you would like to add products to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }

            Location locationName = locationRepo.LReposity.GetWithId(locationID);
            Console.WriteLine($"You have selected {locationName} location");

            Console.WriteLine("Below is the list of all the products you can add: ");
            productRepo.DisplayProducts();

            Console.Write("Enter ID of the product that you would like to add: ");
            userInput = Console.ReadLine();
            int productID = Int32.Parse(userInput);

            while (!productRepo.PReposity.GetAll().Any(l => l.ProductId == productID))
            {
                Console.WriteLine($"Invalid input \"{productID}\".");
                Console.Write("Enter the ID of the product you would like to add: ");
                userInput = Console.ReadLine();
                productID = Int32.Parse(userInput);
            }

            Console.Write("Enter quantity of the product:");
            userInput = Console.ReadLine();
            int productQuantity = Int32.Parse(userInput);

            while (productQuantity <= 0)
            {
                Console.WriteLine($"Invalid input \"{productQuantity}\".");
                Console.WriteLine("Enter quantity of the product: ");
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
                Console.WriteLine($"Invalid input \"{price}\".");
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

        public void SearCustomerByName(string name)
        {
            CustomerRepo customerRepo = new CustomerRepo();
            if (customerRepo.CReposity.GetAll().Any(c => c.FirstName.Equals(name)))
            {
                Customer customer = customerRepo.CReposity.GetAll().First(c => c.FirstName.Equals(name));
                Console.WriteLine($"The customer that you are looking for, their ID is [{customer.CustomerId}] and their full name is {customer.FirstName} {customer.LastName}");
            }
            else
            {
                Console.WriteLine($"No customers exist by that name: {name}");
            }
        }
    }
}
