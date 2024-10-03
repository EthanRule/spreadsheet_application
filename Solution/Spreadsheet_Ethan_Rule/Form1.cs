//-----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Ethan Rule / WSU ID: 11714155">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Spreadsheet_Ethan_Rule
{
    using System.ComponentModel;
    using SpreadsheetEngine;

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
        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (Cell)sender;
            int row = cell.RowIndex;
            int col = cell.ColumnIndex;

            this.dataGridView1.Rows[row].Cells[col].Value = cell.Value;
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
                    cell.Text = "I love C#!";
                }
            }

            // set text in col B
            for (int i = 0; i < 50; i++)
            {
                Cell cell = this.spreadsheet.GetCell(i, 1);
                if (cell != null)
                {
                    cell.Text = $"This is cell B{i + 1}";
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
    }
}
