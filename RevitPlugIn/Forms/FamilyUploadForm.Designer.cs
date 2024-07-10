namespace RevitPlugIn.Forms
{
    partial class FamilyUploadForm
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
            this.familyBox = new System.Windows.Forms.ListBox();
            this.UploadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // familyBox
            // 
            this.familyBox.FormattingEnabled = true;
            this.familyBox.ItemHeight = 16;
            this.familyBox.Location = new System.Drawing.Point(-1, -4);
            this.familyBox.Name = "familyBox";
            this.familyBox.Size = new System.Drawing.Size(802, 372);
            this.familyBox.TabIndex = 0;
            // 
            // UploadButton
            // 
            this.UploadButton.Location = new System.Drawing.Point(261, 384);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(216, 45);
            this.UploadButton.TabIndex = 1;
            this.UploadButton.Text = "UploadButton";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // FamilyUploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.familyBox);
            this.Name = "FamilyUploadForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox familyBox;
        private System.Windows.Forms.Button UploadButton;
    }
}