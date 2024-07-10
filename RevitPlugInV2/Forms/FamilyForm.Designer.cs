using System.Windows.Forms;

namespace RevitPlugInV2.Forms
{
    partial class FamilyForm
    {
        public ComboBox FamilyComboBox;
       


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
            button3 = new Button();
            dataGridView1 = new DataGridView();
            button2 = new Button();
            UploadButton = new Button();
            comboBox1 = new ComboBox();
            button4 = new Button();
            Apply = new Button();
            textBox1 = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            button5 = new Button();
            textBox2 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Location = new Point(-134, 4);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(75, 29);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(3, 43);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 24;
            dataGridView1.Size = new Size(282, 401);
            dataGridView1.TabIndex = 4;
            // 
            // button2
            // 
            button2.Location = new Point(291, 43);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(309, 401);
            button2.TabIndex = 3;
            button2.Text = "Load Families";
            button2.UseVisualStyleBackColor = true;
            button2.Click += LoadFamilies_Click;
            // 
            // UploadButton
            // 
            UploadButton.Location = new Point(3, 487);
            UploadButton.Margin = new Padding(3, 4, 3, 4);
            UploadButton.Name = "UploadButton";
            UploadButton.Size = new Size(229, 33);
            UploadButton.TabIndex = 0;
            UploadButton.Text = "AddToLibrary";
            UploadButton.UseVisualStyleBackColor = true;
            UploadButton.Click += AddToLibraryButton_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 452);
            comboBox1.Margin = new Padding(3, 4, 3, 4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(282, 28);
            comboBox1.TabIndex = 1;
            // 
            // button4
            // 
            button4.Location = new Point(606, 4);
            button4.Margin = new Padding(3, 4, 3, 4);
            button4.Name = "button4";
            button4.Size = new Size(136, 28);
            button4.TabIndex = 5;
            button4.Text = "LogOut";
            button4.UseVisualStyleBackColor = true;
            button4.Click += LogOut_Click;
            // 
            // Apply
            // 
            Apply.Location = new Point(291, 4);
            Apply.Margin = new Padding(3, 4, 3, 4);
            Apply.Name = "Apply";
            Apply.Size = new Size(309, 31);
            Apply.TabIndex = 6;
            Apply.Text = "Apply";
            Apply.UseVisualStyleBackColor = true;
            Apply.Click += Apply_Click;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(3, 4);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(282, 27);
            textBox1.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.69614F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.30386F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
            tableLayoutPanel1.Controls.Add(button5, 1, 3);
            tableLayoutPanel1.Controls.Add(textBox2, 1, 2);
            tableLayoutPanel1.Controls.Add(comboBox1, 0, 2);
            tableLayoutPanel1.Controls.Add(button2, 1, 1);
            tableLayoutPanel1.Controls.Add(Apply, 1, 0);
            tableLayoutPanel1.Controls.Add(UploadButton, 0, 3);
            tableLayoutPanel1.Controls.Add(button4, 2, 0);
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 1);
            tableLayoutPanel1.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel1.Location = new Point(1, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.797654F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 91.20235F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(753, 524);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // button5
            // 
            button5.Location = new Point(291, 487);
            button5.Margin = new Padding(3, 4, 3, 4);
            button5.Name = "button5";
            button5.Size = new Size(309, 33);
            button5.TabIndex = 9;
            button5.Text = "Filter";
            button5.UseVisualStyleBackColor = true;
            button5.Click += FilterByLetter_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(291, 452);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(309, 27);
            textBox2.TabIndex = 9;
            // 
            // FamilyForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(756, 525);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(button3);
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(774, 572);
            MinimumSize = new Size(774, 572);
            Name = "FamilyForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button button3;
        private DataGridView dataGridView1;
        private Button button2;
        private Button UploadButton;
        private ComboBox comboBox1;
        private Button button4;
        private Button Apply;
        private TextBox textBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button5;
        private TextBox textBox2;
    }
}