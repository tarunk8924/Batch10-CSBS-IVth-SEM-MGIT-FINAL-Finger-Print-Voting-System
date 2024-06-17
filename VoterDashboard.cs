using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finger_Print_Based_Voting_System
{
    public partial class VoterDashboard : Form
    {
        public VoterDashboard()
        {
            InitializeComponent();
        }

        private void VoterDashboard_Load(object sender, EventArgs e)
        {
            // Optional: Load voter-specific information if necessary
        }

        private void btnVoteNow_Click(object sender, EventArgs e)
        {
            Election election = new Election();
            election.Show();
        }

        private void btnElectionResults_Click(object sender, EventArgs e)
        {
            Result res = new Result();
            res.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            MainDashboard voterLogin = new MainDashboard();
            voterLogin.Show();
        }
    }
}


