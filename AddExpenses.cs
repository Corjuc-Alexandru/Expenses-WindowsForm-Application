using System;
using System.IO;
using System.Windows.Forms;

namespace Expenses
{
    public partial class AddExpenses : Form
    {
        public AddExpenses()
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
            //Open form 'List Expeneses'
            this.Hide();
            var newForm = new ListExpenses();
            newForm.Closed += (s, args) => this.Close();
            newForm.Show();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //catch wrong inputs
            try
            {
                DateTime formaterrordate = DateTime.Parse(textBox5.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please check if you have entered " +
                    "the correct format of Date.", "Wrong Date", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            try
            {
                decimal formaterrorprice = decimal.Parse(textBox6.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please check if you have entered " +
                    "the correct format of Price.", "Wrong Price", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            //Assign values
            string name = textBox4.Text;
            DateTime date = DateTime.Parse(textBox5.Text);
            decimal price = decimal.Parse(textBox6.Text);
            // Creating a file
            string myfile = @"database.txt";
            // Checking the above file
            if (!File.Exists(myfile))
            {
                // Creating the same file if it doesn't exist
                using (StreamWriter sw = File.CreateText(myfile))
                {
                    //¢ - is important for make columns in text file
                    sw.WriteLine("Name¢Date¢Price");
                }
            }
            // Appending the given texts
            string[] lines = System.IO.File.ReadAllLines(myfile);
            if (lines[0] != null)
            {
                lines[0] = name + "¢" + date.ToShortDateString() + "¢" + price;
            }
            using (StreamWriter sw = System.IO.File.AppendText(myfile))
            {
                sw.WriteLine(lines[0]);
            }
            //Debug if value are saved in txt file
            MessageBox.Show("Was successfully added!", "Add Expenses", MessageBoxButtons.OK,
                MessageBoxIcon.Information );
            //Clear text boxes
            textBox4.Text = String.Empty;
            textBox5.Text = String.Empty;
            textBox6.Text = String.Empty;
        }
    }
}