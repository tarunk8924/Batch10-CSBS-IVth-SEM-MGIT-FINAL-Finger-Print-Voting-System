using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Finger_Print_Based_Voting_System
{
    public partial class Calculate : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);
        public Calculate()
        {
            InitializeComponent();
        }

        private void Calculate_Load(object sender, EventArgs e)
        {
            electionid();
        }
        private void electionid()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select ElectionID from Election", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            comboBox1.Items.Insert(0, "Please choose an item");
            comboBox1.ValueMember = "ElectionID";
            comboBox1.DisplayMember = "ElectionID";
            comboBox1.DataSource = dt;
            con.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(*) as Votes, CID as Participants from Vote where ElectionID='"+comboBox1.Text+"' group by CID", con);                
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                SqlCommand cmd1 = new SqlCommand("select TOP 1 count(*) as cnt,CID from Vote where ElectionID='" + comboBox1.Text + "' group by CID ORDER BY cnt DESC", con);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    label1.Text = "The Leading Participant is : " + dr.GetString(1).ToString();
                    label2.Text = dr.GetString(1).ToString();
                    label4.Text = dr.GetValue(0).ToString();
                    button1.Enabled = true;
                }
                dr.Close();
                con.Close();
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cc = new SqlCommand("select Candidate from Result where ElectionID='" + comboBox1.Text+ "'", con);
            string candidate = Convert.ToString(cc.ExecuteScalar());
            if (candidate == "")
            {
                SqlCommand cmd = new SqlCommand("insert into Result values('" + comboBox1.Text + "','" + label2.Text + "','" + label4.Text + "')", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Result Announced!");
            }
            else
            {
                MessageBox.Show("Already Announced!");
            }
            con.Close();
        }
    }
}
