//-----------------------------------------------------------------------
// <copyright file="MultiplicationNode.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree.BinaryOperatorNodeTypes
{
    /// <summary>
    /// Initializes class Addition Node.
    /// </summary>
    public class MultiplicationNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplicationNode"/> class.
        /// </summary>
        /// <param name="binaryOperator">char operator.</param>
        /// <param name="leftNode">leftNode pointer.</param>
        /// <param name="rightNode">rightNode pointer.</param>
        public MultiplicationNode(char binaryOperator, Node leftNode, Node rightNode)
            : base(binaryOperator, leftNode, rightNode)
        {
        }

        /// <summary>
        /// Gets operator for BinaryOperatorNodeFactory.
        /// </summary>
        public static char Operator => '*';

        /// <summary>
        /// Gets precedence for BinaryOperatorNodeFactory.
        /// </summary>
        public static int Precedence => 2;

        /// <summary>
        /// Gets a value indicating whether left or right associative.
        /// </summary>
        public static bool IsLeftAssociative => true;

        /// <summary>
        /// Multiplies left and right nodes.
        /// </summary>
        /// <returns>The multiplication between left and right node values.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}
