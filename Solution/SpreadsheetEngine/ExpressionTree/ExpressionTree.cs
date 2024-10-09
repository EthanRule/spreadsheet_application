using System;
using System.Collections.Generic;
using System.Linq;
namespace SpreadsheetEngine.ExpressionTree
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public class ExpressionTree
    {
        private Node root;
        private Dictionary<string, double> variables;
        private string expression;

        public ExpressionTree(string expression)
        {
            this.expression = expression;
            this.variables = new Dictionary<string, double>(); // init variables dict
            this.root = this.ParseExpression(expression);
        }

        public double GetVariable(string key)
        {
            return this.variables[key];
        }

        public void SetExpression(string expression)
        {
            this.expression = expression;
            this.root = this.ParseExpression(expression);
        }

        private Node ParseExpression(string expression)
        {
            if (expression == "")
            {
                return null;
            }

            StringBuilder item = new StringBuilder();
            Node leftNode = null;
            char currentOperator = '\0'; // ensure parenthesis are never an operator. but still needs to be handled to group expressions. add '^'?

            for (int i = 0; i < expression.Length; i++)
            {
                // check for variable
                char currentIndex = expression[i];

                if (char.IsLetter(currentIndex))
                {
                    item.Append(currentIndex);

                    // find the values after the letter
                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        currentIndex = expression[j];
                        if (char.IsLetterOrDigit(currentIndex)) // allows for multi letter variables.
                        {
                            item.Append(expression[j]);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    string variableName = item.ToString();
                    Console.WriteLine($"Parsed variable: {variableName}");

                    Node variable = new VariableNode(item.ToString(), this.variables);
                    if (leftNode == null)
                    {
                        leftNode = variable;
                    }
                    else
                    {
                        Console.WriteLine($"Adding binary operator node: {currentOperator} between {leftNode.GetType()} and {variable.GetType()}");
                        leftNode = new BinaryOperatorNode(currentOperator, leftNode, variable);
                    }

                    item.Clear();
                }
                else if (char.IsDigit(currentIndex))
                {
                    item.Append(expression[i]);

                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        currentIndex = expression[j];
                        if (char.IsDigit(currentIndex))
                        {
                            item.Append(expression[j]);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    double number = double.Parse(item.ToString());
                    Console.WriteLine($"Parsed constant: {number}");
                    Node constant = new ConstantValueNode(number);

                    if (leftNode == null)
                    {
                        leftNode = constant;
                    }
                    else
                    {
                        Console.WriteLine($"Adding binary operator node: {currentOperator} between {leftNode.GetType()} and {constant.GetType()}");
                        leftNode = new BinaryOperatorNode(currentOperator, leftNode, constant);
                    }

                    item.Clear();
                }
                else if (currentIndex == '+' || currentIndex == '-' || currentIndex == '*' || currentIndex == '/')
                {
                    currentOperator = currentIndex;
                    Console.WriteLine($"Parsed operator: {currentOperator}");
                }
            }

            return leftNode;
        }

        public void SetVariable(string variableName, double variableValue)
        {
            Console.WriteLine($"Setting variable {variableName} = {variableValue}");
            this.variables[variableName] = variableValue;
        }

        public double Evaluate() // bottom up evaluation
        {
            return this.root.Evaluate();
        }
    }
}
