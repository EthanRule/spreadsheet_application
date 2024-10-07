namespace ConsoleApp1
{
    using SpreadsheetEngine.ExpressionTree;
    internal static class Program
    {
        private static void Main()
        {
            string currentExpression = "A1-12-C1";

            ExpressionTree tree = new ExpressionTree(currentExpression);

            while (true)
            {
                Console.WriteLine($"Menu (current expression=\"{currentExpression}\")");
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
                        Console.Write("Enter a new expression: ");
                        string expression = Console.ReadLine();
                        
                        tree = new ExpressionTree(expression);

                        break;
                    case "2":
                        Console.Write("Enter a variable name: ");
                        string variableName = Console.ReadLine();
                        Console.Write("Enter a variable value: ");
                        string variableValue = Console.ReadLine();
                        tree.SetVariable(variableName, double.Parse(variableValue));

                        break;
                    case "3":
                        Console.WriteLine("Evaluating tree...");
                        tree.Evaluate();

                        break;
                    case "4":
                        Console.WriteLine("Done");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}

