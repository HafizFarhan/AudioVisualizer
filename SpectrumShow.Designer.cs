namespace AudioLoopBack
{
    partial class SpectrumShow
    {
        /// <summary> 
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// </summary>
        /// <param name="disposing"> true； false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 

        /// <summary> 
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpectrumShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Name = "SpectrumShow";
            this.Size = new System.Drawing.Size(526, 334);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.waveshow_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SpectrumShow_MouseDown);
            this.MouseLeave += new System.EventHandler(this.SpectrumShow_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpectrumShow_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SpectrumShow_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
