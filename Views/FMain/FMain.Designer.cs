namespace AIOAuto.Views.FMain
{
	partial class FMain
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
			this.picChangeLanguage = new System.Windows.Forms.PictureBox();
			this.infoLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picChangeLanguage)).BeginInit();
			this.SuspendLayout();
			//
			// picChangeLanguage
			//
			this.picChangeLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picChangeLanguage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picChangeLanguage.Image = global::AIOAuto.Properties.Resources.united_kingdom1;
			this.picChangeLanguage.Location = new System.Drawing.Point(1576, 12);
			this.picChangeLanguage.Name = "picChangeLanguage";
			this.picChangeLanguage.Size = new System.Drawing.Size(25, 25);
			this.picChangeLanguage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picChangeLanguage.TabIndex = 10;
			this.picChangeLanguage.TabStop = false;
			this.picChangeLanguage.WaitOnLoad = true;
			this.picChangeLanguage.Click += new System.EventHandler(this.picChangeLanguage_Click);
			//
			// infoLabel
			//
			this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.infoLabel.Location = new System.Drawing.Point(55, 12);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(1498, 25);
			this.infoLabel.TabIndex = 11;
			this.infoLabel.Text = "label1";
			this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// FMain
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1613, 1070);
			this.Controls.Add(this.infoLabel);
			this.Controls.Add(this.picChangeLanguage);
			this.Name = "FMain";
			this.Text = "FMain";
			((System.ComponentModel.ISupportInitialize)(this.picChangeLanguage)).EndInit();
			this.ResumeLayout(false);
		}

		private System.Windows.Forms.Label infoLabel;

		private System.Windows.Forms.PictureBox picChangeLanguage;

		#endregion
	}
}

