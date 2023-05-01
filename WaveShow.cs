using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace AudioLoopBack
{
    public partial class WaveShow : UserControl
    {

        [DllImport("user32.dll")]

        public static extern IntPtr GetDC(

         IntPtr hwnd

         );
        private float[] _data_;
        public float[] Data
        {
            set
            {
                _data_ = value;
                this.Invalidate();
            }
            get
            {
                //if (_data_ != null)
                return _data_;
            }

        }


        private Color _BaseLineColor_;
        public Color BaseLineColor
        {
            set
            {
                _BaseLineColor_ = value;
                this.Invalidate();
            }
            get
            {
                //if (_data_ != null)
                return _BaseLineColor_;
            }

        }

        private Color _BackgroundColor_;
        public Color BackgroundColor
        {
            set
            {

                _BackgroundColor_ = value;
                genExtraTextBmp(ClientRectangle.Width,ClientRectangle.Height);
                this.Invalidate();
            }
            get
            {
                //if (_data_ != null)
                return _BackgroundColor_;
            }

        }

        private Color _LineColor_;
        public Color LineColor
        {
            set
            {
                _LineColor_ = value;
                this.Invalidate();
            }
            get
            {
                //if (_data_ != null)
                return _LineColor_;
            }

        }

        private string _addtionaltext__ = null;
        public string AddtionalText
        {
            set { _addtionaltext__ = value; this.Invalidate(); }
            get { return _addtionaltext__; }
        }

        private Bitmap extraStrBmp;
        private Rectangle lastRect;

        public WaveShow()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
        ControlStyles.ResizeRedraw |
        ControlStyles.AllPaintingInWmPaint, true);
            _data_ = new float[128];
            for (int i = 0; i < 128; i++)
            {
                _data_[i] = (float)Math.Sin((double)i / 128 * 2 * Math.PI);
            }

            _LineColor_ = Color.FromArgb(255, 255, 153, 0);
            _BaseLineColor_ = Color.DodgerBlue;
            _BackgroundColor_ = Color.White;
            _addtionaltext__ = "Additional Text !! ";
            this.Invalidate();
        }

        private void genExtraTextBmp(int Width,int Height)
        {
            extraStrBmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(extraStrBmp);
            //Draw addtional string
            Font StrFont = new Font("Times New Roman", Height / 10F, GraphicsUnit.Pixel);
            SizeF StrSize = new SizeF();
            StrSize = g.MeasureString(_addtionaltext__, StrFont);
            extraStrBmp = new Bitmap((int)StrSize.Width, (int)StrSize.Height, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(extraStrBmp);
            g.Clear(Color.FromArgb(200, Color.White));

            SolidBrush StrBrush = new SolidBrush(_BackgroundColor_);

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.DrawString(_addtionaltext__, StrFont, StrBrush, 0, 0);
        }

        private void waveshow_Paint(object sender, PaintEventArgs e)
        {



            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;

            g.Clear(_BackgroundColor_);

            Rectangle rect = this.ClientRectangle;

            int maxBarHeight = rect.Height;
            float bar_width_f = ((float)rect.Width / _data_.Length);


            g.SmoothingMode = SmoothingMode.HighSpeed; 
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed; 


            PointF[] pf = new PointF[_data_.Length];
            for (int i = 0; i < _data_.Length; i++)
            {

                pf[i].Y = ((float)maxBarHeight * (1.0F - (_data_[i] + 1.0F) / 2.0F));
                pf[i].X = i * bar_width_f;



            }
            g.DrawLines(new Pen(_LineColor_, 2), pf);
            g.DrawLine(new Pen(_BaseLineColor_, 1), new Point(0, maxBarHeight / 2), new Point(rect.Width, maxBarHeight / 2));


            //Draw addtional string


            if (lastRect != this.ClientRectangle)
            {
                lastRect = this.ClientRectangle;
                genExtraTextBmp(lastRect.Width,lastRect.Height);
            }
            if (extraStrBmp != null)
                g.DrawImage(extraStrBmp, new Point((int)(maxBarHeight / 18f), (int)(maxBarHeight / 18F)));

            myBuffer.Render(e.Graphics);


            g.Dispose();
            myBuffer.Dispose();
        }
    }
}
