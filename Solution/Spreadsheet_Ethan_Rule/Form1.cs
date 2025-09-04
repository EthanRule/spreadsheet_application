//-----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CS8622
#pragma warning disable CS8600
#pragma warning disable CS8604

namespace Spreadsheet_Ethan_Rule
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using SpreadsheetEngine;
    using SpreadsheetEngine.CustomEvents;

    /// <summary>
    /// UI Layer WinForm.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.InitializeDataGrid();

            this.spreadsheet = new Spreadsheet(50, 26);
            this.spreadsheet.CellPropertyChanged += this.OnCellPropertyChanged;
            this.spreadsheet.ExceptionOccurred += this.OnException;
            this.spreadsheet.StackChanged += this.OnStackChanged;

            this.dataGridView1.CellBeginEdit += this.OnCellBeginEdit;
            this.dataGridView1.CellEndEdit += this.OnCellEndEdit;
        }

        private void OnException(object sender, ExceptionOccurredEventArgs e)
        {
            Debug.WriteLine($"An error occurred: {e.Message}");
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (Cell)sender;
            int row = cell.RowIndex;
            int col = cell.ColumnIndex;
            uint bgColor = cell.BGColor;

            this.dataGridView1.Rows[row].Cells[col].Style.BackColor = Color.FromArgb((int)bgColor);
            this.dataGridView1.Rows[row].Cells[col].Value = cell.Value;
        }

        private void OnCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            Cell cell = this.spreadsheet.GetCell(row, col);

            this.dataGridView1.Rows[row].Cells[col].Value = cell.Text; // display orignal text value for easier expression editing.
            Debug.WriteLine("BeginEdit");
        }

        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            string newText = this.dataGridView1.Rows[row].Cells[col].Value?.ToString();
            Cell cell = this.spreadsheet.GetCell(row, col);
            if (cell != null)
            {
                this.spreadsheet.SetText(cell, newText);
                this.dataGridView1.Rows[row].Cells[col].Value = cell.Value;
            }

            Debug.WriteLine("EndEdit");
        }

        private void OnStackChanged(object sender, PropertyChangedEventArgs e)
        {
            string redoButtonText = this.spreadsheet.PeekRedoStack();
            string undoButtonText = this.spreadsheet.PeekUndoStack();

            if (redoButtonText == "Redo")
            {
                this.redoToolStripMenuItem.ForeColor = Color.Gray;
                this.redoToolStripMenuItem.Text = redoButtonText;
            }
            else
            {
                this.redoToolStripMenuItem.ForeColor = Color.Black;
                this.redoToolStripMenuItem.Text = redoButtonText;
            }

            if (undoButtonText == "Undo")
            {
                this.undoToolStripMenuItem.ForeColor = Color.Gray;
                this.undoToolStripMenuItem.Text = undoButtonText;
            }
            else
            {
                this.undoToolStripMenuItem.ForeColor = Color.Black;
                this.undoToolStripMenuItem.Text = undoButtonText;
            }
        }

        private void InitializeDataGrid()
        {
            this.dataGridView1.Columns.Clear(); // Clear pre existing columns

            // add columns A - Z
            for (char i = 'A'; i <= 'Z'; i++)
            {
                string letter = i.ToString();
                this.dataGridView1.Columns.Add(letter, letter);
            }

            // add rows and row headers. Also adjust default header width.
            for (int i = 1; i <= 50; i++)
            {
                string number = i.ToString();
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = number;
            }

            this.dataGridView1.RowHeadersWidth = 75;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // set text in 50 random cells
            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                int randomCol = rand.Next(0, 26);
                int randomRow = rand.Next(0, 50);
                Cell cell = this.spreadsheet.GetCell(randomRow, randomCol);

                if (cell != null)
                {
                    cell.Text = "1";
                }
            }

            // set text in col B
            for (int i = 0; i < 50; i++)
            {
                Cell cell = this.spreadsheet.GetCell(i, 1);
                if (cell != null)
                {
                    cell.Text = $"{i + 1}";
                }
            }

            // add equation to every cell in col A
            for (int i = 0; i < 50; i++)
            {
                Cell cell = this.spreadsheet.GetCell(i, 0);
                if (cell != null)
                {
                    cell.Text = $"=B{i + 1}";
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
        }

        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog.Color;
                uint colorUINT = (uint)color.ToArgb();

                List<Cell> cells = new List<Cell>();

                foreach (DataGridViewCell selected in this.dataGridView1.SelectedCells)
                {
                    int row = selected.RowIndex;
                    int col = selected.ColumnIndex;
                    Cell cell = this.spreadsheet.GetCell(row, col);

                    if (cell != null)
                    {
                        cells.Add(cell);
                    }
                }

                if (cells.Count > 0)
                {
                    this.spreadsheet.SetBackground(cells, colorUINT);
                }
            }
        }

        // save spreadsheet to xml file. Using HW3 as reference
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Users\ethan\GIT\GIT\cpts321-ethan_rule-hws\Solution\xml";
            saveFileDialog1.DefaultExt = "xml";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream?view=net-8.0
                using (FileStream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    this.spreadsheet.Save(stream);
                    stream.Close();
                }
            }
        }

        // load spreadsheet from xml file
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\ethan\GIT\GIT\cpts321-ethan_rule-hws\Solution\xml";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var stream = openFileDialog.OpenFile();
                    this.spreadsheet.Load(stream);
                    stream.Close();
                }
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // un re
            (int, int) stackCounts = this.spreadsheet.GetStackCounts();

            if (stackCounts.Item1 > 0) // can undo
            {
                this.spreadsheet.Undo();
            }
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // un re
            (int, int) stackCounts = this.spreadsheet.GetStackCounts();

            if (stackCounts.Item2 > 0) // can redo
            {
                this.spreadsheet.Redo();
            }
        }
    }
}
