//-----------------------------------------------------------------------
// <copyright file="VariableNode.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    using System.Collections.Generic;

    /// <summary>
    /// VariableNode class.
    /// </summary>
    internal class VariableNode : Node
    {
        private string value;
        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="value">Name beginning with a char.</param>
        /// <param name="variables">Number associated with the name.</param>
        public VariableNode(string value, Dictionary<string, double> variables)
        {
            this.value = value;
            this.variables = variables ?? new Dictionary<string, double>(); // Ensure variables is not null
        }

        /// <summary>
        /// Evaluate function to return the correct variable value associated with the variable node.
        /// </summary>
        /// <returns>variable value.</returns>
        public override double Evaluate()
        {
            // https://stackoverflow.com/questions/52362941/in-line-trygetvalue-in-if-conditon-and-evaluate-its-value
            double variableValue;
            if (this.variables.TryGetValue(this.value, out variableValue))
            {
                return variableValue;
            }

            return 0;
        }
    }
}
