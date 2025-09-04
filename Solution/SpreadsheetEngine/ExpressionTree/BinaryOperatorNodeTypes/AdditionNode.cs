//-----------------------------------------------------------------------
// <copyright file="AdditionNode.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree.BinaryOperatorNodeTypes
{
    /// <summary>
    /// Initializes class Addition Node.
    /// </summary>
    public class AdditionNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionNode"/> class.
        /// </summary>
        /// <param name="binaryOperator">char operator.</param>
        /// <param name="leftNode">leftNode pointer.</param>
        /// <param name="rightNode">rightNode pointer.</param>
        public AdditionNode(char binaryOperator, Node leftNode, Node rightNode)
            : base(binaryOperator, leftNode, rightNode)
        {
        }

        /// <summary>
        /// Gets operator for BinaryOperatorNodeFactory.
        /// </summary>
        public static char Operator => '+';

        /// <summary>
        /// Gets precedence for BinaryOperatorNodeFactory.
        /// </summary>
        public static int Precedence => 1;

        /// <summary>
        /// Gets a value indicating whether left or right associative.
        /// </summary>
        public static bool IsLeftAssociative => true;

        /// <summary>
        /// Adds left and right nodes.
        /// </summary>
        /// <returns>The addition between left and right node values.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() + this.Right.Evaluate();
        }
    }
}
