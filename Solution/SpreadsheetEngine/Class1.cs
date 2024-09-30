using System;
using System.ComponentModel;

namespace SpreadsheetEngine
{
    public abstract class Cell
    {
        protected readonly int rowIndex;
        protected readonly int columnIndex;
        protected string text;

        public Cell(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            text = string.Empty;
        }

        public int RowIndex
        { 
           get { return this.rowIndex; }
        }

        public int ColumnIndex
        {
           get { return this.columnIndex; }
        }

        public string Text
        {
            get { return this.text; }

            set
            {



                // Fire Property Changed Event

            }

        }

    }
}
