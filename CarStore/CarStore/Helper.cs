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
        /// <summary>
        /// Add customers to database
        /// </summary>
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
            Console.WriteLine("You have been added to the database successfully! \n ");

        }

        /// <summary>
        /// check if the string is null or has just plane white space with no words
        /// </summary>
        /// <param name="name"></param>
        /// <param name="print"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string CheckString (string name, string print, int size)
        {
            while (String.IsNullOrWhiteSpace(name) || name.Length > size)
            {
                Console.WriteLine($"\n Invalid input \"{name}\" try again!.");
                Console.Write($"\n Enter {print}-name: ");
                name = Console.ReadLine();
            }
            return name;
        }

        /// <summary>
        /// customers order details
        /// </summary>
        /// <param name="id"></param>
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
                    Console.WriteLine();
                }
            }
            else
                Console.WriteLine($"There are no orders for customer ID [{id}]");
        }

        /// <summary>
        /// location order details
        /// </summary>
        /// <param name="id"></param>
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
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine($"There are no orders associated with location ID [{id}]");
        }

        /// <summary>
        /// place order
        /// </summary>
        /// <param name="id">customer id</param>
        public void PlaceNewOrder (int id)
        {
            // declerations
            LocationRepo locationRepo = new LocationRepo();
            ProductRepo productRepo = new ProductRepo();
            StockRepo stockRepo = new StockRepo();
            Product product = new Product();
            OrdersRepo ordersRepo = new OrdersRepo();
            using var context = new Project0Context(GenericRepo<Location>.Options);
            Dictionary<Product, int> productDictionary = new Dictionary<Product, int>();
            string userInput;
            int productID;
            int locationID;
            int numProducts;

            locationID = ChooseLocation(locationRepo);

            // get all the stocks at that location for each product
            var curLocation = locationRepo.LReposity.GetById(locationID);
            List<Stock> stock = context.Stock
                .Include(c => c.Product)
                .Where(e => e.LocationId == locationID)
                .ToList();

            DateTime date = DateTime.Now;

            while (true)
            {
                // display stocks for each product
                LocationStock(stock);

                // product id
                productID = ChooseProduct(productRepo);

                //check if this product was already purchased in the same order, if it was then don't allow
                if (productDictionary != null)
                {
                    while (productDictionary.Keys.Any(s => s.ProductId == productID))
                    {
                        Console.WriteLine("\nYou have already added this tem to cart, choose another one: ");
                        productID = ChooseProduct(productRepo);

                    }
                }

                // product quantity
                numProducts = ProductQuantity(productRepo, productID);

                //verify product availability dnd quantity, add it product disctionary along witht he product ordered and its quantity
                var getProduct = productRepo.PReposity.GetById(productID);
                Stock inventory = stock.First(s => s.Product.ProductId == productID);
                if (inventory.Inventory == 0)
                    Console.WriteLine($"{getProduct.ProductName} is out of stock, try a different location.");

                else if (numProducts > inventory.Inventory)
                    Console.WriteLine($"\nThis location does not have enough stock for {getProduct.ProductName}, try less quantity of this product or check different store.");

                else
                {
                    Console.WriteLine("\nYour order is being processed!");
                    productDictionary.Add(getProduct, numProducts);
                    inventory.Inventory = inventory.Inventory - numProducts;
                    context.Update(inventory);
                    context.SaveChanges();
                }

                // process order, calculate total, and save the order
                SaveOrder(productDictionary, ordersRepo, id, curLocation, date);

                Console.WriteLine("\nWould you like to continue shopping? Enter 'y' for yes and 'n' for no");
                userInput = Console.ReadLine();
                if (userInput == "n")
                    break;
            }
            
        }

        /// <summary>
        /// print details of an order at a given order id
        /// </summary>
        /// <param name="orderId"></param>
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

                Console.WriteLine($"\nOrder [{order.OrderId}] on {order.OrderDate} by customer ID [{order.CustomerId}] whose grand total was {order.Price}");

                foreach (var item in order.OrderLine)
                {
                    //change the TotalPrice to quantity and make it type int
                    Console.WriteLine($"\n{item.Quantity} {item.Product.ProductName} for the price of {item.Product.Price}");
                }
                Console.WriteLine();
                context.Dispose();
            }
            else
                Console.WriteLine($"There are no orders associated with ID: {orderId}");

        }

        /// <summary>
        /// add products to stock
        /// </summary>
        public void AddStock ()
        {
            LocationRepo locationRepo = new LocationRepo();
            ProductRepo productRepo = new ProductRepo();
            StockRepo stockRepo = new StockRepo();

            Console.WriteLine("Below is a list of all the location you can add products to: \n");
            locationRepo.DisplayLocation();

            Console.WriteLine("\nEnter ID of the location you would like to add products to: ");
            string userInput = Console.ReadLine();
            int locationID = Int32.Parse(userInput);

            while (!locationRepo.LReposity.GetAll().Any(l => l.LocationId == locationID))
            {
                Console.WriteLine($"Invalid input \"{locationID}\".");
                Console.Write("Enter the ID of the location you would like to add products to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }

            Location locationName = locationRepo.LReposity.GetById(locationID);
            Console.WriteLine($"\nYou have selected {locationName.LocationName} location");

            Console.WriteLine("\nBelow is the list of all the products you can add: \n");
            productRepo.DisplayProducts();

            Console.Write("\nEnter ID of the product that you would like to add: ");
            userInput = Console.ReadLine();
            int productID = Int32.Parse(userInput);

            while (!productRepo.PReposity.GetAll().Any(l => l.ProductId == productID))
            {
                Console.WriteLine($"Invalid input \"{productID}\".");
                Console.Write("Enter the ID of the product you would like to add: ");
                userInput = Console.ReadLine();
                productID = Int32.Parse(userInput);
            }

            Console.Write("\nEnter quantity of the product:");
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
            //stockRepo.DisplayStock();
        }

        /// <summary>
        /// select the location (place new order, helper functions)
        /// </summary>
        /// <param name="locationRepo"></param>
        /// <returns></returns>
        public int ChooseLocation (LocationRepo locationRepo)
        {
            // print locations
            Console.WriteLine("\nBelow is a list of all the location you can place your order to: \n");
            locationRepo.DisplayLocation();

            // select location
            Console.Write("\nEnter the ID of the Location you would like to place your order to: ");
            string userInput = Console.ReadLine();
            int locationID = Int32.Parse(userInput);
            while (!locationRepo.LReposity.GetAll().Any(l => l.LocationId == locationID))
            {
                Console.WriteLine($"Invalid input \"{locationID}\".");
                Console.Write("Enter the ID of the Location you would like to place your order to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }
            return locationID;
        }

        /// <summary>
        /// select the product (place new order, helper functions)
        /// </summary>
        /// <param name="productRepo"></param>
        /// <returns></returns>
        public int ChooseProduct (ProductRepo productRepo)
        {
            Console.Write("\nEnter ID of the product that you would like to add: ");
            string userInput = Console.ReadLine();
            int productID = Int32.Parse(userInput);
            while (!productRepo.PReposity.GetAll().Any(l => l.ProductId == productID))
            {
                Console.WriteLine($"Invalid input \"{productID}\".");
                Console.Write("Enter the ID of the product you would like to add: ");
                userInput = Console.ReadLine();
                productID = Int32.Parse(userInput);
            }
            return productID;
        }

        /// <summary>
        /// print stock of a selected location (place order, helper functions)
        /// </summary>
        /// <param name="stock"></param>
        public void LocationStock (List<Stock> stock)
        {
            Console.WriteLine("\nBelow is the list of all the inventory of each product at choosen store location");
            foreach (var item in stock)
                Console.WriteLine($"Product ID [{item.Product.ProductId}], product name is {item.Product.ProductName} and its price is {item.Product.Price}. There are {item.Inventory} items remaining in stock.");
            Console.WriteLine();
        }

        /// <summary>
        /// enter quantity of a certain product that you want (place order, helper functions)
        /// </summary>
        /// <param name="productRepo"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public int ProductQuantity (ProductRepo productRepo, int productID)
        {
            var getProduct = productRepo.PReposity.GetById(productID);
            Console.WriteLine($"\nEnter the number of {getProduct.ProductName} do you want to add to your shopping cart? ");
            string userInput = Console.ReadLine();
            int numProducts = Int32.Parse(userInput);

            while (numProducts <= 0)
            {
                Console.WriteLine($"Invalid input \"{numProducts}\".");
                Console.WriteLine($"How many number of {getProduct.ProductName} do you want to add to your shopping cart? ");
                userInput = Console.ReadLine();
                numProducts = Int32.Parse(userInput);
            }
            return numProducts;
        }

        /// <summary>
        /// save the order (place order, helper functions)
        /// </summary>
        /// <param name="productDictionary"></param>
        /// <param name="ordersRepo"></param>
        /// <param name="customerId"></param>
        /// <param name="curLocation"></param>
        /// <param name="date"></param>
        public void SaveOrder (Dictionary<Product, int> productDictionary, OrdersRepo ordersRepo, int customerId, Location curLocation, DateTime date)
        {
            using var context = new Project0Context(GenericRepo<Location>.Options);

            decimal orderTotal = 0;
            if (productDictionary.Count != 0)
            {
                //calculate grand total of the order
                foreach (var orderedProduct in productDictionary.Keys)
                {
                    orderTotal = (orderTotal + (orderedProduct.Price * productDictionary[orderedProduct]));
                }

                Console.WriteLine("\nThe total cost of your order is: " + orderTotal);
                Orders saveOrder = new Orders
                {

                    LocationId = curLocation.LocationId,
                    CustomerId = customerId,
                    Price = orderTotal,
                    OrderDate = date
                };
                ordersRepo.OReposity.Add(saveOrder);

                foreach (var item in productDictionary.Keys)
                {
                    var saveProduct = context.Product
                        .Include(c => c.OrderLine)
                        .First(s => s.ProductId == item.ProductId);

                    saveProduct.OrderLine.Add(new OrderLine
                    {
                        Order = saveOrder,
                        Quantity = productDictionary[item],
                    });
                }

                context.SaveChanges(); //save changes to the orderline

            }
            else
                Console.WriteLine("\nThere are no products in the order, so the order cannot be placed");
        }

        /// <summary>
        /// add location
        /// </summary>
        public void AddLocation()
        {

            LocationRepo locationRepo = new LocationRepo();

            Console.WriteLine("\nEnter new location name: ");
            string name = Console.ReadLine();
            name = CheckString(name, "location", 255);

            Location newLocation = new Location
            {
                LocationName = name
            };
            locationRepo.LReposity.Add(newLocation);
            locationRepo.LReposity.Save();

            Console.WriteLine("Location was successfully added!");
            //locationRepo.DisplayLocation();
        }

        /// <summary>
        /// add products
        /// </summary>
        public void AddProducts()
        {
            ProductRepo productRepo = new ProductRepo();

            Console.WriteLine("\nEnter a car name");
            string name = Console.ReadLine();
            name = CheckString(name, "car", 255);

            Console.WriteLine("\nEnter a price for the car:");
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
            //productRepo.DisplayProducts();

        }

        /// <summary>
        /// search customer by name
        /// </summary>
        /// <param name="name"></param>
        public void SearCustomerByName(string name)
        {
            CustomerRepo customerRepo = new CustomerRepo();
            if (customerRepo.CReposity.GetAll().Any(c => c.FirstName.Equals(name)))
            {
                Customer customer = customerRepo.CReposity.GetAll().First(c => c.FirstName.Equals(name));
                Console.WriteLine($"\nThe customer that you are looking for, their ID is [{customer.CustomerId}] and their full name is {customer.FirstName} {customer.LastName}");
            }
            else
            {
                Console.WriteLine($"\nNo customers exist by that name: {name}");
            }
        }
    }
}