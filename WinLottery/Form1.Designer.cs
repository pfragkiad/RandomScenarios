namespace WinLottery
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cboLottery = new ComboBox();
            numTickets = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            txtResults = new TextBox();
            btnGenerate = new Button();
            ((System.ComponentModel.ISupportInitialize)numTickets).BeginInit();
            SuspendLayout();
            // 
            // cboLottery
            // 
            cboLottery.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLottery.FormattingEnabled = true;
            cboLottery.Items.AddRange(new object[] { "Tzoker", "Lotto" });
            cboLottery.Location = new Point(112, 34);
            cboLottery.Name = "cboLottery";
            cboLottery.Size = new Size(189, 23);
            cboLottery.TabIndex = 0;
            // 
            // numTickets
            // 
            numTickets.Location = new Point(112, 85);
            numTickets.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numTickets.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTickets.Name = "numTickets";
            numTickets.Size = new Size(52, 23);
            numTickets.TabIndex = 2;
            numTickets.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 88);
            label1.Name = "label1";
            label1.Size = new Size(46, 15);
            label1.TabIndex = 3;
            label1.Text = "Tickets:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 37);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 3;
            label2.Text = "Lottery:";
            // 
            // txtResults
            // 
            txtResults.Location = new Point(25, 134);
            txtResults.Multiline = true;
            txtResults.Name = "txtResults";
            txtResults.ScrollBars = ScrollBars.Vertical;
            txtResults.Size = new Size(276, 110);
            txtResults.TabIndex = 4;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(194, 250);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(107, 26);
            btnGenerate.TabIndex = 5;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(321, 284);
            Controls.Add(btnGenerate);
            Controls.Add(txtResults);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numTickets);
            Controls.Add(cboLottery);
            Name = "Form1";
            Text = "WinLottery";
            ((System.ComponentModel.ISupportInitialize)numTickets).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cboLottery;
        private NumericUpDown numTickets;
        private Label label1;
        private Label label2;
        private TextBox txtResults;
        private Button btnGenerate;
    }
}