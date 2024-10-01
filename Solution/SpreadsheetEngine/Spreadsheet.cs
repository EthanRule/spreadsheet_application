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

        public event PropertyChangedEventHandler CellPropertyChanged;

        public Spreadsheet(int rows, int columns)
        {
            this.cellMatrix = new Cell[rows, columns];

            // subscribe to cells property changed
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.cellMatrix[i, j].PropertyChanged += OnCell
                }
            }

        }
    }
}
