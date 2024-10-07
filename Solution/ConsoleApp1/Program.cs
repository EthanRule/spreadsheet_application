// See https://aka.ms/new-console-template for more information
using System.Transactions;

Console.WriteLine("Hello, World!");

string currentExpression = "A1-12-C1";

while (true)
{
    Console.WriteLine($"Menu, current expression={currentExpression}");
    Console.WriteLine("1 = Enter a new expression");
    Console.WriteLine("2 = Set a variable value");
    Console.WriteLine("3 = Evaluate tree");
    Console.WriteLine("4 = Quit");

    string input = "";

    while (input != "1" && input != "2" && input != "3" && input != "4")
    {
        input = Console.ReadLine();
    }

    switch (input)
    {
        case "1":

            break;
        case "2":

            break;
        case "3":

            break;
        case "4":
            Console.WriteLine("Done");
            Environment.Exit(0);
            break;
    }
}