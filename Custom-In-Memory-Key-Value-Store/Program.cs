using System;
using System.Collections.Generic;

namespace Custom_In_Memory_Key_Value_Store
{
    internal class Program
    {
        static Dictionary<string, string> store = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            Console.WriteLine("==== Mini Redis Server Started ====");
            Console.WriteLine("Commands: SET [key] [value] | GET [key] | DEL [key] | EXIT");

            // REPL
            while (true)
            {
                Console.Write("127.0.0.1:6379> ");
                string input = Console.ReadLine();

                string[] parts = input.Split(' ', 3);
                string command = parts[0].ToUpper();


                if(command == "EXIT")
                {
                    Console.WriteLine("Shutting down server....");
                    break;
                }

                else if(command == "SET" && parts.Length > 3)
                {
                    string key = parts[1];
                    string value = parts[2];

                    // storing the value
                    store[key] = value;
                    Console.WriteLine("OK");
                }
            }
        }
    }
}
