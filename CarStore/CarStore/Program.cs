using System;

namespace CarStore
{
    class Program
    {
        static void Main(string[] args)
        {
            // welcome the user to store
            Console.WriteLine("Welcome to the car delearship store brought to you by Revature .NET project 0");

            while (true)
            {

                // display options for user to enter
                Console.WriteLine("\na:\t Place order.");
                Console.WriteLine("c:\t Search customer\t.");
                Console.WriteLine("d:\t Display order\t.");
                Console.WriteLine("e:\t Customer order history\t.");
                Console.WriteLine("f:\t Location order history\t.");
                Console.WriteLine("s:\t Save data to disk.");
                Console.WriteLine("l:\t Load data from disk.");
                Console.Write("q:\t to exit the store:\n");

                var userInput = Console.ReadLine();

                // if user enterd 'q' then quit the program with exit code 0
                if (userInput == "q")
                {
                    Console.WriteLine("Exiting car delearship store.\nGoodbye, come again!");
                    break;
                }
                else if (userInput == "a")
                {
                    Console.WriteLine("Hello from r");
                }
                else
                {
                    Console.WriteLine($"Invalid input \"{userInput}\".");
                }
            }
        }
    }
}
