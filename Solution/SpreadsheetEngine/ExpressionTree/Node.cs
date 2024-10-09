//-----------------------------------------------------------------------
// <copyright file="Node.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract Node class.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// The Evaluate() function gets overridden in all classes that extend the Node class for their individual purposes.
        /// </summary>
        /// <returns>Evaluated number.</returns>
        public abstract double Evaluate();
    }
}
