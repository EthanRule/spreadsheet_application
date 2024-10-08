using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpreadsheetEngine.ExpressionTree
{
    public class ExpressionTree
    {
        private Node root;
        private Dictionary<string, double> variables;

        public ExpressionTree(string expression)
        {
            // parse expression:
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

                    Node variable = new VariableNode(item.ToString());
                    if (leftNode == null)
                    {
                        leftNode = variable;
                    }
                    else
                    {
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
                    Node constant = new ConstantValueNode(number);

                    if (leftNode == null)
                    {
                        leftNode = constant;
                    }
                    else
                    {
                        leftNode = new BinaryOperatorNode(currentOperator, leftNode, constant);
                    }

                    item.Clear();
                }
                else if (currentIndex == '+' || currentIndex == '-' || currentIndex == '*' || currentIndex == '/')
                {
                    currentOperator = currentIndex;
                }
            }

            return leftNode;
        }

        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        public double Evaluate() // bottom up evaluation
        {
            return this.root.Evaluate();
        }
    }
}
