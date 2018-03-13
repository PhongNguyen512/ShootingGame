namespace ShootingGame
{
    partial class AstherRoids
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.BulletTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Interval = 25;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // BulletTimer
            // 
            this.BulletTimer.Enabled = true;
            this.BulletTimer.Interval = 75;
            this.BulletTimer.Tick += new System.EventHandler(this.BulletTimer_Tick);
            // 
            // AstherRoids
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 849);
            this.Name = "AstherRoids";
            this.Text = "Batman - Plane Shooting Game";
            this.Load += new System.EventHandler(this.AstherRoids_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AstherRoids_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AstherRoids_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Timer BulletTimer;
    }
}

