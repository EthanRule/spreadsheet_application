namespace SpreadsheetEngine
{
    using System.ComponentModel;

    /// <summary>
    /// Creates spreadsheet of cells.
    /// </summary>
    public class Spreadsheet
    {
        private readonly int columnCount;
        private readonly int rowCount;
        private Cell[,] cellMatrix;

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

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Cell.Text)) // using nameof() to prevent potential errors
            {
                var cell = (Cell)sender;
                if (!string.IsNullOrEmpty(cell.Text) && cell.Text[0] != '=')
                {
                    cell.Value = cell.Text;
                }
                else if (!string.IsNullOrEmpty(cell.Text) && cell.Text[0] == '=')
                {
                    // copy
                    char colChar = cell.Text[1];
                    int columnNumber = 0;
                    if (colChar >= 'A' && colChar <= 'Z')
                    {
                        columnNumber = colChar - 'A';
                    }

                    string row = string.Empty;
                    int rowNumber = 0;
                    for (int i = 2; i < cell.Text.Length; i++)
                    {
                        row += cell.Text[i];
                    }

                    rowNumber = int.Parse(row) - 1;

                    if (rowNumber >= 0 && columnNumber >= 0 && rowNumber < this.rowCount && columnNumber < this.columnCount)
                    {
                        cell.Value = this.cellMatrix[rowNumber, columnNumber].Text;
                    }
                }
            }

            this.CellPropertyChanged?.Invoke(sender, e); // this can now be subscribed to from the UI layer
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
