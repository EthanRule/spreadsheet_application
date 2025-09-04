//-----------------------------------------------------------------------
// <copyright file="CellBackgroundCommand.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine
{
    /// <summary>
    /// Cell Background Command.
    /// </summary>
    internal class CellBackgroundCommand : ICommand
    {
        private List<Cell> cells;
        private uint prevBackground;
        private uint newBackground;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellBackgroundCommand"/> class.
        /// </summary>
        /// <param name="cells">Cells in the spreadsheet.</param>
        /// <param name="prevBackground">Previous background color.</param>
        /// <param name="newBackground">New background color.</param>
        public CellBackgroundCommand(List<Cell> cells, uint prevBackground, uint newBackground)
        {
            this.cells = cells;
            this.prevBackground = prevBackground;
            this.newBackground = newBackground;
        }

        /// <summary>
        /// Undos background color to prevBackground.
        /// </summary>
        public void Undo()
        {
            foreach (Cell cell in this.cells)
            {
                cell.BGColor = this.prevBackground;

                cell.ClearDependencies(); // Force a dependancy reset.
            }
        }

        /// <summary>
        /// Updates background color to the newBackground.
        /// </summary>
        public void Execute()
        {
            foreach (Cell cell in this.cells)
            {
                cell.BGColor = this.newBackground;
            }
        }

        /// <summary>
        /// Updates background color to the new Background.
        /// </summary>
        public void Redo()
        {
            this.Execute();
        }
    }
}
