using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine.ExpressionTree
{
    internal class VariableNode : Node
    {
        private string value;

        public VariableNode(string value)
        {
            this.value = value;
        }

        public override double Evaluate()
        {
            // probably need a getter to get this value from the spreadsheet.
            return 0.0;
        }
    }
}
