using System;
using System.Linq;
using CarStore.Library.Model;
using CarStore.Library.Repository;

namespace CarStore
{
    public class Helper
    {
        public void AddCustomer ()
        {
            CustomerRepo customerRepo = new CustomerRepo();

            Console.WriteLine("Are you a new customer? enter 'y' for yes and 'n' for no.");
            var userInput = Console.ReadLine();

            if (userInput == "n")
            {
                bool status = true;
                while (status)
                {
                    Console.Write("Enter first name of the customer: ");
                    string fName = Console.ReadLine();
                    if (fName == null)
                    {
                        Console.WriteLine($"Invalid input \"{fName}\".");
                    }

                    Console.Write("Enter last name of the customer: ");
                    string lName = Console.ReadLine();
                    if (lName == null)
                    {
                        Console.WriteLine($"Invalid input \"{lName}\".");
                    }

                    Customer newCustomer = new Customer
                    {
                        FirstName = fName,
                        LastName = lName,
                    };
                    customerRepo.CReposity.Add(newCustomer);
                    customerRepo.CReposity.Save();
                    Console.WriteLine("You have been added to the database successfully!");
                    status = false;
                }

            }
            else if (userInput == "y")
            {
                if (customerRepo.CReposity.GetAll().FirstOrDefault() == null)
                    Console.WriteLine("There are no customers currently.");
                else
                {
                    customerRepo.CReposity.GetAll();
                }
            }
            else
            {
                Console.WriteLine($"Invalid input \"{userInput}\".");
            }
        }
    }
}
