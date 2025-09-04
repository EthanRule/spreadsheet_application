//-----------------------------------------------------------------------
// <copyright file="BinaryOperatorNode.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    /// <summary>
    /// BinaryOperatorNode abstract class that is used as the base class for differing operators created in the OperatorNodeFactory.
    /// </summary>
    public abstract class BinaryOperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="binaryOperator">Addtion, Subtraction, Division, or Multiplication.</param>
        /// <param name="leftNode">Left child.</param>
        /// <param name="rightNode">Right child.</param>
        public BinaryOperatorNode(char binaryOperator, Node leftNode, Node rightNode)
        {
            this.BinaryOperator = binaryOperator;
            this.Left = leftNode;
            this.Right = rightNode;
        }

        /// <summary>
        /// Gets binary operator.
        /// </summary>
        protected char BinaryOperator { get; }

        /// <summary>
        /// Gets or Sets Left child node.
        /// </summary>
        protected Node Left { get; set; }

        /// <summary>
        /// Gets or sets Right child node.
        /// </summary>
        protected Node Right { get; set; }

        /// <summary>
        /// Recursive evaluate call. This is further overridden in the specific operator node clases.
        /// </summary>
        /// <returns>double number.</returns>
        public override abstract double Evaluate(); // This will be implemented in the subclasses
    }
}
