//-----------------------------------------------------------------------
// <copyright file="Cell.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CS8612
#pragma warning disable CS8618

namespace SpreadsheetEngine
{
    using System.ComponentModel;
    using System.Diagnostics;
    using SpreadsheetEngine.CustomEvents;
    using SpreadsheetEngine.ExpressionTree;

    /// <summary>
    /// Abstract Cell class.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// row index.
        /// </summary>
        private readonly int rowIndex;

        /// <summary>
        /// col index.
        /// </summary>
        private readonly int columnIndex;

        // cells that this cell depends on
        private List<Cell> dependantCells = new List<Cell>();

        // cells that refernce this cell
        private List<Cell> referencedCells = new List<Cell>();

        /// <summary>
        /// text on the UI layer.
        /// </summary>
        private string text;

        /// <summary>
        /// background color of the cell in the UI layer.
        /// </summary>
        private uint bgColor;

        /// <summary>
        /// value in the spreadsheet engine.
        /// </summary>
        private string value;

        private ExpressionTree.ExpressionTree tree;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowIndex">Row Index.</param>
        /// <param name="columnIndex">Column Index.</param>
        public Cell(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.text = string.Empty;
            this.value = string.Empty;
            this.BGColor = 0xFFFFFFFF;
        }

        /// <summary>
        /// Property Changed Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event handler for exceptions.
        /// </summary>
        public event EventHandler<ExceptionOccurredEventArgs> ExceptionOccurred;

        /// <summary>
        /// Gets dependents for a cell.
        /// </summary>
        public IReadOnlyList<Cell> Dependants => this.dependantCells.AsReadOnly();

        /// <summary>
        /// Gets refernces a cell has.
        /// </summary>
        public IReadOnlyList<Cell> References => this.referencedCells.AsReadOnly();

        /// <summary>
        /// Gets row index.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets column index.
        /// </summary>
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets or sets Text index.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value == this.Text)
                {
                    return;
                }

                this.text = value;
                Debug.WriteLine($"Cell text set to: {this.text}");

                // Fire Property Changed Event. ?.Invoke ensures there are subscribers to listen
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
            }
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }

            internal set // ensure only the Spreadsheet Class can set this, but no others can.
            {
                if (value == this.value)
                {
                    return;
                }

                this.value = value;

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        /// <summary>
        /// Gets or sets background color of a cell.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.bgColor;
            }

            set
            {
                if (value == this.bgColor)
                {
                    return;
                }

                this.bgColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.bgColor)));
            }
        }

        /// <summary>
        /// Gets the cells spreadsheet name like "A1, B5 etc...".
        /// </summary>
        public string CellName
        {
            get
            {
                char columnLetter = (char)('A' + this.ColumnIndex);
                return $"{columnLetter}{this.RowIndex + 1}";
            }
        }

        /// <summary>
        /// Gets cell dependents list.
        /// </summary>
        /// <returns>cell dependents list.</returns>
        public List<Cell> GetDependents()
        {
            return this.dependantCells;
        }

        /// <summary>
        /// Gets cell references list.
        /// </summary>
        /// <returns>cell references list.</returns>
        public List<Cell> GetReferences()
        {
            return this.referencedCells;
        }

        /// <summary>
        /// Adds a dependant cell to this cell's list.
        /// </summary>
        /// <param name="cell">The cell to add as a dependant.</param>
        public void AddDependant(Cell cell)
        {
            if (!this.dependantCells.Contains(cell))
            {
                this.dependantCells.Add(cell);
                cell.AddReference(this);
            }
        }

        /// <summary>
        /// Sets an expresiosn tree for a cell.
        /// </summary>
        /// <param name="tree">Expression tree.</param>
        public void SetExpressionTree(ExpressionTree.ExpressionTree tree)
        {
            this.tree = tree;
        }

        /// <summary>
        /// Gets an expression tree from a cell.
        /// </summary>
        /// <returns>ExpressionTree.</returns>
        public ExpressionTree.ExpressionTree GetExpressionTree()
        {
            return this.tree;
        }

        /// <summary>
        /// Adds this cell to another cell's reference list.
        /// </summary>
        /// <param name="cell">Add reference for cell.</param>
        public void AddReference(Cell cell)
        {
            if (!this.referencedCells.Contains(cell))
            {
                this.referencedCells.Add(cell);
            }
        }

        /// <summary>
        /// Clears all dependencies and references.
        /// </summary>
        public void ClearDependencies()
        {
            foreach (var cell in this.dependantCells)
            {
                cell.RemoveReference(this);
            }

            this.dependantCells.Clear();
        }

        /// <summary>
        /// Recursively updates Dependants of the current cell and re-evaluates the cell-specific trees.
        /// </summary>
        public void UpdateDependents()
        {
            Debug.WriteLine($"Updating dependents for cell [{this.RowIndex}, {this.ColumnIndex}], current value: {this.Value}");
            foreach (var dependent in this.referencedCells)
            {
                Debug.WriteLine($"Notifying dependent cell at [{dependent.RowIndex}, {dependent.ColumnIndex}]");

                try
                {
                    // Check if the tree is null before evaluating
                    if (dependent.tree != null)
                    {
                        // re-set variable in the other cells variables dicts
                        dependent.tree.SetVariable(this.CellName, double.Parse(this.value));

                        dependent.value = dependent.tree.Evaluate().ToString();

                        dependent.PropertyChanged?.Invoke(dependent, new PropertyChangedEventArgs(nameof(dependent.Value)));
                    }
                    else
                    {
                        Debug.WriteLine($"Warning: Tree is null for cell [{dependent.RowIndex}, {dependent.ColumnIndex}] during evaluation.");
                    }
                }
                catch (FormatException ex)
                {
                    this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                }
                catch (DivideByZeroException ex)
                {
                    this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                }
                catch (OverflowException ex)
                {
                    this.ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(ex.Message));
                }

                dependent.UpdateDependents(); // Recurse through all dependent cells.
            }
        }

        /// <summary>
        /// Removes a dependant cell.
        /// </summary>
        /// <param name="cell">The cell to remove.</param>
        private void RemoveDependant(Cell cell)
        {
            if (this.dependantCells.Remove(cell))
            {
                cell.RemoveReference(this);
            }
        }

        /// <summary>
        /// Removes this cell from another cell's reference list.
        /// </summary>
        /// <param name="cell">Remove cell reference.</param>
        private void RemoveReference(Cell cell)
        {
            this.referencedCells.Remove(cell);
        }
    }
}
