using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Finger_Print_Based_Voting_System
{
    public partial class VoterLogin : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);

        public VoterLogin()
        {
            InitializeComponent();
        }

        private void VoterLogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the Voter ID", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter the Password", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (IsValidVoter(textBox1.Text, textBox2.Text))
                {
                    VoterDashboard voterDashboard = new VoterDashboard();
                    voterDashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Voter ID or Password", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private bool IsValidVoter(string voterId, string password)
        {
            bool isValid = false;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Voters WHERE VoterID=@VoterID AND Mobile=@Password", con);
                cmd.Parameters.AddWithValue("@VoterID", voterId);
                cmd.Parameters.AddWithValue("@Password", password);

                int result = (int)cmd.ExecuteScalar();
                isValid = result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            return isValid;
        }
    }
}


