using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine.ExpressionTree
{
    internal class ConstantValueNode : Node
    {
        private double value;

        public ConstantValueNode(double value)
        {
            this.value = value;
        }

        public override double Evaluate()
        {
            return value;
        }
    }
}
