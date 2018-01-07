namespace CPENTECOST.BizHawk.DeepLearning
{
    partial class UserView
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
            this.pictureBox_inputImage = new System.Windows.Forms.PictureBox();
            this.tabControl_neuralNetView = new System.Windows.Forms.TabControl();
            this.tabPage_init = new System.Windows.Forms.TabPage();
            this.tabPage_extract = new System.Windows.Forms.TabPage();
            this.comboBox_imageMode = new System.Windows.Forms.ComboBox();
            this.textBox_watchName = new System.Windows.Forms.TextBox();
            this.textBox_watchAddr = new System.Windows.Forms.TextBox();
            this.textBox_watchNotes = new System.Windows.Forms.TextBox();
            this.textBox_watchValFloat = new System.Windows.Forms.TextBox();
            this.textBox_watchValUint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_setWatch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_inputImage)).BeginInit();
            this.tabControl_neuralNetView.SuspendLayout();
            this.tabPage_extract.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_inputImage
            // 
            this.pictureBox_inputImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_inputImage.Location = new System.Drawing.Point(8, 104);
            this.pictureBox_inputImage.Name = "pictureBox_inputImage";
            this.pictureBox_inputImage.Size = new System.Drawing.Size(762, 424);
            this.pictureBox_inputImage.TabIndex = 0;
            this.pictureBox_inputImage.TabStop = false;
            // 
            // tabControl_neuralNetView
            // 
            this.tabControl_neuralNetView.Controls.Add(this.tabPage_init);
            this.tabControl_neuralNetView.Controls.Add(this.tabPage_extract);
            this.tabControl_neuralNetView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_neuralNetView.Location = new System.Drawing.Point(0, 0);
            this.tabControl_neuralNetView.Name = "tabControl_neuralNetView";
            this.tabControl_neuralNetView.SelectedIndex = 0;
            this.tabControl_neuralNetView.Size = new System.Drawing.Size(784, 562);
            this.tabControl_neuralNetView.TabIndex = 1;
            // 
            // tabPage_init
            // 
            this.tabPage_init.Location = new System.Drawing.Point(4, 22);
            this.tabPage_init.Name = "tabPage_init";
            this.tabPage_init.Size = new System.Drawing.Size(776, 536);
            this.tabPage_init.TabIndex = 2;
            this.tabPage_init.Text = "Initialize";
            this.tabPage_init.UseVisualStyleBackColor = true;
            // 
            // tabPage_extract
            // 
            this.tabPage_extract.Controls.Add(this.button_setWatch);
            this.tabPage_extract.Controls.Add(this.label5);
            this.tabPage_extract.Controls.Add(this.label4);
            this.tabPage_extract.Controls.Add(this.label3);
            this.tabPage_extract.Controls.Add(this.label2);
            this.tabPage_extract.Controls.Add(this.label1);
            this.tabPage_extract.Controls.Add(this.textBox_watchValUint);
            this.tabPage_extract.Controls.Add(this.textBox_watchValFloat);
            this.tabPage_extract.Controls.Add(this.textBox_watchNotes);
            this.tabPage_extract.Controls.Add(this.textBox_watchAddr);
            this.tabPage_extract.Controls.Add(this.textBox_watchName);
            this.tabPage_extract.Controls.Add(this.comboBox_imageMode);
            this.tabPage_extract.Controls.Add(this.pictureBox_inputImage);
            this.tabPage_extract.Location = new System.Drawing.Point(4, 22);
            this.tabPage_extract.Name = "tabPage_extract";
            this.tabPage_extract.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_extract.Size = new System.Drawing.Size(776, 536);
            this.tabPage_extract.TabIndex = 0;
            this.tabPage_extract.Text = "Extract";
            this.tabPage_extract.UseVisualStyleBackColor = true;
            // 
            // comboBox_imageMode
            // 
            this.comboBox_imageMode.FormattingEnabled = true;
            this.comboBox_imageMode.Location = new System.Drawing.Point(8, 6);
            this.comboBox_imageMode.Name = "comboBox_imageMode";
            this.comboBox_imageMode.Size = new System.Drawing.Size(121, 21);
            this.comboBox_imageMode.TabIndex = 1;
            this.comboBox_imageMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_imageMode_SelectedIndexChanged);
            // 
            // textBox_watchName
            // 
            this.textBox_watchName.Location = new System.Drawing.Point(245, 78);
            this.textBox_watchName.Name = "textBox_watchName";
            this.textBox_watchName.Size = new System.Drawing.Size(100, 20);
            this.textBox_watchName.TabIndex = 2;
            this.textBox_watchName.Text = "P1 Pos";
            // 
            // textBox_watchAddr
            // 
            this.textBox_watchAddr.Location = new System.Drawing.Point(351, 78);
            this.textBox_watchAddr.Name = "textBox_watchAddr";
            this.textBox_watchAddr.Size = new System.Drawing.Size(100, 20);
            this.textBox_watchAddr.TabIndex = 3;
            this.textBox_watchAddr.Text = "0x1644D0";
            // 
            // textBox_watchNotes
            // 
            this.textBox_watchNotes.Location = new System.Drawing.Point(477, 78);
            this.textBox_watchNotes.Name = "textBox_watchNotes";
            this.textBox_watchNotes.Size = new System.Drawing.Size(100, 20);
            this.textBox_watchNotes.TabIndex = 4;
            this.textBox_watchNotes.Text = "(Float) position of player 1. 0-1 is one lap";
            // 
            // textBox_watchValFloat
            // 
            this.textBox_watchValFloat.Enabled = false;
            this.textBox_watchValFloat.Location = new System.Drawing.Point(668, 78);
            this.textBox_watchValFloat.Name = "textBox_watchValFloat";
            this.textBox_watchValFloat.Size = new System.Drawing.Size(100, 20);
            this.textBox_watchValFloat.TabIndex = 5;
            this.textBox_watchValFloat.Text = "NA";
            // 
            // textBox_watchValUint
            // 
            this.textBox_watchValUint.Enabled = false;
            this.textBox_watchValUint.Location = new System.Drawing.Point(668, 30);
            this.textBox_watchValUint.Name = "textBox_watchValUint";
            this.textBox_watchValUint.Size = new System.Drawing.Size(100, 20);
            this.textBox_watchValUint.TabIndex = 6;
            this.textBox_watchValUint.Text = "NA";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Address (hex, 0x1234)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(474, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Notes";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(665, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Value (uint)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(665, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Value (float)";
            // 
            // button_setWatch
            // 
            this.button_setWatch.Location = new System.Drawing.Point(587, 75);
            this.button_setWatch.Name = "button_setWatch";
            this.button_setWatch.Size = new System.Drawing.Size(75, 23);
            this.button_setWatch.TabIndex = 12;
            this.button_setWatch.Text = "Set";
            this.button_setWatch.UseVisualStyleBackColor = true;
            this.button_setWatch.Click += new System.EventHandler(this.button_setWatch_Click);
            // 
            // UserView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl_neuralNetView);
            this.Name = "UserView";
            this.Text = "UserView";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_inputImage)).EndInit();
            this.tabControl_neuralNetView.ResumeLayout(false);
            this.tabPage_extract.ResumeLayout(false);
            this.tabPage_extract.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_inputImage;
        private System.Windows.Forms.TabControl tabControl_neuralNetView;
        private System.Windows.Forms.TabPage tabPage_extract;
        private System.Windows.Forms.ComboBox comboBox_imageMode;
        private System.Windows.Forms.TabPage tabPage_init;
        private System.Windows.Forms.Button button_setWatch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_watchValUint;
        private System.Windows.Forms.TextBox textBox_watchValFloat;
        private System.Windows.Forms.TextBox textBox_watchNotes;
        private System.Windows.Forms.TextBox textBox_watchAddr;
        private System.Windows.Forms.TextBox textBox_watchName;
    }
}