using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Finger_Print_Based_Voting_System
{
    public partial class AddElection : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);

        public AddElection()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = dateTimePicker1.Value.Date.ToString("dd/MM/yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the Election ID", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter the Election Name", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Enter the Election Date", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Election (ElectionID, ElectionName, Date) VALUES (@ElectionID, @ElectionName, @ElectionDate)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ElectionID", textBox1.Text);
                    cmd.Parameters.AddWithValue("@ElectionName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@ElectionDate", textBox3.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Election Details Added!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

