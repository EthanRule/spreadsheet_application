//-----------------------------------------------------------------------
// <copyright file="BinaryOperatorNodeFactory.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine.ExpressionTree
{
    using System;
    using System.Reflection;
    using SpreadsheetEngine.ExpressionTree.BinaryOperatorNodeTypes;

    /// <summary>
    /// Pure Fabrication BinaryOperatorNodeFactory to instantiate differing types of BinaryOperatorNode classes.
    /// </summary>
    public class BinaryOperatorNodeFactory
    {
        private Dictionary<char, Type> operators = new Dictionary<char, Type>(); // Maybe condense these two dicts
        private Dictionary<char, (int precedence, bool isLeftAssociative)> operatorPrecedenceAssociativity = new Dictionary<char, (int, bool)>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNodeFactory"/> class.
        /// </summary>
        /// <param name="expression">Factory to create BinaryOperatorNodes.</param>
        public BinaryOperatorNodeFactory()
        {
            this.TraverseAvailableOperators((op, type) => this.operators.Add(op, type));
        }

        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// Gets the operators, precedences, and associativities.
        /// </summary>
        /// <returns>dict of operators, precedences, and associativities.</returns>
        public Dictionary<char, (int precedence, bool isLeftAssociative)> GetOperators()
        {
            return this.operatorPrecedenceAssociativity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNodeFactory"/> class.
        /// </summary>
        /// <param name="op">Binary operator.</param>
        /// <param name="leftNode">Left node.</param>
        /// <param name="rightNode">Right node.</param>
        /// <returns>BinaryOperatorNode of type op with left and right node children.</returns>
        public BinaryOperatorNode CreateOperatorNode(char op, Node leftNode, Node rightNode)
        {
            if (this.operators.ContainsKey(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(this.operators[op], op, leftNode, rightNode);
                if (operatorNodeObject is BinaryOperatorNode)
                {
                    return (BinaryOperatorNode)operatorNodeObject;
                }
            }

            throw new Exception("Unhandled operator");
        }

        private void TraverseAvailableOperators(OnOperator onOperator)
        {
            // get the type declaration of OperatorNode
            Type operatorNodeType = typeof(BinaryOperatorNode);

            // Iterate over all loaded assemblies:
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Get all types that inherit from our OperatorNode class using LINQ
                IEnumerable<Type> operatorTypes =
                assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                // Iterate over those subclasses of OperatorNode
                foreach (var type in operatorTypes)
                {
                    // for each subclass, retrieve the operator, precedence, and associtvity property
                    PropertyInfo operatorField = type.GetProperty("Operator");
                    PropertyInfo precedenceField = type.GetProperty("Precedence");
                    PropertyInfo isLeftAssociativeField = type.GetProperty("IsLeftAssociative");

                    // dont add the node type if values are missing.
                    if (operatorField != null && precedenceField != null && isLeftAssociativeField != null)
                    {
                        // Get the operator, precedence, and associativity
                        char operatorSymbol = (char)operatorField.GetValue(null);
                        int precedence = (int)precedenceField.GetValue(null);
                        bool isLeftAssociative = (bool)isLeftAssociativeField.GetValue(null);

                        onOperator(operatorSymbol, type);

                        // build more advanced dict used for shunting-yard
                        this.operatorPrecedenceAssociativity[operatorSymbol] = (precedence, isLeftAssociative);
                    }
                }
            }
        }
    }
}
