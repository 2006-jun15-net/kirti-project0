using System;

namespace CarStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var helperClass = new Helper();

            while (true)
            {
                Console.WriteLine("\nb:\t Add a new customer.");
                Console.Write("q:\t To exit the store:\n");
                var userInput = Console.ReadLine();

                if (userInput == "q")
                {
                    Console.WriteLine("Exiting the store. \nGoodbye, come again!");
                    break;
                }
                else if (userInput == "b")
                {
                    helperClass.AddCustomer();
                }
            }
        }
    }
}
