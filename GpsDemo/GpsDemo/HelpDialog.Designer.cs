namespace JeromeTerry.GpsDemo
{
    partial class HelpDialog
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
            this._helpText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _helpText
            // 
            this._helpText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._helpText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._helpText.Location = new System.Drawing.Point(31, 25);
            this._helpText.Name = "_helpText";
            this._helpText.ReadOnly = true;
            this._helpText.Size = new System.Drawing.Size(692, 421);
            this._helpText.TabIndex = 0;
            this._helpText.Text = "";
            // 
            // HelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 471);
            this.Controls.Add(this._helpText);
            this.Name = "HelpDialog";
            this.Text = "JT-GPS Demo Help ";
            this.Load += new System.EventHandler(this.HelpDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _helpText;
    }
}