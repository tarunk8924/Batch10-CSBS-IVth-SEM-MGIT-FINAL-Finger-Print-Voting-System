using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace Finger_Print_Based_Voting_System
{
    public partial class AddCandidate : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);
        public AddCandidate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the Candidate ID", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter the Candidate Name", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (richTextBox1.Text == "")
            {
                MessageBox.Show("Enter the Candidate Address", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Enter the Election ID", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (label6.Text == "")
            {
                MessageBox.Show("Choose Candidate Photos", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                FileStream fs = new FileStream(label6.Text,FileMode.Open,FileAccess.ReadWrite);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                fs.Flush();
                fs.Dispose();
                fs.Close();

                con.Open();
                SqlCommand cmd = new SqlCommand("insert into candidate values('" + textBox1.Text + "','" + textBox2.Text + "','" + richTextBox1.Text + "',@Photo,'"+comboBox1.Text+"')",con);
                cmd.Parameters.AddWithValue("@Photo",buffer);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Candidate Details Added!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            label6.Text = ofd.FileName;
            if (ofd.FileName != "")
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                pictureBox1.Image = Image.FromStream(fs);
                fs.Flush();
                fs.Dispose();
                fs.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
            pictureBox1.Image = null;
            label6.Text = "";
        }

        private void AddCandidate_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select ElectionID from Election", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            comboBox1.ValueMember = "ElectionID";
            comboBox1.DisplayMember = "ElectionID";
            comboBox1.DataSource = dt;
            con.Close(); 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
