//-----------------------------------------------------------------------
// <copyright file="ExpressionTree.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// ExpresisonTree class.
    /// </summary>
    public class ExpressionTree
    {
        private Node root;
        private Dictionary<string, double> variables;
        private string expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">Expression used to build tree.</param>
        public ExpressionTree(string expression)
        {
            this.expression = expression;
            this.variables = new Dictionary<string, double>(); // init variables dict
            this.root = this.ParseExpression(expression);
        }

        private enum PrevIndex // used to track multiple operators used back to back. like "1++2"
        {
            Number,
            Variable,
            Operator,
            None,
        }

        /// <summary>
        /// Returns the varible from the variables dict.
        /// </summary>
        /// <param name="key">Variable name.</param>
        /// <returns>Variable value.</returns>
        public double GetVariable(string key)
        {
            return this.variables[key];
        }

        /// <summary>
        /// Sets the expression. This allows for console testing aswell as re-entering expresion after instatiation.
        /// </summary>
        /// <param name="expression">requires expression.</param>
        public void SetExpression(string expression)
        {
            this.expression = expression;
            this.root = this.ParseExpression(expression);
        }

        /// <summary>
        /// Gets the expression from the tree.
        /// </summary>
        /// <returns>returns expression.</returns>
        public string GetExpression()
        {
            return this.expression;
        }

        /// <summary>
        /// Sets a variable in the variables dict.
        /// </summary>
        /// <param name="variableName">variable name.</param>
        /// <param name="variableValue">variable value.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// Evaluates tree.
        /// </summary>
        /// <returns>The tree evaluation result.</returns>
        public double Evaluate() // bottom up evaluation
        {
            return this.root.Evaluate();
        }

        private Node ParseExpression(string expression)
        {
            if (expression == string.Empty)
            {
                return null;
            }

            StringBuilder item = new StringBuilder();
            Node leftNode = null;
            char currentOperator = '\0'; // ensure parenthesis are never an operator. but still needs to be handled to group expressions. add '^'?
            PrevIndex prev = PrevIndex.None;

            for (int i = 0; i < expression.Length; i++)
            {
                // check for variable
                char currentIndex = expression[i];

                if (currentIndex == ' ')
                {
                    continue;
                }

                if (char.IsLetter(currentIndex))
                {
                    item.Append(currentIndex);

                    // find the values after the letter
                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        currentIndex = expression[j];

                        if (currentIndex == ' ')
                        {
                            continue;
                        }

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

                    Node variable = new VariableNode(item.ToString(), this.variables);
                    if (leftNode == null)
                    {
                        leftNode = variable;
                    }
                    else
                    {
                        leftNode = new BinaryOperatorNode(currentOperator, leftNode, variable);
                    }

                    item.Clear();
                    prev = PrevIndex.Variable;
                }
                else if (char.IsDigit(currentIndex))
                {
                    item.Append(expression[i]);

                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        currentIndex = expression[j];

                        if (currentIndex == ' ')
                        {
                            continue;
                        }

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
                    prev = PrevIndex.Number;
                }
                else if (currentIndex == '+' || currentIndex == '-' || currentIndex == '*' || currentIndex == '/')
                {
                    if (prev == PrevIndex.Operator)
                    {
                        throw new ArgumentException("Consecuitive Binary Operators Not Allowed");
                    }

                    currentOperator = currentIndex;
                    prev = PrevIndex.Operator;
                }
            }

            return leftNode;
        }
    }
}
