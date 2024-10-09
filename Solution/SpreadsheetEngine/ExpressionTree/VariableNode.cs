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
        private Dictionary<string, double> variables;

        public VariableNode(string value, Dictionary<string, double> variables)
        {
            this.value = value;
            this.variables = variables ?? new Dictionary<string, double>(); // Ensure variables is not null
        }

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
