namespace Stage_GUI
{
    partial class MainGUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.X_move = new System.Windows.Forms.Button();
            this.textboxInc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.X_move_neg = new System.Windows.Forms.Button();
            this.Y_move_neg = new System.Windows.Forms.Button();
            this.Y_move = new System.Windows.Forms.Button();
            this.Z_move = new System.Windows.Forms.Button();
            this.Z_move_neg = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEFDOff = new System.Windows.Forms.Button();
            this.buttonEFDOn = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textboxSpeed = new System.Windows.Forms.TextBox();
            this.Reset_Stage = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxEFDPortNames = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBoxStagePorts = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 102);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Stage Port";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(59, 206);
            this.buttonRun.Margin = new System.Windows.Forms.Padding(6);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(138, 42);
            this.buttonRun.TabIndex = 11;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 25);
            this.label6.TabIndex = 14;
            this.label6.Text = "File Name";
            // 
            // X_move
            // 
            this.X_move.Location = new System.Drawing.Point(244, 222);
            this.X_move.Margin = new System.Windows.Forms.Padding(6);
            this.X_move.Name = "X_move";
            this.X_move.Size = new System.Drawing.Size(62, 44);
            this.X_move.TabIndex = 16;
            this.X_move.Text = "X+";
            this.X_move.UseVisualStyleBackColor = true;
            this.X_move.Click += new System.EventHandler(this.X_move_Click);
            // 
            // textboxInc
            // 
            this.textboxInc.Location = new System.Drawing.Point(123, 57);
            this.textboxInc.Margin = new System.Windows.Forms.Padding(6);
            this.textboxInc.Name = "textboxInc";
            this.textboxInc.Size = new System.Drawing.Size(180, 29);
            this.textboxInc.TabIndex = 17;
            this.textboxInc.TextChanged += new System.EventHandler(this.inc_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 63);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 25);
            this.label7.TabIndex = 18;
            this.label7.Text = "Increment";
            // 
            // X_move_neg
            // 
            this.X_move_neg.Location = new System.Drawing.Point(50, 223);
            this.X_move_neg.Margin = new System.Windows.Forms.Padding(6);
            this.X_move_neg.Name = "X_move_neg";
            this.X_move_neg.Size = new System.Drawing.Size(62, 42);
            this.X_move_neg.TabIndex = 19;
            this.X_move_neg.Text = "X-";
            this.X_move_neg.UseVisualStyleBackColor = true;
            this.X_move_neg.Click += new System.EventHandler(this.X_move_neg_Click);
            // 
            // Y_move_neg
            // 
            this.Y_move_neg.Location = new System.Drawing.Point(147, 170);
            this.Y_move_neg.Margin = new System.Windows.Forms.Padding(6);
            this.Y_move_neg.Name = "Y_move_neg";
            this.Y_move_neg.Size = new System.Drawing.Size(59, 44);
            this.Y_move_neg.TabIndex = 20;
            this.Y_move_neg.Text = "Y-";
            this.Y_move_neg.UseVisualStyleBackColor = true;
            this.Y_move_neg.Click += new System.EventHandler(this.Y_move_neg_Click);
            // 
            // Y_move
            // 
            this.Y_move.Location = new System.Drawing.Point(147, 286);
            this.Y_move.Margin = new System.Windows.Forms.Padding(6);
            this.Y_move.Name = "Y_move";
            this.Y_move.Size = new System.Drawing.Size(59, 44);
            this.Y_move.TabIndex = 21;
            this.Y_move.Text = "Y+";
            this.Y_move.UseVisualStyleBackColor = true;
            this.Y_move.Click += new System.EventHandler(this.Y_move_Click);
            // 
            // Z_move
            // 
            this.Z_move.Location = new System.Drawing.Point(359, 186);
            this.Z_move.Margin = new System.Windows.Forms.Padding(6);
            this.Z_move.Name = "Z_move";
            this.Z_move.Size = new System.Drawing.Size(68, 44);
            this.Z_move.TabIndex = 22;
            this.Z_move.Text = "Z+";
            this.Z_move.UseVisualStyleBackColor = true;
            this.Z_move.Click += new System.EventHandler(this.Z_move_Click);
            // 
            // Z_move_neg
            // 
            this.Z_move_neg.Location = new System.Drawing.Point(363, 286);
            this.Z_move_neg.Margin = new System.Windows.Forms.Padding(6);
            this.Z_move_neg.Name = "Z_move_neg";
            this.Z_move_neg.Size = new System.Drawing.Size(64, 44);
            this.Z_move_neg.TabIndex = 23;
            this.Z_move_neg.Text = "Z-";
            this.Z_move_neg.UseVisualStyleBackColor = true;
            this.Z_move_neg.Click += new System.EventHandler(this.Z_move_neg_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 124);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 25);
            this.label8.TabIndex = 25;
            this.label8.Text = "Speed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(317, 124);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 25);
            this.label9.TabIndex = 27;
            this.label9.Text = "mm/s";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonEFDOff);
            this.groupBox1.Controls.Add(this.buttonEFDOn);
            this.groupBox1.Controls.Add(this.textBoxSend);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.X_move_neg);
            this.groupBox1.Controls.Add(this.Z_move_neg);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.Z_move);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textboxInc);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textboxSpeed);
            this.groupBox1.Controls.Add(this.Y_move_neg);
            this.groupBox1.Controls.Add(this.Y_move);
            this.groupBox1.Controls.Add(this.X_move);
            this.groupBox1.Location = new System.Drawing.Point(22, 480);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(466, 467);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manual Control";
            // 
            // buttonEFDOff
            // 
            this.buttonEFDOff.Location = new System.Drawing.Point(204, 423);
            this.buttonEFDOff.Margin = new System.Windows.Forms.Padding(6);
            this.buttonEFDOff.Name = "buttonEFDOff";
            this.buttonEFDOff.Size = new System.Drawing.Size(114, 44);
            this.buttonEFDOff.TabIndex = 31;
            this.buttonEFDOff.Text = "EFD Off";
            this.buttonEFDOff.UseVisualStyleBackColor = true;
            // 
            // buttonEFDOn
            // 
            this.buttonEFDOn.Location = new System.Drawing.Point(352, 423);
            this.buttonEFDOn.Margin = new System.Windows.Forms.Padding(6);
            this.buttonEFDOn.Name = "buttonEFDOn";
            this.buttonEFDOn.Size = new System.Drawing.Size(114, 44);
            this.buttonEFDOn.TabIndex = 30;
            this.buttonEFDOn.Text = "EFD On";
            this.buttonEFDOn.UseVisualStyleBackColor = true;
            this.buttonEFDOn.Click += new System.EventHandler(this.buttonEFDOn_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(126, 354);
            this.textBoxSend.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(180, 29);
            this.textBoxSend.TabIndex = 29;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(38, 347);
            this.button4.Margin = new System.Windows.Forms.Padding(6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 44);
            this.button4.TabIndex = 28;
            this.button4.Text = "Send";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(317, 63);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 25);
            this.label10.TabIndex = 1;
            this.label10.Text = "hund-microns";
            // 
            // textboxSpeed
            // 
            this.textboxSpeed.Location = new System.Drawing.Point(123, 118);
            this.textboxSpeed.Margin = new System.Windows.Forms.Padding(6);
            this.textboxSpeed.Name = "textboxSpeed";
            this.textboxSpeed.Size = new System.Drawing.Size(180, 29);
            this.textboxSpeed.TabIndex = 0;
            this.textboxSpeed.TextChanged += new System.EventHandler(this.Speed_TextChanged);
            // 
            // Reset_Stage
            // 
            this.Reset_Stage.Location = new System.Drawing.Point(227, 206);
            this.Reset_Stage.Margin = new System.Windows.Forms.Padding(6);
            this.Reset_Stage.Name = "Reset_Stage";
            this.Reset_Stage.Size = new System.Drawing.Size(138, 42);
            this.Reset_Stage.TabIndex = 30;
            this.Reset_Stage.Text = "Reset";
            this.Reset_Stage.UseVisualStyleBackColor = true;
            this.Reset_Stage.Click += new System.EventHandler(this.Reset_Stage_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxEFDPortNames);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.comboBoxStagePorts);
            this.groupBox2.Controls.Add(this.Reset_Stage);
            this.groupBox2.Controls.Add(this.buttonRun);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(13, 22);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(427, 272);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Upload/Run";
            // 
            // comboBoxEFDPortNames
            // 
            this.comboBoxEFDPortNames.FormattingEnabled = true;
            this.comboBoxEFDPortNames.Location = new System.Drawing.Point(143, 146);
            this.comboBoxEFDPortNames.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxEFDPortNames.Name = "comboBoxEFDPortNames";
            this.comboBoxEFDPortNames.Size = new System.Drawing.Size(219, 32);
            this.comboBoxEFDPortNames.TabIndex = 34;
            this.comboBoxEFDPortNames.SelectedIndexChanged += new System.EventHandler(this.comboBoxEFDPortNames_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 151);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 25);
            this.label2.TabIndex = 33;
            this.label2.Text = "EFD Port";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 42);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(222, 42);
            this.button1.TabIndex = 32;
            this.button1.Text = "Search Directory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBoxStagePorts
            // 
            this.comboBoxStagePorts.FormattingEnabled = true;
            this.comboBoxStagePorts.Location = new System.Drawing.Point(143, 96);
            this.comboBoxStagePorts.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxStagePorts.Name = "comboBoxStagePorts";
            this.comboBoxStagePorts.Size = new System.Drawing.Size(219, 32);
            this.comboBoxStagePorts.TabIndex = 31;
            this.comboBoxStagePorts.SelectedIndexChanged += new System.EventHandler(this.comboBoxPorts_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Location = new System.Drawing.Point(13, 306);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox3.Size = new System.Drawing.Size(427, 162);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stage Positions";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(28, 96);
            this.button3.Margin = new System.Windows.Forms.Padding(6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(299, 42);
            this.button3.TabIndex = 37;
            this.button3.Text = "Z Position";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 42);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(299, 42);
            this.button2.TabIndex = 36;
            this.button2.Text = "X/Y Position";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonGetXY_Click);
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 969);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainGUI";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button X_move;
        private System.Windows.Forms.TextBox textboxInc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button X_move_neg;
        private System.Windows.Forms.Button Y_move_neg;
        private System.Windows.Forms.Button Y_move;
        private System.Windows.Forms.Button Z_move;
        private System.Windows.Forms.Button Z_move_neg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textboxSpeed;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button Reset_Stage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxStagePorts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBoxEFDPortNames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buttonEFDOff;
        private System.Windows.Forms.Button buttonEFDOn;
    }
}

