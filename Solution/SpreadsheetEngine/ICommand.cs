//-----------------------------------------------------------------------
// <copyright file="ICommand.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SpreadsheetEngine
{
    /// <summary>
    /// Interface for commands.
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Undo command.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo command.
        /// </summary>
        void Redo();

        /// <summary>
        /// Execute command.
        /// </summary>
        void Execute();
    }
}
