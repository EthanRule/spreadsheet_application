//-----------------------------------------------------------------------
// <copyright file="ExpressionTree.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CS8600 // mute null ref warnings.
#pragma warning disable CS8603
#pragma warning disable CS8618

namespace SpreadsheetEngine.ExpressionTree
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// ExpresisonTree class.
    /// </summary>
    public class ExpressionTree
    {
        private Node root;
        private Dictionary<string, double> variables;
        private string expression;
        private BinaryOperatorNodeFactory binaryOperatorNodeFactory = new BinaryOperatorNodeFactory();
        private Dictionary<char, (int precedence, bool isLeftAssociative)> operators = new Dictionary<char, (int, bool)>();
        private HashSet<string> expressionVariables = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">Expression used to build tree.</param>
        public ExpressionTree(string expression)
        {
            this.operators = this.binaryOperatorNodeFactory.GetOperators();
            this.expression = expression;
            this.variables = new Dictionary<string, double>(); // init variables dict
            try
            {
                this.root = this.ParseExpression(expression);
            }
            catch (FormatException)
            {
                Debug.WriteLine("Format Exception thrown in expression tree constuctor");
                throw;
            }
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
        /// Gets the variables dict from an expression tree.
        /// </summary>
        /// <returns>variables dictionary.</returns>
        public Dictionary<string, double> GetVaribles()
        {
            return this.variables;
        }

        /// <summary>
        /// Sets the expression. This allows for console testing aswell as re-entering expresion after instatiation.
        /// </summary>
        /// <param name="expression">requires expression.</param>
        public void SetExpression(string expression)
        {
            this.expression = expression;
            this.variables.Clear(); // Clear out all previous variable values for new expression.
            Debug.WriteLine("Cleared out previous variables");
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

        // need to display these exceptions to user not just the debug log
        // need to handle string hello + string hello exception
        // maybe show a popup to the user
        // Allow user to add and remove operators

        /// <summary>
        /// Evaluates tree.
        /// </summary>
        /// <returns>The tree evaluation result.</returns>
        public double Evaluate() // bottom up evaluation
        {
            try
            {
                if (this.root != null)
                {
                    return this.root.Evaluate();
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (FormatException ex)
            {
                throw;
            }
            catch (DivideByZeroException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the set of variables used in the current expression.
        /// </summary>
        /// <returns>set of current expression variables.</returns>
        public HashSet<string> GetExpressionVariables()
        {
            return this.expressionVariables;
        }

        // Dijkstra's Shunting-yard algorithm: https://mathcenter.oxford.emory.edu/site/cs171/shuntingYardAlgorithm/
        private Node ParseExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            StringBuilder item = new StringBuilder();
            Stack<char> operatorStack = new Stack<char>();
            List<string> postfix = new List<string>();
            PrevIndex previous = PrevIndex.None;

            // After the reflection refactor. operators are found from the binaryOperatorNodeFactory.
            for (int i = 0; i < expression.Length; i++)
            {
                char currentSymbol = expression[i];
                if (currentSymbol == ' ') // whitespace handling
                {
                    continue;
                }

                if (char.IsDigit(currentSymbol))
                {
                    item.Clear();
                    while (i < expression.Length && char.IsDigit(expression[i]))
                    {
                        item.Append(expression[i]);
                        i++;
                    }

                    postfix.Add(item.ToString());
                    i--;
                    previous = PrevIndex.Number;
                    continue;
                }

                if (char.IsLetter(currentSymbol))
                {
                    item.Clear();
                    while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
                    {
                        item.Append(expression[i]);
                        i++;
                    }

                    if (!this.expressionVariables.Contains(item.ToString()))
                    {
                        this.expressionVariables.Add(item.ToString());
                    }

                    postfix.Add(item.ToString());
                    i--;
                    previous = PrevIndex.Variable;
                    continue;
                }

                if (currentSymbol == '(')
                {
                    operatorStack.Push(currentSymbol);
                    previous = PrevIndex.None;
                    continue;
                }

                if (currentSymbol == ')')
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                    {
                        postfix.Add(operatorStack.Pop().ToString());
                    }

                    operatorStack.Pop(); // discard the '('
                    previous = PrevIndex.None;
                    continue;
                }

                // 1. Throw an exception for consecutive binary operators.
                // 2. Pop operators from the stack to the postfix list while maintaining precedence.
                //    If the top operator on the stack has higher precedence, or the same precedence
                //    and the current operator is left associative, pop the operator.
                // 3. Push the current operator onto the stack and update the previous operators' state.
                // Note: Currently this does not support right associative operators. TODO
                if (this.operators.ContainsKey(currentSymbol))
                {
                    while (operatorStack.Count > 0)
                    {
                        if (previous == PrevIndex.Operator)
                        {
                            throw new ArgumentException("Consecuitive Binary Operators Not Allowed");
                        }

                        char topOperator = operatorStack.Peek();

                        if (topOperator == '(' || topOperator == ')')
                        {
                            break;
                        }

                        if (this.operators[topOperator].precedence > this.operators[currentSymbol].precedence ||
                            (this.operators[topOperator].precedence == this.operators[currentSymbol].precedence && this.operators[currentSymbol].isLeftAssociative))
                        {
                            postfix.Add(operatorStack.Pop().ToString());
                        }
                        else
                        {
                            break;
                        }
                    }

                    operatorStack.Push(currentSymbol);
                    previous = PrevIndex.Operator;
                }
                else
                {
                    throw new FormatException($"Operator '{currentSymbol}' is not recognized.");
                }
            }

            // Pop all remaining operators
            while (operatorStack.Count > 0)
            {
                postfix.Add(operatorStack.Pop().ToString());
            }

            return this.ConstructExpressionTree(postfix, this.operators);
        }

        // Construct Expression Tree from Postfix
        private Node ConstructExpressionTree(List<string> postfix, Dictionary<char, (int, bool)> operators)
        {
            Stack<Node> nodeStack = new Stack<Node>();

            foreach (string token in postfix)
            {
                if (double.TryParse(token, out double constantValue))
                {
                    Node constantNode = new ConstantValueNode(constantValue);
                    nodeStack.Push(constantNode);
                }
                else if (char.IsLetter(token[0]))
                {
                    Node variableNode = new VariableNode(token, this.variables);
                    nodeStack.Push(variableNode);
                }
                else if (operators.ContainsKey(token[0]))
                {
                    Node rightNode = nodeStack.Pop();
                    Node leftNode = nodeStack.Pop();

                    Node operatorNode = this.binaryOperatorNodeFactory.CreateOperatorNode(token[0], leftNode, rightNode);
                    nodeStack.Push(operatorNode);
                }
            }

            return nodeStack.Pop();
        }
    }
}
