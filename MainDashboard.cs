using System;
using System.Windows.Forms;

namespace Finger_Print_Based_Voting_System
{
    public partial class MainDashboard : Form
    {
        public MainDashboard()
        {
            InitializeComponent();
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            Login adminLogin = new Login();
            adminLogin.Show();
            this.Hide();
        }

        private void btnVoterLogin_Click(object sender, EventArgs e)
        {
            VoterLogin voterLogin = new VoterLogin();
            voterLogin.Show();
            this.Hide();
        }
    }
}

