//-----------------------------------------------------------------------
// <copyright file="ExpressionTreeTests.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CS8600 // mute null ref warnings.
#pragma warning disable CS8602
#pragma warning disable CS8603

namespace TestProject1
{
    using System.Diagnostics;
    using System.Reflection;
    using SpreadsheetEngine.ExpressionTree;
    using SpreadsheetEngine.ExpressionTree.BinaryOperatorNodeTypes;

    /// <summary>
    /// Testing class.
    /// </summary>
    public class ExpressionTreeTests
    {
        // HW 5 Tests:

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

        /// <summary>
        /// Ensures setting and getting variables work.
        /// </summary>
        [Test]
        public void TestSetGetVariable() // ensure the variables are being set properly
        {
            ExpressionTree tree = new ExpressionTree("1+1");
            tree.SetVariable("A1", 1);

            Assert.That(tree.GetVariable("A1"), Is.EqualTo(1));
        }

        /// <summary>
        /// Ensures variable addition works.
        /// </summary>
        [Test]
        public void TestExpressionTreeMultipleVariableAddition()
        {
            ExpressionTree tree = new ExpressionTree("1+1");
            tree.SetExpression("A1+A3+B9+Z2+H4+G97+P5+O0432+L123123+HELLO");
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

            double result = tree.Evaluate();
            double expected = 1 + 9 + 2109301 + 24 + 900 + 24 + 101 + 92130 + 1248 + 32;
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Ensures variable subtraction works.
        /// </summary>
        [Test]
        public void TestExpressionTreeMultipleVariableSubtraction()
        {
            ExpressionTree tree = new ExpressionTree("10-5");
            tree.SetExpression("A1-A3-B9-Z2-H4-G97-P5-O0432-L123123-HELLO");
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

            double result = tree.Evaluate();
            double expected = 1 - 9 - 2109301 - 24 - 900 - 24 - 101 - 92130 - 1248 - 32;
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Ensures variable multiplication works.
        /// </summary>
        [Test]
        public void TestExpressionTreeMultipleVariableMultiplication()
        {
            ExpressionTree tree = new ExpressionTree("2*3");
            tree.SetExpression("A1*A3*Z2");
            tree.SetVariable("A1", 1);
            tree.SetVariable("A3", 9);
            tree.SetVariable("Z2", 24);

            double result = tree.Evaluate();
            double expected = 1 * 9 * 24;
            Assert.That(expected, Is.EqualTo(result));
        }

        /// <summary>
        /// Ensures variable division works.
        /// </summary>
        [Test]
        public void TestExpressionTreeMultipleVariableDivision()
        {
            ExpressionTree tree = new ExpressionTree("20/4");
            tree.SetExpression("A1/A3/Z2");
            tree.SetVariable("A1", 4);
            tree.SetVariable("A3", 2);
            tree.SetVariable("Z2", 2);

            double result = tree.Evaluate();
            double expected = 4 / 2 / 2;
            Assert.That(expected, Is.EqualTo(result));
        }

        /// <summary>
        /// Ensures you cant divide by zero.
        /// </summary>
        [Test]
        public void TestExpressionTreeDivisionByZero()
        {
            ExpressionTree tree = new ExpressionTree("20/0");
            Assert.Throws<DivideByZeroException>(() => tree.Evaluate());
        }

        /// <summary>
        /// Ensure top node is a binary operator node.
        /// </summary>
        [Test]
        public void TestParseExpressionTopNode()
        {
            ExpressionTree tree = new ExpressionTree(string.Empty);
            string testExpression = "5+10+24+90";

            MethodInfo method = this.GetPrivateMethod("ParseExpression");
            Node resultNode = (Node)method.Invoke(tree, new object[] { testExpression });
            Assert.That(resultNode, Is.InstanceOf<BinaryOperatorNode>());
        }

        /// <summary>
        /// Ensure single nodes work.
        /// </summary>
        [Test]
        public void TestParseExpressionSingleNode()
        {
            ExpressionTree tree = new ExpressionTree(string.Empty);
            string testExpression = "90";

            MethodInfo method = this.GetPrivateMethod("ParseExpression");
            Node resultNode = (Node)method.Invoke(tree, new object[] { testExpression });
            Assert.That(resultNode.Evaluate(), Is.EqualTo(90));
        }

        /// <summary>
        /// Ensure whitespace is skipped.
        /// </summary>
        [Test]
        public void TestParseExpressionWhiteSpace()
        {
            ExpressionTree tree = new ExpressionTree("1 + 2");
            Assert.That(tree.Evaluate(), Is.EqualTo(3));
        }

        /// <summary>
        /// Ensure two consecutive binary operators cannot be inputted. e.g. 1++2.
        /// </summary>
        [Test]
        public void TestTwoConsecutiveBinaryOperators()
        {
            string expression = "1++2";
            var ex = Assert.Throws<ArgumentException>(() => new ExpressionTree(expression));
            string expected = "Consecuitive Binary Operators Not Allowed";
            Assert.That(expected, Is.EqualTo(ex.Message));
        }

        /// <summary>
        /// Ensure two consecutive binary operators cannot be inputted. e.g. 1++2.
        /// </summary>
        [Test]
        public void TestThreeConsecutiveBinaryOperators()
        {
            string expression = "1*/-2";
            var ex = Assert.Throws<ArgumentException>(() => new ExpressionTree(expression));
            string expected = "Consecuitive Binary Operators Not Allowed";
            Assert.That(expected, Is.EqualTo(ex.Message));
        }

        /// <summary>
        /// Ensure variables can be set after the expression has been defined. This is the demo example.
        /// </summary>
        [Test]
        public void TestSetVariablesAfterExpression()
        {
            string expression = "Hello-12-World";
            ExpressionTree tree = new ExpressionTree(expression);
            tree.SetVariable("Hello", 42);
            tree.SetVariable("World", 20);
            double result = tree.Evaluate();
            double expected = 42 - 12 - 20;
            Assert.That(expected, Is.EqualTo(result));
        }

        // HW 6 TESTS:

        /// <summary>
        /// Ensure the variable values are being reset when a new expression tree is set.
        /// </summary>
        [Test]
        public void TestExpressionTreeClearsPrevVariables()
        {
            ExpressionTree tree = new ExpressionTree("20/4");
            tree.SetVariable("A1", 4);
            tree.SetVariable("A3", 2);
            tree.SetVariable("Z2", 2);
            tree.SetExpression("A1 * A3 * Z2");

            double result = 0;
            try
            {
                tree.Evaluate();
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            double expected = 0;
            Assert.That(expected, Is.EqualTo(result));
        }

        /// <summary>
        /// Ensure variable values are not being reset after evaluation.
        /// </summary>
        [Test]
        public void TestExpressionValuesStillExistAfterEval()
        {
            ExpressionTree tree = new ExpressionTree("20/4");
            tree.SetExpression("A1 * A3 * Z2");
            tree.SetVariable("A1", 4);
            tree.SetVariable("A3", 2);
            tree.SetVariable("Z2", 2);

            Assert.That(tree.GetVariable("A1"), Is.EqualTo(4));
            Assert.That(tree.GetVariable("A3"), Is.EqualTo(2));
            Assert.That(tree.GetVariable("Z2"), Is.EqualTo(2));
        }

        /// <summary>
        /// Ensures OperatorNodeFactory creates nodes with a type.
        /// </summary>
        [Test]
        public void TestBinaryOperatorNodeFactory()
        {
            var leftNode = new ConstantValueNode(1);
            var rightNode = new ConstantValueNode(2);
            char operatorChar = '+';
            BinaryOperatorNodeFactory binaryOperatorNodeFactory = new BinaryOperatorNodeFactory();
            BinaryOperatorNode result = binaryOperatorNodeFactory.CreateOperatorNode(operatorChar, leftNode, rightNode);

            Assert.IsInstanceOf<AdditionNode>(result);
        }

        // ORDER OF OPERATIONS TESTS

        /// <summary>
        /// Ensure mult sub order of operations work.
        /// </summary>
        [Test]
        public void TestOrderOfOperationsMultSub()
        {
            string expression = "4 * 4 - 2";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(14));
        }

        /// <summary>
        /// Ensure mult add order of operations work.
        /// </summary>
        [Test]
        public void TestOrderOfOperationsMultAdd()
        {
            string expression = "4 * 4 + 2";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(18));
        }

        /// <summary>
        /// Ensure div sub order of operations work.
        /// </summary>
        [Test]
        public void TestOrderOfOperationsDivSub()
        {
            string expression = "4 / 4 - 2";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(-1));
        }

        /// <summary>
        /// Ensure div add order of operations work.
        /// </summary>
        [Test]
        public void TestOrderOfOperationsDivAdd()
        {
            string expression = "4 / 4 + 2";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(3));
        }

        /// <summary>
        /// Ensure inner parenthesis work.
        /// </summary>
        [Test]
        public void TestInnerParenthesis()
        {
            string expression = "4 * (4 - 2)";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(8));
        }

        /// <summary>
        /// Ensure multiple inner parenthesis on an expression have no effect.
        /// </summary>
        [Test]
        public void TestMultipleInnerParenthesis()
        {
            string expression = "4 * (((4 - 2)))";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(8));
        }

        /// <summary>
        /// Ensure outer parenthesis wrapping the expression have no effect.
        /// </summary>
        [Test]
        public void TestOuterParenthesis()
        {
            string expression = "(4 * (4 - 2))";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(8));
        }

        /// <summary>
        /// Ensure multiple outer parenthesis have no effect.
        /// </summary>
        [Test]
        public void TestMultipleOuterParenthesis()
        {
            string expression = "(((4 * (4 - 2))))";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(8));
        }

        /// <summary>
        /// Ensure parenthesis on the first part can work too.
        /// </summary>
        public void TestInnerParenthesisStart()
        {
            string expression = "(4 * 4) - 2";
            ExpressionTree tree = new ExpressionTree(expression);
            double result = tree.Evaluate();
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo(14));
        }

        /// <summary>
        /// Private method getter.
        /// </summary>
        private MethodInfo GetPrivateMethod(string methodName) // getter for private methods from Spreadsheet
        {
            var method = typeof(ExpressionTree).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(methodName);
            }

            return method;
        }
    }
}