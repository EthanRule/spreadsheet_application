//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleApp1
{
    using SpreadsheetEngine.ExpressionTree;

    /// <summary>
    /// ExpressionTree Program class.
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            string currentExpression = string.Empty;
            ExpressionTree tree = new ExpressionTree(currentExpression);

            while (true)
            {
                Console.WriteLine($"Menu (current expression=\"{currentExpression}\")");
                Console.WriteLine("1 = Enter a new expression");
                Console.WriteLine("2 = Set a variable value");
                Console.WriteLine("3 = Evaluate tree");
                Console.WriteLine("4 = Quit");

                string input = string.Empty;

                while (input != "1" && input != "2" && input != "3" && input != "4")
                {
                    input = Console.ReadLine();
                }

                switch (input)
                {
                    case "1": // Enter a new expression
                        Console.Write("Enter a new expression: ");
                        string expression = Console.ReadLine();
                        tree.SetExpression(expression);
                        currentExpression = tree.GetExpression();
                        break;
                    case "2": // Set a variable value
                        Console.Write("Enter a variable name: ");
                        string variableName = Console.ReadLine();
                        Console.Write("Enter a variable value: ");
                        string variableValue = Console.ReadLine();
                        tree.SetVariable(variableName, double.Parse(variableValue));

                        break;
                    case "3": // Evaluate tree
                        Console.WriteLine(tree.Evaluate());

                        break;
                    case "4": // Quit
                        Console.WriteLine("Done");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}