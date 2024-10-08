namespace TestProject1
{
    using System.ComponentModel;
    using System.Reflection;
    using SpreadsheetEngine;
    using SpreadsheetEngine.ExpressionTree;

    /// <summary>
    /// Testing class.
    /// </summary>
    public class ExpresisonTreeTests
    {
        /// <summary>
        /// Ensures rows are set to rows.
        /// </summary>
        [Test]
        public void TestExpressionAdd()
        {
            ExpressionTree tree = new ExpressionTree("1+2");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(3));
        }

        /// <summary>
        /// Ensures rows are set to rows.
        /// </summary>
        [Test]
        public void TestExpressionSub()
        {
            ExpressionTree tree = new ExpressionTree("3-2");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(1));
        }

        /// <summary>
        /// Ensures rows are set to rows.
        /// </summary>
        [Test]
        public void TestExpressionMult()
        {
            ExpressionTree tree = new ExpressionTree("9*9");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(81));
        }

        /// <summary>
        /// Ensures rows are set to rows.
        /// </summary>
        [Test]
        public void TestExpressionDiv()
        {
            ExpressionTree tree = new ExpressionTree("4/4");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(1));
        }
    }
}