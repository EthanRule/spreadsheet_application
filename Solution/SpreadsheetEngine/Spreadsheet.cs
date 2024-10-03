using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SpreadsheetEngine
{
    public class Spreadsheet
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
                        columnNumber =  colChar - 'A';
                    }

                    string row = "";
                    int rowNumber = 0;
                    for (int i = 2;i < cell.Text.Length; i++)
                    {
                        row += cell.Text[i];
                    }
                    rowNumber = int.Parse(row) - 1;


                    if (rowNumber >= 0 && columnNumber >= 0 && rowNumber < this.rowCount && columnNumber < this.columnCount)
                    {
                        cell.Value = cellMatrix[rowNumber, columnNumber].Text;
                    }
                }
            }


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

        // Creating a class that inherits the Cell class to instantiate the abstract base class.
        private class SpreadsheetCell : Cell
        {
            public SpreadsheetCell(int row, int column) : base(row, column) { }
        }
    }
}
