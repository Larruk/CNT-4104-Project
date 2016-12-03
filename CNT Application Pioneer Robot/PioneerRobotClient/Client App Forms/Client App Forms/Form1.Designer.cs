namespace Client_App_Forms
{
    partial class Form1
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
            this.camView = new System.Windows.Forms.PictureBox();
            this.forward = new System.Windows.Forms.Button();
            this.back = new System.Windows.Forms.Button();
            this.right = new System.Windows.Forms.Button();
            this.left = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.connect = new System.Windows.Forms.Button();
            this.disconnect = new System.Windows.Forms.Button();
            this.ipAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.distance = new System.Windows.Forms.TextBox();
            this.rotation = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.camView)).BeginInit();
            this.SuspendLayout();
            // 
            // camView
            // 
            this.camView.Location = new System.Drawing.Point(16, 15);
            this.camView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.camView.Name = "camView";
            this.camView.Size = new System.Drawing.Size(853, 591);
            this.camView.TabIndex = 0;
            this.camView.TabStop = false;
            // 
            // forward
            // 
            this.forward.Enabled = false;
            this.forward.Location = new System.Drawing.Point(1117, 166);
            this.forward.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.forward.Name = "forward";
            this.forward.Size = new System.Drawing.Size(107, 98);
            this.forward.TabIndex = 1;
            this.forward.Text = "Forward";
            this.forward.UseVisualStyleBackColor = true;
            this.forward.Click += new System.EventHandler(this.SendCommands);
            // 
            // back
            // 
            this.back.Enabled = false;
            this.back.Location = new System.Drawing.Point(1117, 378);
            this.back.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(107, 98);
            this.back.TabIndex = 2;
            this.back.Text = "Backward";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.SendCommands);
            // 
            // right
            // 
            this.right.Enabled = false;
            this.right.Location = new System.Drawing.Point(1232, 272);
            this.right.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(107, 98);
            this.right.TabIndex = 3;
            this.right.Text = "Rotate Right";
            this.right.UseVisualStyleBackColor = true;
            this.right.Click += new System.EventHandler(this.SendCommands);
            // 
            // left
            // 
            this.left.Enabled = false;
            this.left.Location = new System.Drawing.Point(1003, 272);
            this.left.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(107, 98);
            this.left.TabIndex = 4;
            this.left.Text = "Rotate Left";
            this.left.UseVisualStyleBackColor = true;
            this.left.Click += new System.EventHandler(this.SendCommands);
            // 
            // reset
            // 
            this.reset.Enabled = false;
            this.reset.Location = new System.Drawing.Point(1117, 272);
            this.reset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(107, 98);
            this.reset.TabIndex = 5;
            this.reset.Text = "Reset";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.SendCommands);
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(240, 676);
            this.connect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(100, 28);
            this.connect.TabIndex = 6;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.Connect);
            // 
            // disconnect
            // 
            this.disconnect.Enabled = false;
            this.disconnect.Location = new System.Drawing.Point(495, 676);
            this.disconnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(100, 28);
            this.disconnect.TabIndex = 7;
            this.disconnect.Text = "Disconnect";
            this.disconnect.UseVisualStyleBackColor = true;
            this.disconnect.Click += new System.EventHandler(this.Disconnect);
            // 
            // ipAddr
            // 
            this.ipAddr.Location = new System.Drawing.Point(388, 629);
            this.ipAddr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ipAddr.Name = "ipAddr";
            this.ipAddr.Size = new System.Drawing.Size(132, 22);
            this.ipAddr.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 629);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1061, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Distance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1061, 79);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Rotation";
            // 
            // distance
            // 
            this.distance.Location = new System.Drawing.Point(1148, 48);
            this.distance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.distance.Name = "distance";
            this.distance.Size = new System.Drawing.Size(132, 22);
            this.distance.TabIndex = 12;
            // 
            // rotation
            // 
            this.rotation.Location = new System.Drawing.Point(1148, 79);
            this.rotation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rotation.Name = "rotation";
            this.rotation.Size = new System.Drawing.Size(132, 22);
            this.rotation.TabIndex = 13;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(1003, 556);
            this.status.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(335, 180);
            this.status.TabIndex = 14;
            this.status.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1377, 752);
            this.Controls.Add(this.status);
            this.Controls.Add(this.rotation);
            this.Controls.Add(this.distance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ipAddr);
            this.Controls.Add(this.disconnect);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.left);
            this.Controls.Add(this.right);
            this.Controls.Add(this.back);
            this.Controls.Add(this.forward);
            this.Controls.Add(this.camView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.camView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox camView;
        private System.Windows.Forms.Button forward;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Button right;
        private System.Windows.Forms.Button left;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button disconnect;
        private System.Windows.Forms.TextBox ipAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox distance;
        private System.Windows.Forms.TextBox rotation;
        private System.Windows.Forms.RichTextBox status;
    }
}

