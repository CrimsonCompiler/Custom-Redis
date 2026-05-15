using System;
using System.Collections.Generic;
using System.IO;

namespace Custom_In_Memory_Key_Value_Store
{
    internal class Program
    {
        static Dictionary<string, string> store = new Dictionary<string, string>();
        // Log File
        const string AofFilePath = "custom_redis_storage.aof";

        static void Main(string[] args)
        {
            Console.WriteLine("==== Mini Redis Server Started ====");
            Console.WriteLine("Commands: SET [key] [value] | GET [key] | DEL [key] | EXIT");

            // Loading Existing data from hard-disk drive
            LoadDataFromDisk();

            // REPL
            while (true)
            {
                Console.Write("127.0.0.1:6379> ");
                string input = Console.ReadLine();

                string[] parts = input.Split(' ', 3);
                string command = parts[0].ToUpper();

                // ignore the fake input (inc. null, whitespace)
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }


                if(command == "EXIT")
                {
                    Console.WriteLine("Shutting down server....");
                    break;
                }

                else if(command == "SET" && parts.Length >= 3)
                {
                    string key = parts[1];
                    string value = parts[2];

                    // storing the value in RAM
                    store[key] = value;

                    // storing the value in Log
                    AppendToLog(input);

                    Console.WriteLine("OK");
                }

                else if(command == "GET" && parts.Length >= 2)
                {
                    string key = parts[1];

                    if (store.ContainsKey(key))
                    {
                        Console.WriteLine($"\"{store[key]}\"");
                    }
                    else
                    {
                        Console.WriteLine("nil");
                    }
                }

                else if(command == "DEL" && parts.Length >= 2)
                {
                    string key = parts[1];

                    if (store.Remove(key))
                    {
                        // Storing the delete info too
                        AppendToLog(input);
                        Console.WriteLine("(integer) 1");
                    }
                    else
                    {
                        Console.WriteLine("(integer) 0");
                    }
                }

                else
                {
                    Console.WriteLine("(error) ERR unknown command or wrong number of arguments");
                }
            }
        }

        static void AppendToLog(string command)
        {
            using (StreamWriter sw = new StreamWriter(AofFilePath, true))
            {
                sw.WriteLine(command);
            }
        }


        static void LoadDataFromDisk()
        {
            if (!File.Exists(AofFilePath)) return; // If the log isn't created yet

            Console.WriteLine("Loading data from disk...");

            string[] logLines = File.ReadAllLines(AofFilePath);

            int recoveredCount = 0;

            foreach(string line in logLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(' ', 3);
                string command = parts[0].ToUpper();

                if(command == "SET" && parts.Length >= 3)
                {
                    store[parts[1]] = parts[2];
                    recoveredCount++;
                }

                else if(command == "DEL" && parts.Length >= 2)
                {
                    store.Remove(parts[1]);
                    recoveredCount++;
                }
            }
            Console.WriteLine($"Recovered {recoveredCount} operations from AOF log.\n");
        }
    }
}
