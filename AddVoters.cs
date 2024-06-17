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
    public partial class AddVoters : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);
        public AddVoters()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the Voter ID", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter the Voter Name", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (richTextBox1.Text == "")
            {
                MessageBox.Show("Enter the Voter Address", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Enter the Mobile No", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (label8.Text == "")
            {
                MessageBox.Show("Choose Voter Photo", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (label9.Text == "")
            {
                MessageBox.Show("Choose Voter FingerPrint", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                FileStream fs = new FileStream(label8.Text, FileMode.Open, FileAccess.ReadWrite);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                fs.Flush();
                fs.Dispose();
                fs.Close();

                FileStream fs1 = new FileStream(label9.Text, FileMode.Open, FileAccess.ReadWrite);
                byte[] buffer1 = new byte[fs1.Length];
                fs1.Read(buffer1, 0, (int)fs1.Length);
                fs1.Flush();
                fs1.Dispose();
                fs1.Close();

                string imgname = Path.GetFileNameWithoutExtension(label9.Text);

                con.Open();
                SqlCommand cmd = new SqlCommand("insert into Voters values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + richTextBox1.Text + "',@Photo,@FingerPrint,'"+imgname+"')", con);
                cmd.Parameters.AddWithValue("@Photo", buffer);
                cmd.Parameters.AddWithValue("@FingerPrint", buffer1);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Voters Details Added!");
                button2_Click(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            ofd.ShowDialog();
            label8.Text = ofd.FileName;
            if (ofd.FileName != "")
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                pictureBox1.Image = Image.FromStream(fs);
                fs.Flush();
                fs.Dispose();
                fs.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.tif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.tif";
            ofd.ShowDialog();
            label9.Text = ofd.FileName;
            if (ofd.FileName != "")
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                pictureBox2.Image = Image.FromStream(fs);
                fs.Flush();
                fs.Dispose();
                fs.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            richTextBox1.Clear();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            label8.Text = "";
            label9.Text = "";
        }

        private void AddVoters_Load(object sender, EventArgs e)
        {

        }
    }
}
