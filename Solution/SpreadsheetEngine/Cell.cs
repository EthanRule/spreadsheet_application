//-----------------------------------------------------------------------
// <copyright file="Cell.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine
{
    using System;
    using System.ComponentModel;

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

        /// <summary>
        /// text on the UI layer.
        /// </summary>
        private string text;

        /// <summary>
        /// value in the spreadsheet engine.
        /// </summary>
        private string value;

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
        }

        /// <summary>
        /// Property Changed Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}
