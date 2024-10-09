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
            double leftValue = this.left.Evaluate();
            double rightValue = this.right.Evaluate();

            switch (this._operator)
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
