namespace AudioLoopBack
{
    partial class WaveShow
    {
        /// <summary> 
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// </summary>
        /// <param name="disposing"> false。</param>
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
            // WaveShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "WaveShow";
            this.Size = new System.Drawing.Size(941, 542);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.waveshow_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
