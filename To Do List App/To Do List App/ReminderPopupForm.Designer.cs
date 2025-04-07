namespace To_Do_List_App
{
    partial class ReminderPopupForm
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
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Location = new System.Drawing.Point(458, 12);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(302, 252);
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(458, 317);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(302, 43);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "label1";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(458, 403);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(302, 32);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "label2";
            // 
            // ReminderPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 679);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.pictureBoxImage);
            this.Name = "ReminderPopupForm";
            this.Text = "ReminderPopupForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelDescription;
    }
}