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
    public partial class Participants : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);
        public Participants(string participant)
        {
            InitializeComponent();
            label1.Text = participant;            
        }

        public Participants()
        {
        }

        private void Participants_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Select CID as ParticipantID, Name as Participant,Address from candidate where Election='" + label1.Text + "'", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
                string val1 = rows[0].Cells[0].Value.ToString();                
               
                if (val1 != "")
                {
                    con.Open();                    
                    SqlCommand cmd2 = new SqlCommand("select Photo from candidate where CID = '" + val1 + "'", con);
                    byte[] buffer = (byte[])cmd2.ExecuteScalar();

                    Image img = Image.FromStream(new MemoryStream(buffer));
                    Bitmap tImg = new Bitmap(new Bitmap(img));
                    pictureBox1.Image = tImg;
                    con.Close();
                }
            }
            catch
            {
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
                string val1 = rows[0].Cells[0].Value.ToString();

                if (val1 != "")
                {
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("select Photo from candidate where CID = '" + val1 + "'", con);
                    byte[] buffer = (byte[])cmd2.ExecuteScalar();

                    Image img = Image.FromStream(new MemoryStream(buffer));
                    Bitmap tImg = new Bitmap(new Bitmap(img));
                    pictureBox1.Image = tImg;
                    con.Close();
                }
            }
            catch
            {

            }
        }
    }
}
