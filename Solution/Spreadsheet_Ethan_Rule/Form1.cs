using SpreadsheetEngine;
using System.ComponentModel;

namespace Spreadsheet_Ethan_Rule
{
    public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet;

        public Form1()
        {
            InitializeComponent();
            InitializeDataGrid();
            spreadsheet = new Spreadsheet(26, 50);
            spreadsheet.CellPropertyChanged += OnCellPropertyChanged;
        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (Cell)sender;

            int row = cell.RowIndex;
            int col = cell.ColumnIndex;

            dataGridView1.Rows[row].Cells[col].Value = cell.Value;
        }

        private void InitializeDataGrid()
        {
            dataGridView1.Columns.Clear(); // Clear pre existing columns

            // add columns A - Z
            for (char i = 'A'; i <= 'Z'; i++)
            {
                string letter = i.ToString();
                dataGridView1.Columns.Add(letter, letter);
            }

            // add rows and row headers. Also adjust default header width.
            for (int i = 1; i < 50; i++)
            {
                string number = i.ToString();
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i - 1].HeaderCell.Value = number;
            }
            dataGridView1.RowHeadersWidth = 75;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // set text in 50 random cells
            Random rand = new Random();

            for ( int i = 0; i < 50; i++)
            {
                int randomCol = rand.Next(0, 26);
                int randomRow = rand.Next(0, 50);
                Cell cell = spreadsheet.GetCell(randomRow, randomCol);

                if (cell != null)
                {
                    cell.Text = "I love C#!";
                }
            }

            // set text in col B
            for (int i = 0; i < 50; i++)
            {
                Cell cell = spreadsheet.GetCell(i, 1);
                if (cell != null)
                {
                    cell.Text = $"This is cell B{i + 1}";
                }
            }

            // add equation to every cell in col A
            for (int i = 0; i < 50; i++)
            {
                Cell cell = spreadsheet.GetCell(i, 0);
                if (cell != null)
                {
                    cell.Text = $"=B{i}";
                }
            }

        }
    }
}
