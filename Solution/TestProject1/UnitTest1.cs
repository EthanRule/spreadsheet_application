using SpreadsheetEngine;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSpreadSheetRowCount() // ensure rows are set to rows
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Assert.That(spreadsheet.RowCount, Is.EqualTo(50));
        }

        [Test]
        public void TestSpreadSheetColumnCount() // ensure cols are set to cols
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Assert.That(spreadsheet.ColumnCount, Is.EqualTo(26));
        }

        // Below are a bunch of GetCell tests for "off by 1" errors
        [Test]
        public void TestGetCell() // ensure normal cells can be found
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(10, 10);
            Assert.That(cell, Is.Not.Null);
        }

        [Test]
        public void TestGetCellUpperBounds() // ensure upper bound cells can be found
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(49, 25);
            Assert.That(cell, Is.Not.Null);
        }

        [Test]
        public void TestGetCellLowerBounds() // ensure lower bound cells can be found
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Assert.That(cell, Is.Not.Null);
        }

        [Test]
        public void TestGetCellOutOfBounds() // ensure cells out of bounds return null
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(-1, -1);
            Assert.That(cell, Is.Null);

            Cell cell2 = spreadsheet.GetCell(50, 26);
            Assert.That(cell, Is.Null);
        }

        private MethodInfo GetPrivateMethod(string methodName) // getter for private methods from Spreadsheet
        {
            var method = typeof(Spreadsheet).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                Assert.Fail(methodName);
            }
            return method;
        }

        [Test]
        public void TestSpreadsheetOnPropertyChangedTextToValue() // ensure the OnPropertyChanged event is updating text to value
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(5, 5);
            MethodInfo method = this.GetPrivateMethod("OnCellPropertyChanged");

            cell.Text = "test";
            // trigger a event from spreadsheet down to the cell
            method.Invoke(spreadsheet, new object[] {cell, new PropertyChangedEventArgs(nameof(cell.Text))});
            Assert.That(cell.Value, Is.EqualTo("test"));
        }

        [Test]
        public void TestSpreadsheetOnPropertyChangedCopy()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            MethodInfo method = this.GetPrivateMethod("OnCellPropertyChanged");

            Cell cell = spreadsheet.GetCell(0, 0);
            Cell copyCell = spreadsheet.GetCell(1, 0);

            cell.Text = "test";
            copyCell.Text = "=A1";

            method.Invoke(spreadsheet, new object[] { cell, new PropertyChangedEventArgs(nameof(cell.Text)) });
            method.Invoke(spreadsheet, new object[] { copyCell, new PropertyChangedEventArgs(nameof(copyCell.Text)) });

            Assert.That(copyCell.Value, Is.EqualTo("test"));
        }
    }
}