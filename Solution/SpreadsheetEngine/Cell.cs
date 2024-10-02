using System;
using System.ComponentModel;

namespace SpreadsheetEngine
{
    public abstract class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly int rowIndex;
        protected readonly int columnIndex;
        protected string text;
        protected string value;

        public Cell(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.text = string.Empty;
            this.value = string.Empty;
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
                if (value == Text) { return; }
                this.text = value;
                
                // Fire Property Changed Event. ?.Invoke ensures there are subscribers to listen
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public string Value
        {
            get
            {
                if (this.value != null && this.value[0] == '=')
                {
                    // evaluate the expression and return the value
                    double expressionResult = 3.14;

                    return expressionResult.ToString();
                }
                return this.text;
            }

            protected set // ensure only the Spreadsheet Class can set this, but no others can.
            {
                if (value == this.value) { return; }
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }
    }
}
