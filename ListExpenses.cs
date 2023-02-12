using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Expenses
{
    public partial class ListExpenses : Form
    {
        public ListExpenses()
        {
            InitializeComponent();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //Open form 'Add Expeneses'
            this.Hide();
            var thisForm = new AddExpenses();
            thisForm.Closed += (s, args) => this.Close();
            thisForm.Show();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            //Open form 'List Expenses'
            this.Hide();
            var newForm = new ListExpenses();
            newForm.Closed += (s, args) => this.Close();
            newForm.Show();
        }
        //declare sumAll for calculate Total, and file to get values
        decimal sumAll;
        readonly string myfile = @"database.txt";
        private void Button3_Click(object sender, EventArgs e)
        {
            sumAll = 0; // set sumAll = 0 to not double total result when you click the button again
            //catch error for not existing file
            try
            {
                System.IO.StreamReader testfile = new System.IO.StreamReader(myfile);
                testfile.Close();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("No input text in table."
                  , "Table not found",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Read text file for values
            System.IO.StreamReader file = new System.IO.StreamReader(myfile);
            //Add Column Names in Table from txt file
            //¢ - is important for read in columns from text file
            string[] columnnames = file.ReadLine().Split('¢');
            DataTable dt = new DataTable();
            foreach (string c in columnnames)
            {
                dt.Columns.Add(c);
            }
            //Add Row with value in table from txt file
            string newline;
            while ((newline = file.ReadLine()) != null)
            {
                DataRow dr = dt.NewRow();
                string[] values = newline.Split('¢');
                for (int i = 0; i < values.Length; i++)
                {
                    dr[i] = values[i];
                }
                dt.Rows.Add(dr);
            }
            file.Close();
            //View table in data grid from txt file
            dataGridView1.DataSource = dt;
            //set font style for table
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial Black", 16);
            this.dataGridView1.DefaultCellStyle.Font = new Font("Arial", 14);
            //Add a total row 
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                sumAll += decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
            }
            //Show result with currency
            string totalResult = string.Format(CultureInfo.CurrentCulture, "{0:C}", sumAll);
            dt.Rows.Add("Total", null, totalResult);
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            //Set alert for edit text
            string message = "Do you want to edit this text?";
            string title = "Edit Text";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //Edit a specific text from table
                int number_of_rows = dataGridView1.CurrentCell.RowIndex;
                int fileline = number_of_rows + 2;
                if (dataGridView1.CurrentCell.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex];
                    var editedName = row.Cells[0].Value.ToString();
                    var editedDate = row.Cells[1].Value.ToString();
                    var editedPrice = row.Cells[2].Value.ToString();
                    var editedline = editedName + "¢" + editedDate + "¢" + editedPrice;
                    void lineChanger(string newText, string fileName, int line_to_edit)
                    {
                        string[] arrLine = File.ReadAllLines(fileName);
                        arrLine[line_to_edit - 1] = newText;
                        File.WriteAllLines(fileName, arrLine);
                    }
                    lineChanger(editedline, myfile, fileline);
                }
                Button3.PerformClick();
                //Debug if text was edited
                MessageBox.Show("Text was edited.", "Edit text", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {

            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            //Set alert for delete row
            string message = "Do you want to delete this row?";
            string title = "Delete Row";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //Delete a specific row from table
                int number_of_rows = dataGridView1.CurrentCell.RowIndex;
                int fileline = number_of_rows + 2;
                string tempFile = Path.GetTempFileName();
                using (var sr = new StreamReader(myfile))
                using (var sw = new StreamWriter(tempFile))
                {
                    string line;
                    int line_number = 0;
                    int line_to_delete = fileline;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line_number++;
                        if (line_number == line_to_delete)
                            continue;
                        sw.WriteLine(line);
                    }
                }
                File.Delete(myfile);
                File.Move(tempFile, myfile);
                Button3.PerformClick();
                //Debug if row was deleted.
                MessageBox.Show("Row was deleted.", "Delete Row", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {

            }
        }
    }
}