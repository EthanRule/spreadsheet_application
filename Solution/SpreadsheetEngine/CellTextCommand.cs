//-----------------------------------------------------------------------
// <copyright file="CellTextCommand.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine
{
    /// <summary>
    /// Cell Text Command.
    /// </summary>
    internal class CellTextCommand : ICommand
    {
        private Cell cell;
        private string prevText;
        private string newText;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellTextCommand"/> class.
        /// </summary>
        /// <param name="cell">Cell in the spreadsheet.</param>
        /// <param name="prevText">Previous Text.</param>
        /// <param name="newText">New Text.</param>
        public CellTextCommand(Cell cell, string prevText, string newText)
        {
            this.cell = cell;
            this.prevText = prevText;
            this.newText = newText;
        }

        /// <summary>
        /// Sets the cell text to the previous text.
        /// </summary>
        public void Undo()
        {
            this.cell.Text = this.prevText;

            this.cell.ClearDependencies(); // Force a dependancy reset.
        }

        /// <summary>
        /// Sets the cell text to the new text.
        /// </summary>
        public void Execute()
        {
            this.cell.Text = this.newText;
        }

        /// <summary>
        /// Sets the cell text to the new text.
        /// </summary>
        public void Redo()
        {
            this.Execute();
        }
    }
}
