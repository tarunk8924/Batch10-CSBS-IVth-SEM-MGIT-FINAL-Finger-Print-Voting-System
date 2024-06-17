namespace Finger_Print_Based_Voting_System
{
    partial class MainDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Other controls declarations

        private void InitializeComponent()
        {
            this.btnAdminLogin = new System.Windows.Forms.Button();
            this.btnVoterLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAdminLogin
            // 
            this.btnAdminLogin.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdminLogin.Location = new System.Drawing.Point(50, 50);
            this.btnAdminLogin.Name = "btnAdminLogin";
            this.btnAdminLogin.Size = new System.Drawing.Size(150, 50);
            this.btnAdminLogin.TabIndex = 0;
            this.btnAdminLogin.Text = "Admin Login";
            this.btnAdminLogin.UseVisualStyleBackColor = true;
            this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
            // 
            // btnVoterLogin
            // 
            this.btnVoterLogin.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoterLogin.Location = new System.Drawing.Point(50, 120);
            this.btnVoterLogin.Name = "btnVoterLogin";
            this.btnVoterLogin.Size = new System.Drawing.Size(150, 50);
            this.btnVoterLogin.TabIndex = 1;
            this.btnVoterLogin.Text = "Voter Login";
            this.btnVoterLogin.UseVisualStyleBackColor = true;
            this.btnVoterLogin.Click += new System.EventHandler(this.btnVoterLogin_Click);
            // 
            // MainDashboard
            // 
            this.BackgroundImage = global::Finger_Print_Based_Voting_System.Properties.Resources.tumblr_na;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnAdminLogin);
            this.Controls.Add(this.btnVoterLogin);
            this.Name = "MainDashboard";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnAdminLogin;
        private System.Windows.Forms.Button btnVoterLogin;
        // Other control declarations
    }
}
