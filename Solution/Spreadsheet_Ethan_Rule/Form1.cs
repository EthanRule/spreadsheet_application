namespace Spreadsheet_Ethan_Rule
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGrid();
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
    }
}
