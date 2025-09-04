//-----------------------------------------------------------------------
// <copyright file="SpreadSheetTests.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CS8603 // mute null ref warnings.

namespace TestProject1
{
    using System.ComponentModel;
    using System.Reflection;
    using NUnit.Framework.Constraints;
    using SpreadsheetEngine;

    /// <summary>
    /// Testing class.
    /// </summary>
    public class SpreadSheetTests
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
            Assert.That(cell.Value, Is.EqualTo("!(bad reference)"));
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

            cell.Text = "5";
            copyCell.Text = "=A1";

            method.Invoke(spreadsheet, new object[] { cell, new PropertyChangedEventArgs(nameof(cell.Text)) });

            Assert.That(copyCell.Value, Is.EqualTo("5"));
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

            cell.Text = "5";
            copyCell.Text = "=Z50";

            method.Invoke(spreadsheet, new object[] { cell, new PropertyChangedEventArgs(nameof(cell.Text)) });

            Assert.That(copyCell.Value, Is.EqualTo("5"));
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

            Assert.That(copyCell.Value, Is.EqualTo(string.Empty));
        }

        // HW 7 Unit Tests

        /// <summary>
        /// Ensure cell references and dependants propogate vertically.
        /// </summary>
        [Test]
        public void TestCellChangesPropogateVertically()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);
            Cell cell3 = spreadsheet.GetCell(2, 0);
            Cell cell4 = spreadsheet.GetCell(3, 0);
            Cell cell5 = spreadsheet.GetCell(4, 0);

            cell1.Text = "5";
            cell2.Text = "=A1";
            cell3.Text = "=A1+ A2";
            cell4.Text = "=A3 + A2";

            Assert.That(cell4.Value, Is.EqualTo("15"));
        }

        /// <summary>
        /// Ensure cell references and dependants propogate horizontally.
        /// </summary>
        [Test]
        public void TestCellChangesPropogateHorizontally()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(0, 1);
            Cell cell3 = spreadsheet.GetCell(0, 2);
            Cell cell4 = spreadsheet.GetCell(0, 3);

            cell1.Text = "5";
            cell2.Text = "=A1";
            cell3.Text = "=A1 +B1";
            cell4.Text = "=C1 + B1";

            Assert.That(cell4.Value, Is.EqualTo("15"));
        }

        /// <summary>
        /// Ensure cell references and dependants propogate diagonally.
        /// </summary>
        [Test]
        public void TestCellChangesPropogateDiagonally()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 1);
            Cell cell3 = spreadsheet.GetCell(2, 2);
            Cell cell4 = spreadsheet.GetCell(3, 3);

            cell1.Text = "5";
            cell2.Text = "=A1";
            cell3.Text = "=A1+B2";
            cell4.Text = "=C3 + B2";

            Assert.That(cell4.Value, Is.EqualTo("15"));
        }

        /// <summary>
        /// Ensure foreign operators are handled properly.
        /// </summary>
        [Test]
        public void TestForeignOperator()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);
            Cell cell3 = spreadsheet.GetCell(2, 0);

            cell1.Text = "5";
            cell2.Text = "5";
            cell3.Text = "=5^5";

            Assert.That(cell3.Value, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Ensure foreign operators are handled properly for variables.
        /// </summary>
        [Test]
        public void TestForeignOperatorOnVariables()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);
            Cell cell3 = spreadsheet.GetCell(2, 0);

            cell1.Text = "5";
            cell2.Text = "5";
            cell3.Text = "=A1^A2";

            Assert.That(cell3.Value, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Ensure if a string is entered instead of a number it doesnt crash.
        /// </summary>
        [Test]
        public void TestStringValueEntered()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);

            cell1.Text = "5+5";
            cell2.Text = "A1 + 5"; // Ensure the value of A1 hasnet been actually set.
            Assert.That(cell2.Value, Is.EqualTo("!(bad reference)"));
        }

        /// <summary>
        /// Exact same demo sequence as provided in the HW assignment.
        /// </summary>
        [Test]
        public void TestSpreadSheetHW7ExactDemo()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);
            Cell cell3 = spreadsheet.GetCell(0, 1);
            Cell cell4 = spreadsheet.GetCell(1, 1);

            cell1.Text = "22";
            cell4.Text = "33";
            cell2.Text = "=A1";
            cell2.Text = "=A1 + B2";
            cell3.Text = "=A2*2";
            cell4.Text = "11";

            Assert.That(cell1.Value, Is.EqualTo("22"));
            Assert.That(cell2.Value, Is.EqualTo("33"));
            Assert.That(cell3.Value, Is.EqualTo("66"));
            Assert.That(cell4.Value, Is.EqualTo("11"));
        }

        /* HW 8 Tests
         * Set text
         * set background
         * undo
         * redo
         * peekUndo
         * peekRedo
         * cell dependancies reset
        */

        /// <summary>
        /// Ensure that cell text is being set through the CellTextCommand.
        /// </summary>
        [Test]
        public void TestCellTextCommandText()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 1);
            string newText = "20";

            spreadsheet.SetText(cell, newText);

            Assert.That(cell.Text, Is.EqualTo("20"));
        }

        /// <summary>
        /// Ensure that cell value is updated after the CellTextCommand has occured.
        /// </summary>
        [Test]
        public void TestCellTextCommandValue()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 1);
            string newText = "20";

            spreadsheet.SetText(cell, newText);

            Assert.That(cell.Value, Is.EqualTo("20"));
        }

        /// <summary>
        /// Ensure that cell BGColor is being set with the CellBackgroundCommand.
        /// </summary>
        [Test]
        public void TestCellBackgroundCommand()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 1);
            List<Cell> cells = new List<Cell>();
            cells.Add(cell);
            uint newBackgroundColor = 4286643968
;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            Assert.That(cells[0].BGColor, Is.EqualTo(4286643968));
        }

        /// <summary>
        /// Ensure that multiple cells BGColor is being set with the CellBackgroundCommand.
        /// </summary>
        [Test]
        public void TestCellBackgroundCommandMultipleCells()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            Cell cell2 = spreadsheet.GetCell(0, 2);
            Cell cell3 = spreadsheet.GetCell(0, 3);
            Cell cell4 = spreadsheet.GetCell(0, 4);

            List<Cell> cells = new List<Cell>();
            cells.Add(cell1);
            cells.Add(cell2);
            cells.Add(cell3);
            cells.Add(cell4);
            uint newBackgroundColor = 4286643968
;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            foreach (Cell cell in cells)
            {
                Assert.That(cell.BGColor, Is.EqualTo(4286643968));
            }
        }

        /// <summary>
        /// Ensure that Undoing cell text works.
        /// </summary>
        [Test]
        public void UndoCellText()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            spreadsheet.SetText(cell1, "50");
            spreadsheet.Undo();

            Assert.That(cell1.Text, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Ensure that Undo works for background colors.
        /// </summary>
        [Test]
        public void UndoCellBackgroundColor()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            Cell cell2 = spreadsheet.GetCell(0, 2);
            Cell cell3 = spreadsheet.GetCell(0, 3);
            Cell cell4 = spreadsheet.GetCell(0, 4);

            List<Cell> cells = new List<Cell>();
            cells.Add(cell1);
            cells.Add(cell2);
            cells.Add(cell3);
            cells.Add(cell4);
            uint newBackgroundColor = 4286643968
;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            spreadsheet.Undo();

            foreach (Cell cell in cells)
            {
                Assert.That(cell.BGColor, Is.EqualTo(4294967295)); // white
            }
        }

        /// <summary>
        /// Ensure that redoing cell text works.
        /// </summary>
        [Test]
        public void RedoCellText()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            spreadsheet.SetText(cell1, "50");
            spreadsheet.Undo();
            spreadsheet.Redo();

            Assert.That(cell1.Text, Is.EqualTo("50"));
        }

        /// <summary>
        /// Ensure that redoing works for background colors.
        /// </summary>
        [Test]
        public void RedoCellBackgroundColor()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            Cell cell2 = spreadsheet.GetCell(0, 2);
            Cell cell3 = spreadsheet.GetCell(0, 3);
            Cell cell4 = spreadsheet.GetCell(0, 4);

            List<Cell> cells = new List<Cell>();
            cells.Add(cell1);
            cells.Add(cell2);
            cells.Add(cell3);
            cells.Add(cell4);
            uint newBackgroundColor = 4286643968
;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            spreadsheet.Undo();
            spreadsheet.Redo();

            foreach (Cell cell in cells)
            {
                Assert.That(cell.BGColor, Is.EqualTo(4286643968));
            }
        }

        /// <summary>
        /// Ensure that a text command appears when it was most recent added to the command stack.
        /// </summary>
        [Test]
        public void TestPeekUndoTextCommands()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            spreadsheet.SetText(cell1, "50");

            string undoText = spreadsheet.PeekUndoStack();

            Assert.That(undoText, Is.EqualTo("Undo text change"));
        }

        /// <summary>
        /// Ensure a background color command appears when a background color was most recently changed.
        /// </summary>
        [Test]
        public void TestPeekUndoBackgroundColorCommand()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            List<Cell> cells = new List<Cell>();
            cells.Add(cell1);
            uint newBackgroundColor = 4286643968;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            string undoText = spreadsheet.PeekUndoStack();

            Assert.That(undoText, Is.EqualTo("Undo background color change"));
        }

        /// <summary>
        /// Ensure "Undo" is returned when no commands are in the undo stack.
        /// </summary>
        [Test]
        public void TestPeekUndoNoCommand()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            string undoText = spreadsheet.PeekUndoStack();

            Assert.That(undoText, Is.EqualTo("Undo"));
        }

        /// <summary>
        /// Ensure that a text command appears in redo stack after an Undo().
        /// </summary>
        [Test]
        public void TestPeekRedoTextCommands()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            spreadsheet.SetText(cell1, "50");
            spreadsheet.Undo();

            string undoText = spreadsheet.PeekRedoStack();

            Assert.That(undoText, Is.EqualTo("Redo text change"));
        }

        /// <summary>
        /// Ensure a background color command appears in the redo stack after a Undo().
        /// </summary>
        [Test]
        public void TestPeekRedoBackgroundColorCommand()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 1);
            List<Cell> cells = new List<Cell>();
            cells.Add(cell1);
            uint newBackgroundColor = 4286643968;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            spreadsheet.Undo();
            string undoText = spreadsheet.PeekRedoStack();

            Assert.That(undoText, Is.EqualTo("Redo background color change"));
        }

        /// <summary>
        /// Ensure "Redo" is returned when no commands are in the redo stack.
        /// </summary>
        [Test]
        public void TestPeekRedoNoCommand()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            string undoText = spreadsheet.PeekRedoStack();

            Assert.That(undoText, Is.EqualTo("Redo"));
        }

        /// <summary>
        /// Ensure that cell depancies are removed when an undo happends. This allows them to be re-evaluated naturally.
        /// </summary>
        [Test]
        public void TestUndoSequenceCellDependancyReset()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell1 = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);
            List<Cell> cells = new List<Cell>();
            Cell cell3 = spreadsheet.GetCell(0, 1);
            cells.Add(cell1);
            cells.Add(cell2);

            spreadsheet.SetText(cell1, "=5");
            spreadsheet.SetBackground(cells, 4286643968);
            spreadsheet.SetText(cell3, "=A1");

            spreadsheet.Undo();
            spreadsheet.Undo();

            Assert.That(cell3.Value, Is.EqualTo(string.Empty));
        }

        // HW9 Tests

        /// <summary>
        /// Ensure that saving a empty spreadsheet writes content to a file.
        /// </summary>
        [Test]
        public void TestSaveEmptySpreadsheet()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            string path = "C:\\Users\\ethan\\GIT\\GIT\\cpts321-ethan_rule-hws\\Solution\\TestProject1\\XmlTestFiles\\test.xml";

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                spreadsheet.Save(stream);
                stream.Close();
            }

            Assert.That(File.Exists(path), Is.True);
            string content = File.ReadAllText(path);
            Assert.IsNotEmpty(content);

            File.Delete(path);
        }

        /// <summary>
        /// Ensure loading empty spreadsheet empties all cell values.
        /// </summary>
        [Test]
        public void LoadEmptySpreadsheet()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 1);
            spreadsheet.SetText(cell, "10");

            string path = "C:\\Users\\ethan\\GIT\\GIT\\cpts321-ethan_rule-hws\\Solution\\TestProject1\\XmlTestFiles\\test.xml";

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                spreadsheet.Save(stream);

                // reset stream position to beginning of file.
                stream.Seek(0, SeekOrigin.Begin);

                Cell cell2 = spreadsheet.GetCell(0, 2);
                spreadsheet.SetText(cell2, "20");
                spreadsheet.Load(stream);

                Assert.That(cell2.Text, Is.EqualTo(string.Empty));
                stream.Close();
                File.Delete(path);
            }
        }

        /// <summary>
        /// Ensure that saving and loading expressions throws no exceptions and sets cell dependants + references properly.
        /// </summary>
        [Test]
        public void TestSaveAndLoadExpressions()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(0, 1);
            Cell cell3 = spreadsheet.GetCell(0, 2);
            Cell cell4 = spreadsheet.GetCell(0, 3);
            spreadsheet.SetText(cell, "10");
            spreadsheet.SetText(cell2, "20");
            spreadsheet.SetText(cell3, "=A1+B1");
            spreadsheet.SetText(cell4, "=C1 + B1");

            string path = "C:\\Users\\ethan\\GIT\\GIT\\cpts321-ethan_rule-hws\\Solution\\TestProject1\\XmlTestFiles\\test.xml";

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                spreadsheet.Save(stream);

                // reset stream position to beginning of file.
                stream.Seek(0, SeekOrigin.Begin);

                spreadsheet.Load(stream);

                Assert.That(cell4.Value, Is.EqualTo("50"));
                stream.Close();
                File.Delete(path);
            }
        }

        /// <summary>
        /// Ensure that saving and loading bgcolor throws no exceptions and saves properly.
        /// </summary>
        [Test]
        public void TestSaveAndLoadBgColor()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(0, 1);
            Cell cell3 = spreadsheet.GetCell(0, 2);
            Cell cell4 = spreadsheet.GetCell(0, 3);
            List<Cell> cells = new List<Cell>();
            cells.Add(cell);
            cells.Add(cell2);
            cells.Add(cell3);
            cells.Add(cell4);
            uint newBackgroundColor = 4286643968;
            spreadsheet.SetBackground(cells, newBackgroundColor);

            string path = "C:\\Users\\ethan\\GIT\\GIT\\cpts321-ethan_rule-hws\\Solution\\TestProject1\\XmlTestFiles\\test.xml";

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                spreadsheet.Save(stream);

                // reset stream position to beginning of file.
                stream.Seek(0, SeekOrigin.Begin);
                spreadsheet.Load(stream);

                Assert.That(cell.BGColor, Is.EqualTo(newBackgroundColor));
                Assert.That(cell2.BGColor, Is.EqualTo(newBackgroundColor));
                Assert.That(cell3.BGColor, Is.EqualTo(newBackgroundColor));
                Assert.That(cell4.BGColor, Is.EqualTo(newBackgroundColor));
                stream.Close();
                File.Delete(path);
            }
        }

        /// <summary>
        /// Ensure that text and bg tag order dont break or change the spreadsheet when loading.
        /// </summary>
        [Test]
        public void TestLoadUnorderedTags()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(1, 0);
            Cell cell3 = spreadsheet.GetCell(2, 0);
            List<Cell> cells = new List<Cell>();
            cells.Add(cell);
            cells.Add(cell2);
            cells.Add(cell3);

            string path = "C:\\Users\\ethan\\GIT\\GIT\\cpts321-ethan_rule-hws\\Solution\\TestProject1\\XmlTestFiles\\unordered.xml";

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                spreadsheet.Load(stream);

                Assert.That(cell.BGColor, Is.EqualTo(4294967168));
                Assert.That(cell.Text, Is.EqualTo("5"));
                Assert.That(cell2.BGColor, Is.EqualTo(4286644096));
                Assert.That(cell2.Text, Is.EqualTo("10"));
                Assert.That(cell3.BGColor, Is.EqualTo(4294934783));
                Assert.That(cell3.Text, Is.EqualTo("15"));
                stream.Close();
            }
        }

        // HW 10 Tests

        /// <summary>
        /// Ensure our of range cells cant be accessed.
        /// </summary>
        [Test]
        public void TestOutOfRangeReference()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);

            spreadsheet.SetText(cell, "Z12345");

            Assert.That(cell.Value, Is.EqualTo("!(bad reference)"));
        }

        /// <summary>
        /// Ensure a cell with an unrecognized string wont crash.
        /// </summary>
        [Test]
        public void TestStringCell()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);

            spreadsheet.SetText(cell, "ba");

            Assert.That(cell.Value, Is.EqualTo("!(bad reference)"));
        }

        /// <summary>
        /// Ensure an invalid cell mid expression changes text to be "!(bad reference)".
        /// </summary>
        [Test]
        public void TestMidExpressionInvalidCell()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);

            spreadsheet.SetText(cell, "=6+Cell*27");

            Assert.That(cell.Value, Is.EqualTo("!(bad reference)"));
        }

        /// <summary>
        /// Ensure that self references change text to "!(self reference)".
        /// </summary>
        [Test]
        public void TestSingleSelfReference()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);

            spreadsheet.SetText(cell, "=A1");

            Assert.That(cell.Value, Is.EqualTo("!(self reference)"));
        }

        /// <summary>
        /// Ensure that mid expression self references change text to "!(self reference)".
        /// </summary>
        [Test]
        public void TestMidExpressionSelfReference1()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);

            spreadsheet.SetText(cell, "=B2/(A1*A2)*7");

            Assert.That(cell.Value, Is.EqualTo("!(self reference)"));
        }

        /// <summary>
        /// Ensure that self references get caught mid expression 2.
        /// </summary>
        [Test]
        public void TestMidExpressionSelfReference2()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(0, 1);

            spreadsheet.SetText(cell2, "=2");
            spreadsheet.SetText(cell, "=101+(322*B2/A1)");

            Assert.That(cell.Value, Is.EqualTo("!(self reference)"));
        }

        /// <summary>
        /// Ensure that simple 2 way circular references get caught.
        /// </summary>
        [Test]
        public void TestCircularReferenceSimple()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0);
            Cell cell2 = spreadsheet.GetCell(0, 1);

            spreadsheet.SetText(cell, "=5");
            spreadsheet.SetText(cell2, "=A1");
            spreadsheet.SetText(cell, "=B1");

            Assert.That(cell.Value, Is.EqualTo("!(circular reference)"));
        }

        /// <summary>
        /// Ensure a loop of circular references get caught.
        /// </summary>
        [Test]
        public void TestCircularReferenceLoop()
        {
            Spreadsheet spreadsheet = new Spreadsheet(50, 26);
            Cell cell = spreadsheet.GetCell(0, 0); // A1
            Cell cell2 = spreadsheet.GetCell(0, 1); // B1
            Cell cell3 = spreadsheet.GetCell(1, 0); // A2
            Cell cell4 = spreadsheet.GetCell(1, 1); // B2

            spreadsheet.SetText(cell, "=5");
            spreadsheet.SetText(cell3, "=A1*5");
            spreadsheet.SetText(cell4, "=A2*4");
            spreadsheet.SetText(cell2, "=B2*3");
            spreadsheet.SetText(cell, "=B1*2");

            Assert.That(cell.Value, Is.EqualTo("!(circular reference)"));
        }

        /*
            The demo video provided goes against some of the things I implemented in previous assignments. Previous instructions said
            explicitly to not have default values of 0 and to handle exceptions. The current demo has default values of 0. I'm making the
            assumption that this assignment doesent want me to go backwards and remove my default value handling.
        */

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