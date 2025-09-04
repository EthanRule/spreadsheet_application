//-----------------------------------------------------------------------
// <copyright file="Spreadsheet.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CS8603 // mute null ref warnings.
#pragma warning disable CS8618
#pragma warning disable CS8622
#pragma warning disable CS8600
#pragma warning disable CS8602

namespace SpreadsheetEngine
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Linq;
    using SpreadsheetEngine.CustomEvents;

    /// <summary>
    /// Creates spreadsheet of cells.
    /// </summary>
    public class Spreadsheet
    {
        private readonly int columnCount;
        private readonly int rowCount;
        private Cell[,] cellMatrix;
        private Stack<ICommand> undoStack = new Stack<ICommand>();
        private Stack<ICommand> redoStack = new Stack<ICommand>();
        private bool suppressExceptions = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Spreadsheet(int rows, int columns)
        {
            this.cellMatrix = new Cell[rows, columns];
            this.rowCount = rows;
            this.columnCount = columns;

            // subscribe to cells property changed
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.cellMatrix[i, j] = new SpreadsheetCell(i, j);
                    this.cellMatrix[i, j].PropertyChanged += this.OnCellPropertyChanged; // subscribe to cell
                }
            }
        }

        /// <summary>
        /// Event handler for cell property changes.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        /// <summary>
        /// Event handler for exceptions.
        /// </summary>
        public event EventHandler<ExceptionOccurredEventArgs> ExceptionOccurred;

        /// <summary>
        /// Event handler for undo and redo stack changes.
        /// </summary>
        public event PropertyChangedEventHandler StackChanged;

        /// <summary>
        /// Gets Column Count.
        /// </summary>
        public int ColumnCount
        {
            get { return this.columnCount; }
        }

        /// <summary>
        /// Gets Row Count.
        /// </summary>
        public int RowCount
        {
            get { return this.rowCount; }
        }

        /// <summary>
        /// Gets the cell in the cellMatrix.
        /// </summary>
        /// <param name="row">row index.</param>
        /// <param name="col">col index.</param>
        /// <returns>Cell object.</returns>
        public Cell GetCell(int row, int col)
        {
            if (row < 0 || col < 0 || row >= this.rowCount || col >= this.columnCount)
            {
                return null;
            }

            return this.cellMatrix[row, col];
        }

        /// <summary>
        /// Set cell text with command.
        /// </summary>
        /// <param name="cell">Cell in spreadsheet.</param>
        /// <param name="text">New text for cell.</param>
        public void SetText(Cell cell, string text)
        {
            bool isCircularRef = this.CircularReference(cell, text);
            bool isSelfRef = this.SelfReference(cell, text);

            if (isCircularRef)
            {
                text = "!(circular reference)";
            }

            if (isCircularRef && isSelfRef)
            {
                text = "!(self reference)";
            }

            var command = new CellTextCommand(cell, cell.Text, text);
            command.Execute();
            this.undoStack.Push(command);
            this.redoStack.Clear();
            this.StackChanged?.Invoke(this, new PropertyChangedEventArgs(" "));
        }

        /// <summary>
        /// Set cell background color with command.
        /// </summary>
        /// <param name="cell">Cell in spreadsheet.</param>
        /// <param name="bgColor">Background color.</param>
        public void SetBackground(List<Cell> cell, uint bgColor)
        {
            var command = new CellBackgroundCommand(cell, cell[0].BGColor, bgColor);
            command.Execute();
            this.undoStack.Push(command);
            this.redoStack.Clear();
            this.StackChanged?.Invoke(this, new PropertyChangedEventArgs(" "));
        }

        /// <summary>
        /// Fires a command at the top of the undo stack.
        /// </summary>
        public void Undo()
        {
            if (this.undoStack.Count > 0)
            {
                var command = this.undoStack.Pop();
                command.Undo();
                this.redoStack.Push(command);
                this.StackChanged?.Invoke(this, new PropertyChangedEventArgs(" "));
            }
        }

        /// <summary>
        /// Fires a command at the top of the redo stack.
        /// </summary>
        public void Redo()
        {
            if (this.redoStack.Count > 0)
            {
                var command = this.redoStack.Pop();
                command.Redo();
                this.undoStack.Push(command);
                this.StackChanged?.Invoke(this, new PropertyChangedEventArgs(" "));
            }
        }

        /// <summary>
        /// Get the command on the top of the undo stack.
        /// </summary>
        /// <returns>Command on top of the undo stack.</returns>
        public string PeekUndoStack()
        {
            if (this.undoStack.Count != 0)
            {
                var command = this.undoStack.Peek();
                if (command is CellBackgroundCommand)
                {
                    return "Undo background color change";
                }
                else if (command is CellTextCommand)
                {
                    return "Undo text change";
                }
            }

            return "Undo";
        }

        /// <summary>
        /// Get the command on the top of the redo stack.
        /// </summary>
        /// <returns>Command on top of the redo stack.</returns>
        public string PeekRedoStack()
        {
            if (this.redoStack.Count != 0)
            {
                var command = this.redoStack.Peek();
                if (command is CellBackgroundCommand)
                {
                    return "Redo background color change";
                }
                else if (command is CellTextCommand)
                {
                    return "Redo text change";
                }
            }

            return "Redo";
        }

        /// <summary>
        /// Gets the stack counts for undo and redo.
        /// </summary>
        /// <returns>undo and redo stack counts.</returns>
        public (int, int) GetStackCounts()
        {
            return (this.undoStack.Count, this.redoStack.Count);
        }

        /// <summary>
        /// Saves spreadsheet to XML file.
        /// </summary>
        /// <param name="stream">file path stream.</param>
        public void Save(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Async = true;
            settings.Indent = true;

            uint defaultBgColor = 4294967295;

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                for (int i = 0; i < this.rowCount; i++)
                {
                    for (int j = 0; j < this.columnCount; j++)
                    {
                        Cell cell = this.cellMatrix[i, j];

                        if (cell.BGColor != defaultBgColor || cell.Text != string.Empty) // only add altered cells.
                        {
                            writer.WriteStartElement("cell");
                            writer.WriteAttributeString("name", cell.CellName);

                            if (cell.BGColor != defaultBgColor)
                            {
                                writer.WriteStartElement("bgcolor");
                                writer.WriteString(cell.BGColor.ToString());
                                writer.WriteEndElement();
                            }

                            if (cell.Text != string.Empty)
                            {
                                writer.WriteStartElement("text");
                                writer.WriteString(cell.Text);
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Loads spreadsheet xml file.
        /// </summary>
        /// <param name="stream">file path of xml file.</param>
        public void Load(Stream stream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;
            this.suppressExceptions = true;

            // update cells from xml. https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.read?view=net-8.0
            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                reader.ReadStartElement();

                // reset cells to default values
                for (int i = 0; i < this.rowCount; i++)
                {
                    for (int j = 0; j < this.columnCount; j++)
                    {
                        Cell cell = this.GetCell(i, j);
                        cell.Text = string.Empty;
                        cell.BGColor = 4294967295;
                    }
                }

                // TODO: make this more resilient to differing xml files
                Debug.WriteLine($"Loading spreadsheet...");
                while (reader.Read())
                {
                    if (reader.IsStartElement("cell"))
                    {
                        string name = reader.GetAttribute("name");
                        int column = name[0] - 'A';
                        int row = int.Parse(name.Substring(1));
                        Cell cell = null;
                        if (row >= 0 && row <= this.RowCount && column >= 0 && column <= this.ColumnCount)
                        {
                            cell = this.cellMatrix[row - 1, column];
                        }

                        while (reader.Read() && reader.IsStartElement())
                        {
                            if (reader.Name == "bgcolor")
                            {
                                string bgColor = reader.ReadElementContentAsString();
                                if (bgColor != null)
                                {
                                    uint uintBgColor = uint.Parse(bgColor);
                                    cell.BGColor = uintBgColor;
                                }
                            }
                            else if (reader.Name == "text")
                            {
                                string text = reader.ReadElementContentAsString();
                                if (text != null)
                                {
                                    cell.Text = text;
                                }
                            }
                            else
                            {
                                reader.Skip();
                            }
                        }
                    }
                }
            }

            // reset current spreadsheet
            this.undoStack.Clear();
            this.redoStack.Clear();
            this.StackChanged?.Invoke(this, new PropertyChangedEventArgs(" "));
            this.suppressExceptions = false;
        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Cell Property Changed");
            bool badReference = false;
            bool selfReference = false;
            bool circularReference = false;

            var cell = (Cell)sender;
            if (e.PropertyName == nameof(Cell.Text)) // using nameof() to prevent potential errors
            {
                if (cell.Text == string.Empty || cell.Text == null)
                {
                    cell.Value = string.Empty;
                }
                else if (cell.Text[0] != '=') // not an expression
                {
                    bool isdigit = true;
                    for (int i = 0; i < cell.Text.Length; i++)
                    {
                        if (!char.IsDigit(cell.Text[i]))
                        {
                            isdigit = false;
                        }
                    }

                    if (isdigit)
                    {
                        cell.Value = cell.Text;
                        Debug.WriteLine($"value now equals {cell.Value}");
                    }
                    else if (cell.Text == "!(circular reference)")
                    {
                        cell.Value = "!(circular reference)";
                    }
                    else if (cell.Text == "!(self reference)")
                    {
                        cell.Value = "!(self reference)";
                    }
                    else
                    {
                        cell.Value = "!(bad reference)";
                    }
                }
                else if (!string.IsNullOrEmpty(cell.Text) && cell.Text[0] == '=') // expression handling
                {
                    SpreadsheetEngine.ExpressionTree.ExpressionTree tree = null; // Declare tree outside

                    try
                    {
                        tree = new SpreadsheetEngine.ExpressionTree.ExpressionTree(cell.Text.Substring(1));
                    }
                    catch (FormatException ex)
                    {
                        tree = new SpreadsheetEngine.ExpressionTree.ExpressionTree(string.Empty);
                        if (!this.suppressExceptions)
                        {
                            this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        tree = new SpreadsheetEngine.ExpressionTree.ExpressionTree(string.Empty);
                        if (!this.suppressExceptions)
                        {
                            this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                        }
                    }

                    Debug.WriteLine("Expression Tree Created");
                    Debug.WriteLine($"Setting expression tree for cell [{cell.RowIndex}, {cell.ColumnIndex}]");
                    cell.SetExpressionTree(tree);
                    Debug.WriteLine($"Expression tree set: {cell.GetExpressionTree()}");

                    // Check if the tree is created correctly
                    if (cell.GetExpressionTree() == null)
                    {
                        Debug.WriteLine($"Warning: Tree is null for cell [{cell.RowIndex}, {cell.ColumnIndex}] after setting.");
                    }

                    // Update the variables in the trees' dictionary.
                    for (int i = 0; i < this.rowCount; i++) // TODO: test if this is atcually right.
                    {
                        for (int j = 0; j < this.columnCount; j++)
                        {
                            if (this.cellMatrix[i, j].Value != string.Empty)
                            {
                                string cellName = $"{(char)('A' + j)}{i + 1}";
                                string cellValue = this.cellMatrix[i, j].Value;

                                Debug.WriteLine($"Name: {cellName} Value: {cellValue}");

                                if (cellValue != "!(bad reference)" && cellValue != "!(self reference)" && cellValue != "!(circular reference)")
                                {
                                    tree.SetVariable(cellName, double.Parse(cellValue));
                                }
                            }
                        }
                    }

                    // Manage cell dependancies for updates.
                    Dictionary<string, double> variables = tree.GetVaribles();
                    cell.ClearDependencies();
                    Debug.WriteLine("Cell references cleared. Adding new ones...");

                    foreach (var varName in tree.GetExpressionVariables())
                    {
                        int col = varName[0] - 'A';
                        if (varName.Length > 1 && char.IsLetter(varName[1]))
                        {
                            badReference = true;
                            continue;
                        }

                        int row = int.Parse(varName.Substring(1)) - 1;

                        Cell cellItDependsOn = this.GetCell(row, col);

                        if (cellItDependsOn != null)
                        {
                            if (cellItDependsOn == cell)
                            {
                                selfReference = true;
                                continue;
                            }

                            cell.AddDependant(cellItDependsOn);
                            cellItDependsOn.AddReference(cell);

                            Debug.WriteLine($"Cell[{cell.RowIndex}, {cell.ColumnIndex}] depends on Cell[{row}, {col}]");
                            Debug.WriteLine($"Cell[{row}, {col}] now references Cell[{cell.RowIndex}, {cell.ColumnIndex}]");
                        }
                        else
                        {
                            Debug.WriteLine($"Warning: Variable '{varName}' does not correspond to an existing cell.");
                        }
                    }

                    try
                    {
                        cell.Value = tree.Evaluate().ToString();
                    }
                    catch (FormatException ex) // unknown operator.
                    {
                        if (!this.suppressExceptions)
                        {
                            Debug.WriteLine("FormatException Caught Spreadsheet");
                            this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                        }
                    }
                    catch (DivideByZeroException ex)
                    {
                        if (!this.suppressExceptions)
                        {
                            Debug.WriteLine("DivideByZeroException Caught Spreadsheet");
                            this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                        }
                    }
                    catch (OverflowException ex)
                    {
                        if (!this.suppressExceptions)
                        {
                            Debug.WriteLine("OverflowException Caught Spreadsheet");
                            this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                        }
                    }
                }
            }

            if (badReference && cell.Value != "!(circular reference)" && cell.Value != "!(self reference)")
            {
                cell.Value = "!(bad reference)";
            }

            cell.UpdateDependents();
            this.CellPropertyChanged?.Invoke(sender, e); // this can now be subscribed to from the UI layer
        }

        private bool SelfReference(Cell cell, string text)
        {
            if (!string.IsNullOrEmpty(text) && text[0] != '=') // not an expression
            {
                return false;
            }

            // Run a mock of a cell dependancy update. This prevents an infinate loop later on.
            SpreadsheetEngine.ExpressionTree.ExpressionTree mockTree = new SpreadsheetEngine.ExpressionTree.ExpressionTree(text.Substring(1));
            HashSet<Cell> mockDependancies = new HashSet<Cell>();

            // Find all future cells the current cell will depend on.
            foreach (var variable in mockTree.GetExpressionVariables())
            {
                int col = variable[0] - 'A';

                if (char.IsLetter(variable[1]))
                {
                    return false;
                }

                int row = int.Parse(variable.Substring(1)) - 1;

                Cell dependentCell = this.GetCell(row, col);
                if (dependentCell != null)
                {
                    mockDependancies.Add(dependentCell);
                }
            }

            if (mockDependancies.Contains(cell))
            {
                return true;
            }

            cell.ClearDependencies();

            return false;
        }

        // Source: https://stackoverflow.com/questions/28502403/how-to-find-circular-references-in-a-tree
        // Source: https://stackoverflow.com/questions/46484725/most-efficient-way-of-finding-circular-references-in-list
        private bool CircularReferenceDFS(Cell cell, HashSet<Cell> visited, HashSet<Cell> path)
        {
            if (path.Contains(cell)) // cell is already in the current DFS path. Circular reference.
            {
                return true;
            }

            if (visited.Contains(cell)) // base case. No cycle.
            {
                return false;
            }

            visited.Add(cell);
            path.Add(cell);

            // DFS cell references
            foreach (var reference in cell.GetReferences())
            {
                if (this.CircularReferenceDFS(reference, visited, path))
                {
                    return true;
                }
            }

            path.Remove(cell); // Fully explored cell and its refs/dependents.
            return false;
        }

        // Catch a circular reference before it becomes a problem by mocking.
        private bool CircularReference(Cell cell, string newText)
        {
            if (!string.IsNullOrEmpty(newText) && newText[0] != '=') // not an expression
            {
                return false;
            }

            // Run a mock of a cell dependancy update. This prevents an infinate loop later on.
            SpreadsheetEngine.ExpressionTree.ExpressionTree mockTree = new SpreadsheetEngine.ExpressionTree.ExpressionTree(newText.Substring(1));
            HashSet<Cell> mockDependancies = new HashSet<Cell>();

            // Find all future cells the current cell will depend on.
            foreach (var variable in mockTree.GetExpressionVariables())
            {
                int col = variable[0] - 'A';

                if (char.IsLetter(variable[1]))
                {
                    return false;
                }

                int row = int.Parse(variable.Substring(1)) - 1;

                Cell dependentCell = this.GetCell(row, col);
                if (dependentCell != null)
                {
                    mockDependancies.Add(dependentCell);
                }
            }

            // Clear the current cell dependancies and update future dependents / references.
            cell.ClearDependencies();
            foreach (var dependent in mockDependancies)
            {
                cell.AddDependant(dependent);
                dependent.AddReference(cell);
            }

            HashSet<Cell> visited = new HashSet<Cell>();
            HashSet<Cell> path = new HashSet<Cell>();
            bool foundCircularReference = this.CircularReferenceDFS(cell, visited, path);
            cell.ClearDependencies(); // reset

            return foundCircularReference;
        }

        // Creating a class that inherits the Cell class to instantiate the abstract base class.
        private class SpreadsheetCell : Cell
        {
            public SpreadsheetCell(int row, int column)
                : base(row, column)
            {
            }
        }
    }
}
