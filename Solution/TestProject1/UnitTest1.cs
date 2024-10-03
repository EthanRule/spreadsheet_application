//-----------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestProject1
{
    using System.ComponentModel;
    using System.Reflection;
    using SpreadsheetEngine;

    /// <summary>
    /// Testing class.
    /// </summary>
    public class UnitTest1
    {
        /// <summary>
        /// Ensures rows are set to rows.
        /// </summary>
        [Test]
        public void TestSpreadSheetRowCount()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Assert.That(spreadsheet.RowCount, Is.EqualTo(50));
        }

        /// <summary>
        /// Ensures cols are set to cols.
        /// </summary>
        [Test]
        public void TestSpreadSheetColumnCount()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Assert.That(spreadsheet.ColumnCount, Is.EqualTo(26));
        }

        /// <summary>
        /// Below are a bunch of GetCell tests for "off by 1" errors.
        /// </summary>
        [Test]
        public void TestGetCell()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(10, 10);
            Assert.That(cell, Is.Not.Null);
        }

        /// <summary>
        /// Test upperbound cell.
        /// </summary>
        [Test]
        public void TestGetCellUpperBounds()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(49, 25);
            Assert.That(cell, Is.Not.Null);
        }

        /// <summary>
        /// Test lowerbound cell.
        /// </summary>
        [Test]
        public void TestGetCellLowerBounds() // ensure lower bound cells can be found
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Assert.That(cell, Is.Not.Null);
        }

        /// <summary>
        /// Test outofbounds cells.
        /// </summary>
        [Test]
        public void TestGetCellOutOfBounds() // ensure cells out of bounds return null
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(-1, -1);
            Assert.That(cell, Is.Null);

            Cell cell2 = spreadsheet.GetCell(50, 26);
            Assert.That(cell, Is.Null);
        }

        /// <summary>
        /// Test if cell values are changed from text changes.
        /// </summary>
        [Test]
        public void TestSpreadsheetOnPropertyChangedTextToValue() // ensure the OnPropertyChanged event is updating text to value
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(5, 5);
            MethodInfo method = this.GetPrivateMethod("OnCellPropertyChanged");

            cell.Text = "test";

            // trigger a event from spreadsheet down to the cell
            method.Invoke(spreadsheet, new object[] { cell, new PropertyChangedEventArgs(nameof(cell.Text)) });
            Assert.That(cell.Value, Is.EqualTo("test"));
        }

        /// <summary>
        /// Test if cell values are changed from text equations like "=A1". (Lower Bounds).
        /// </summary>
        [Test]
        public void TestSpreadsheetOnPropertyChangedCopyLower() // ensure that we can copy from the lower bound cells
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            MethodInfo method = this.GetPrivateMethod("OnCellPropertyChanged");

            Cell cell = spreadsheet.GetCell(0, 0);
            Cell copyCell = spreadsheet.GetCell(1, 0);

            cell.Text = "test";
            copyCell.Text = "=A1";

            method.Invoke(spreadsheet, new object[] { cell, new PropertyChangedEventArgs(nameof(cell.Text)) });

            Assert.That(copyCell.Value, Is.EqualTo("test"));
        }

        /// <summary>
        /// Test if cell values are changed from text equations like "=Z50". (Upper Bounds).
        /// </summary>
        [Test]
        public void TestSpreadsheetOnPropertyChangedCopyUpper() // ensure that we can copy from the upper bound cells
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            MethodInfo method = this.GetPrivateMethod("OnCellPropertyChanged");

            Cell cell = spreadsheet.GetCell(49, 25);
            Cell copyCell = spreadsheet.GetCell(1, 0);

            cell.Text = "test";
            copyCell.Text = "=Z50";

            method.Invoke(spreadsheet, new object[] { cell, new PropertyChangedEventArgs(nameof(cell.Text)) });

            Assert.That(copyCell.Value, Is.EqualTo("test"));
        }

        /// <summary>
        /// Ensure text cannot be updated with an equation referencing a cell that does not exist.
        /// </summary>
        [Test]
        public void TestSpreadsheetOnPropertyChangedCopyOutOfBounds() // ensure we cannot copy from out of bounds cells
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            MethodInfo method = this.GetPrivateMethod("OnCellPropertyChanged");

            Cell copyCell = spreadsheet.GetCell(1, 0);

            copyCell.Text = "=Z51";

            method.Invoke(spreadsheet, new object[] { copyCell, new PropertyChangedEventArgs(nameof(copyCell.Text)) });

            Assert.That(copyCell.Value, Is.Empty);
        }

        /// <summary>
        /// Private method getter.
        /// </summary>
        private MethodInfo GetPrivateMethod(string methodName) // getter for private methods from Spreadsheet
        {
            var method = typeof(Spreadsheet).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(methodName);
            }

            return method;
        }
    }
}