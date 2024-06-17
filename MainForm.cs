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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void addCandidateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCandidate add = new AddCandidate();
            add.Show();
        }

        private void addVotersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddVoters voters = new AddVoters();
            voters.Show();
        }

        private void addElectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddElection election = new AddElection();
            election.Show();
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculate calc = new Calculate();
            calc.Show();
        }

        private void calculateResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result res = new Result();
            res.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            MainDashboard voterLogin = new MainDashboard();
            voterLogin.Show();
        }
    }
}
