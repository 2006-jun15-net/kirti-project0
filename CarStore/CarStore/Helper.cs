using System;
using System.Linq;
using CarStore.DataAccess.Model;
using CarStore.Library.Repository;

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

        public void CustomerOrders ()
        {

        }

        public void LocationOrders ()
        {

        }

        public void PlaceNewOrder (Customer customer)
        {
            LocationRepo locationRepo = new LocationRepo();

            Console.WriteLine("Below is a list of all the stores you can place your order to: ");

            locationRepo.DisplayLocation();

            Console.Write("Enter the ID of the Location you would like to place your order to: ");
            string userInput = Console.ReadLine();
            int locationID = Int32.Parse(userInput);

            while (locationRepo.LReposity.GetAll().Any(l => l.LocationId != locationID))
            {
                Console.WriteLine($"Invalid input, cannot be negative \"{locationID}\".");
                Console.Write("Enter the ID of the Location you would like to place your order to: ");
                userInput = Console.ReadLine();
                locationID = Int32.Parse(userInput);
            }
        }

        public void AddLocation()
        {

            LocationRepo locationRepo = new LocationRepo();

            Console.WriteLine("Enter new store name: ");
            string name = Console.ReadLine();
            name = CheckString(name, "store", 255);

            Location newStore = new Location
            {
                LocationName = name
            };
            locationRepo.LReposity.Add(newStore);
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
