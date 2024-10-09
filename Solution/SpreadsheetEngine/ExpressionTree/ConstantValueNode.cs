//-----------------------------------------------------------------------
// <copyright file="ConstantValueNode.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    /// <summary>
    /// ConstantValueNode class.
    /// </summary>
    internal class ConstantValueNode : Node
    {
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantValueNode"/> class.
        /// </summary>
        /// <param name="value">number to be stored in the node.</param>
        public ConstantValueNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// returns the value of the value node. This evaluate just simplfies the logic used in the BinaryOperatorNode.
        /// </summary>
        /// <returns>value of the node.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
