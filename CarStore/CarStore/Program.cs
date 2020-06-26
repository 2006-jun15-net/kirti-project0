using System;
using System.Collections.Generic;

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

                //Console.WriteLine("c:\t Search customer\t.");
                //Console.WriteLine("d:\t Display order\t.");
                //Console.WriteLine("e:\t Customer order history\t.");
                //Console.WriteLine("f:\t Location order history\t.");
                //Console.WriteLine("s:\t Save data to disk.");

                Console.WriteLine("l:\t Load data from disk.");
                Console.Write("q:\t to exit the store:\n");

                var input = Console.ReadLine();

                // if user enterd 'q' then quit the program with exit code 0
                if (input == "q")
                {
                    Console.WriteLine("Exiting car delearship store.\nGoodbye, come again!");
                    break;
                }
                else if (input == "a")
                {
                    Console.WriteLine("Hello, you are placing a new order");

                    while (true)
                    {
                        Console.WriteLine("\n1:\t Find the car by name.");
                        Console.WriteLine("\n2:\t Find by brand name.");

                        input = Console.ReadLine();
                        //parse this string to int
                        int option = Int32.Parse(input); 

                        if(option == 1)
                        {
                            Console.WriteLine("Please enter the car name");
                            input = Console.ReadLine();


                        }
                        else if (option == 2)
                        {
                            Console.WriteLine("Please enter the brand name");
                            input = Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Invalid input \"{input}\".");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
            }
        }

        public static void NewOrder()
        {
            
        }
    }
}
