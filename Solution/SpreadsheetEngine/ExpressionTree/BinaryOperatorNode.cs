//-----------------------------------------------------------------------
// <copyright file="BinaryOperatorNode.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    using System;

    /// <summary>
    /// BinaryOperatorNode class.
    /// </summary>
    internal class BinaryOperatorNode : Node
    {
        private char binaryOperator;
        private Node left;
        private Node right;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="binaryOperator">Addtion, Subtraction, Division, or Multiplication.</param>
        /// <param name="leftNode">Left child.</param>
        /// <param name="rightNode">Right child.</param>
        public BinaryOperatorNode(char binaryOperator, Node leftNode, Node rightNode)
        {
            this.binaryOperator = binaryOperator;
            this.left = leftNode;
            this.right = rightNode;
        }

        /// <summary>
        /// Evaluates Left and Right child nodes.
        /// </summary>
        /// <returns>value of left and right child when compared with the binary operator.</returns>
        /// <exception cref="DivideByZeroException">Left child is divided by zero.</exception>
        /// <exception cref="NotImplementedException">Binary operator does not exist in this context.</exception>
        public override double Evaluate() // TODO: implment order of operations
        {
            double leftValue = this.left.Evaluate();
            double rightValue = this.right.Evaluate();

            switch (this.binaryOperator)
            {
                case '+':
                    return leftValue + rightValue;
                case '-':
                    return leftValue - rightValue;
                case '*':
                    return leftValue * rightValue;
                case '/':
                    if (this.right.Evaluate() == 0)
                    {
                        throw new DivideByZeroException();
                    }

                    return leftValue / rightValue;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
