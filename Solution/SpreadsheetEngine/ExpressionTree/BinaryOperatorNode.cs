using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine.ExpressionTree
{
    internal class BinaryOperatorNode : Node
    {
        private char _operator;
        private Node left;
        private Node right;

        public BinaryOperatorNode(char _operator, Node leftNode, Node rightNode)
        {
            this._operator = _operator;
            this.left = leftNode;
            this.right = rightNode;
        }

        public override double Evaluate() // implment following order of operations
        {
           switch (this._operator)
           {
                case '+':
                    return left.Evaluate() + right.Evaluate();
                case '-':
                    return left.Evaluate() - right.Evaluate();
                case '*':
                    return left.Evaluate() * right.Evaluate();
                case '/':
                    return left.Evaluate() / right.Evaluate();
                default:
                    throw new NotImplementedException();
           }
        }
    }
}
