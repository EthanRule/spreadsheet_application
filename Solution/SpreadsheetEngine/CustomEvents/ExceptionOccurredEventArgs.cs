//-----------------------------------------------------------------------
// <copyright file="ExceptionOccurredEventArgs.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

// this is just an extended event with a different name.
namespace SpreadsheetEngine.CustomEvents
{
    using System;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionOccurredEventArgs"/> class.
    /// </summary>
    public class ExceptionOccurredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionOccurredEventArgs"/> class.
        /// </summary>
        /// <param name="message">Event message.</param>
        public ExceptionOccurredEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }
    }
}
