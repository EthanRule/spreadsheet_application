using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    internal class Spreadsheet
    {
        private Cell[,] cellMatrix;
        private readonly int columnCount;
        private readonly int rowCount;

        public event PropertyChangedEventHandler CellPropertyChanged;

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
                    this.cellMatrix[i, j].PropertyChanged += OnCellPropertyChanged; // subscribe to cell
                }
            }

        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CellPropertyChanged?.Invoke(sender, e); // this can now be subscribed to from the UI layer
        }

        public Cell GetCell(int row, int col)
        {
            if(row <  0 || col < 0 || row >= this.rowCount || col >= this.columnCount)
            {
                return null;
            }

            return this.cellMatrix[row, col];
        }

        public int ColumnCount
        {
            get { return this.columnCount; }
        }

        public int RowCount
        {
            get { return this.rowCount; }
        }

        // Creating a class that inherits the Cell class to instantiate the abstract class.
        private class SpreadsheetCell : Cell
        {
            public SpreadsheetCell(int row, int column) : base(row, column) { }
        }
    }
}
