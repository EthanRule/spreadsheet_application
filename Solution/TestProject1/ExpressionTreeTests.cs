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
        /// Ensures basic addition works.
        /// </summary>
        [Test]
        public void TestExpressionAdd()
        {
            ExpressionTree tree = new ExpressionTree("1+2");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(3));
        }

        /// <summary>
        /// Ensures basic subtraction works.
        /// </summary>
        [Test]
        public void TestExpressionSub()
        {
            ExpressionTree tree = new ExpressionTree("3-2");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(1));
        }

        /// <summary>
        /// Ensures basic multiplication works.
        /// </summary>
        [Test]
        public void TestExpressionMult()
        {
            ExpressionTree tree = new ExpressionTree("9*9");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(81));
        }

        /// <summary>
        /// Ensures basic division works.
        /// </summary>
        [Test]
        public void TestExpressionDiv()
        {
            ExpressionTree tree = new ExpressionTree("4/4");
            double number = tree.Evaluate();

            Assert.That(number, Is.EqualTo(1));
        }

        [Test]
        public void TestSetVariable() // ensure the variables are being set properly
        {
            ExpressionTree tree = new ExpressionTree("1+1");
            tree.SetVariable("A1", 1);

            Assert.That(tree.GetVariable("A1"), Is.EqualTo(1));
        }

        [Test]
        public void TestExpressionTreeMultipleVariableAddition()
        {
            ExpressionTree tree = new ExpressionTree("1+1");
            tree.SetVariable("A1", 1);
            tree.SetVariable("A3", 9);
            tree.SetVariable("B9", 2109301);
            tree.SetVariable("Z2", 24);
            tree.SetVariable("H4", 900);
            tree.SetVariable("G97", 24);
            tree.SetVariable("P5", 101);
            tree.SetVariable("O0432", 92130);
            tree.SetVariable("L123123", 1248);
            tree.SetVariable("HELLO", 32);
            tree.SetExpression("A1+A3+B9+Z2+H4+G97+P5+O0432+L123123+HELLO");

            double result = tree.Evaluate();
            double expected = 1 + 9 + 2109301 + 24 + 900 + 24 + 101 + 92130 + 1248 + 32;
            Assert.That(expected, Is.EqualTo(result));
        }

        [Test]
        public void TestExpressionTreeMultipleVariableSubtraction()
        {
            ExpressionTree tree = new ExpressionTree("10-5");
            tree.SetVariable("A1", 1);
            tree.SetVariable("A3", 9);
            tree.SetVariable("B9", 2109301);
            tree.SetVariable("Z2", 24);
            tree.SetVariable("H4", 900);
            tree.SetVariable("G97", 24);
            tree.SetVariable("P5", 101);
            tree.SetVariable("O0432", 92130);
            tree.SetVariable("L123123", 1248);
            tree.SetVariable("HELLO", 32);
            tree.SetExpression("A1-A3-B9-Z2-H4-G97-P5-O0432-L123123-HELLO");

            double result = tree.Evaluate();
            double expected = 1 - 9 - 2109301 - 24 - 900 - 24 - 101 - 92130 - 1248 - 32;
            Assert.That(expected, Is.EqualTo(result));
        }

        [Test]
        public void TestExpressionTreeMultipleVariableMultiplication()
        {
            ExpressionTree tree = new ExpressionTree("2*3");
            tree.SetVariable("A1", 1);
            tree.SetVariable("A3", 9);
            tree.SetVariable("Z2", 24);
            tree.SetExpression("A1*A3*Z2");

            double result = tree.Evaluate();
            double expected = 1 * 9 * 24;
            Assert.That(expected, Is.EqualTo(result));
        }

        [Test]
        public void TestExpressionTreeMultipleVariableDivision()
        {
            ExpressionTree tree = new ExpressionTree("20/4");
            tree.SetVariable("A1", 4);
            tree.SetVariable("A3", 2);
            tree.SetVariable("Z2", 2);
            tree.SetExpression("A1/A3/Z2");

            double result = tree.Evaluate();
            double expected = 4/2/2;
            Assert.That(expected, Is.EqualTo(result));
        }

        [Test]
        public void TestExpressionTreeDivisionByZero()
        {
            ExpressionTree tree = new ExpressionTree("20/0");
            Assert.Throws<DivideByZeroException>(() => tree.Evaluate());
        }
    }
}