namespace Finger_Print_Based_Voting_System
{
    partial class VoterDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnVoteNow = new System.Windows.Forms.Button();
            this.btnElectionResults = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVoteNow
            // 
            this.btnVoteNow.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoteNow.Location = new System.Drawing.Point(75, 41);
            this.btnVoteNow.Margin = new System.Windows.Forms.Padding(2);
            this.btnVoteNow.Name = "btnVoteNow";
            this.btnVoteNow.Size = new System.Drawing.Size(150, 41);
            this.btnVoteNow.TabIndex = 0;
            this.btnVoteNow.Text = "Vote Now";
            this.btnVoteNow.UseVisualStyleBackColor = true;
            this.btnVoteNow.Click += new System.EventHandler(this.btnVoteNow_Click);
            // 
            // btnElectionResults
            // 
            this.btnElectionResults.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElectionResults.Location = new System.Drawing.Point(75, 122);
            this.btnElectionResults.Margin = new System.Windows.Forms.Padding(2);
            this.btnElectionResults.Name = "btnElectionResults";
            this.btnElectionResults.Size = new System.Drawing.Size(150, 41);
            this.btnElectionResults.TabIndex = 1;
            this.btnElectionResults.Text = "Election Results";
            this.btnElectionResults.UseVisualStyleBackColor = true;
            this.btnElectionResults.Click += new System.EventHandler(this.btnElectionResults_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(75, 203);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(150, 41);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // VoterDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Finger_Print_Based_Voting_System.Properties.Resources._1234__3_;
            this.ClientSize = new System.Drawing.Size(299, 284);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnElectionResults);
            this.Controls.Add(this.btnVoteNow);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VoterDashboard";
            this.Text = "Voter Dashboard";
            this.Load += new System.EventHandler(this.VoterDashboard_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnVoteNow;
        private System.Windows.Forms.Button btnElectionResults;
        private System.Windows.Forms.Button btnLogout;
    }
}
