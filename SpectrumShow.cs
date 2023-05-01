using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace AudioLoopBack
{
    public partial class SpectrumShow : UserControl
    {
        private float[] _Data_;
        public float[] Data
        {
            set
            {
                _Data_ = value;
                this.Invalidate();
            }
            get
            {
                //if (_data_ != null)
                return _Data_;


            }

        }



        private Color _BackgroundColor_;
        public Color BackgroundColor
        {
            set
            {
                _BackgroundColor_ = value;
                genExtraTextBmp();
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

        private Color _SelectWindowColor_;
        public Color SelectWindowColor
        {
            set { _SelectWindowColor_ = value; this.Invalidate(); }
            get { return _SelectWindowColor_; }
        }

        private Color _SelectCursorColor_;
        public Color SelectCursorColor
        {
            set { _SelectCursorColor_ = value; this.Invalidate(); }
            get { return _SelectCursorColor_; }
        }


        private string _addtionaltext__ = null;
        public string AddtionalText
        {
            set { _addtionaltext__ = value; this.Invalidate(); }
            get { return _addtionaltext__; }
        }

        private bool _showruler_ = false;
        public bool ShowRuler
        {
            set { _showruler_ = value; this.Invalidate(); }
            get { return _showruler_; }
        }
        private float _RulerLeft_;
        public float RulerLeft
        {
            set { _RulerLeft_ = value; this.Invalidate(); }
            get { return _RulerLeft_; }
        }

        private float _RulerRight_;
        public float RulerRight
        {
            set { _RulerRight_ = value; this.Invalidate(); }
            get { return _RulerRight_; }
        }


        private float _MinimalWindow_;
        public float MinimalWindow
        {
            set { _MinimalWindow_ = value; this.Invalidate(); }
            get { return _MinimalWindow_; }
        }


        private int _Mouse_X = 0;
        private int _Mouse_Y = 0;

        private bool _LeftCursorPending_ = false;
        private bool _LeftCursorFocused_ = false;

        private bool _RightCursorPending_ = false;
        private bool _RightCursorFocused_ = false;

        private int RectWidth;
        private int RectHeight;

        private Bitmap extraStrBmp;
        private Rectangle lastRect;



        public SpectrumShow()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
        ControlStyles.ResizeRedraw |
        ControlStyles.AllPaintingInWmPaint, true);
            _Data_ = new float[1024];
            for (int i = 0; i < 1024; i++)
            {
                _Data_[i] = (float)Math.Sin((double)i / 1024 * 2 * Math.PI) / 2.0f + 0.5F;
            }

            _LineColor_ = Color.FromArgb(255, 255, 153, 0);
            _BackgroundColor_ = Color.White;
            _SelectCursorColor_ = Color.FromArgb(100, Color.Green);
            _SelectWindowColor_ = Color.FromArgb(100, Color.Red);

            _addtionaltext__ = "Addtional Text !!";

            _RulerLeft_ = 0F;
            _RulerRight_ = 1F;

            _MinimalWindow_ = 0.1F;


            RectWidth = this.Width;
            RectHeight = this.Height;






            this.Invalidate();
        }

        private void genExtraTextBmp()
        {
            extraStrBmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(extraStrBmp);
            //Draw addtional string
            Font StrFont = new Font("Times New Roman", RectHeight / 10F, GraphicsUnit.Pixel);
            SizeF StrSize = new SizeF();
            StrSize = g.MeasureString(_addtionaltext__, StrFont);
            //extraStrBmp = new Bitmap((int)StrSize.Width, (int)StrSize.Height,PixelFormat.Format32bppArgb);
            //g = Graphics.FromImage(extraStrBmp);
            //g.Clear(Color.FromArgb(200,Color.White));

            SolidBrush StrBrush = new SolidBrush(_BackgroundColor_);

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.DrawString(_addtionaltext__, StrFont, StrBrush, 0, 0);
        }
        private void waveshow_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush Brush_Rectangle = new SolidBrush(_LineColor_);
            

            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;

            g.Clear(_BackgroundColor_);

            Rectangle rect = this.ClientRectangle;


            RectWidth = rect.Width;
            RectHeight = rect.Height;


            int sample_len = _Data_.Length;
            int maxBarHeight = rect.Height;
            float bar_width_f = ((float)rect.Width / sample_len);


            g.SmoothingMode = SmoothingMode.HighSpeed; 
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed; 

            RectangleF[] rt = new RectangleF[sample_len];
            for (int i = 0; i < sample_len; i++)
            {

                rt[i].X = i * bar_width_f;
                rt[i].Width = bar_width_f;
                rt[i].Y = ((float)maxBarHeight * (1.0F - _Data_[i]));
                rt[i].Height = (float)maxBarHeight * _Data_[i];


            }
            g.FillRectangles(Brush_Rectangle, rt);


            if (_showruler_)
            {


            RectangleF SelectWindow;

            SolidBrush SelectWindowBrush = new SolidBrush(_SelectWindowColor_);
            SolidBrush SelectCursorBrush = new SolidBrush(_SelectCursorColor_);

 

                if (_LeftCursorFocused_)
                {
                    SelectWindow = new RectangleF(_RulerLeft_ * rect.Width - 10, 0, 20, maxBarHeight);
                    g.FillRectangle(SelectWindowBrush, SelectWindow);


                }


                if (_LeftCursorPending_)
                {
                    _RulerLeft_ = (float)_Mouse_X / rect.Width;
                }


                if (_RightCursorFocused_)
                {
                    SelectWindow = new RectangleF(_RulerRight_ * rect.Width - 10, 0, 20, maxBarHeight);
                    g.FillRectangle(SelectWindowBrush, SelectWindow);

                }


                if (_RightCursorPending_)
                {
                    _RulerRight_ = (float)_Mouse_X / rect.Width;
                }
            


                RectangleF WindowArea = new RectangleF(_RulerLeft_ * rect.Width, 0, (_RulerRight_-_RulerLeft_) * rect.Width, maxBarHeight);
                g.FillRectangle(SelectCursorBrush, WindowArea);
            }



            if (lastRect != this.ClientRectangle)
            {

                lastRect = this.ClientRectangle;
                genExtraTextBmp();
            }
            if(extraStrBmp!=null)
                    g.DrawImage(extraStrBmp, new Point((int)(maxBarHeight / 18f), (int)(maxBarHeight / 18F)));
                //g.DrawString(string.Format("Mouse:X={0},Y={1}.", _Mouse_X, _Mouse_Y), StrFont, StrBrush, maxBarHeight / 18F, maxBarHeight / 18F);


            myBuffer.Render(e.Graphics);
            g.Dispose();
            myBuffer.Dispose();
        }



        private void SpectrumShow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_showruler_)
            {
                _Mouse_X = e.X;
                _Mouse_Y = e.Y;

                if (_Mouse_X > RectWidth)
                    _Mouse_X = RectWidth;

                if (_Mouse_X < 0)
                    _Mouse_X = 0;


                if (Math.Abs(_RulerLeft_ * this.Width - (float)_Mouse_X) < 15)
                {
                    _LeftCursorFocused_ = true;
                }
                else
                {
                    _LeftCursorFocused_ = false;
                }


                if (Math.Abs(_RulerRight_ * this.Width - (float)_Mouse_X) < 15)
                {
                    _RightCursorFocused_ = true;
                }
                else
                {
                    _RightCursorFocused_ = false;
                }

                this.Invalidate();
            }
        }

        private void SpectrumShow_MouseDown(object sender, MouseEventArgs e)
        {

            if (_LeftCursorFocused_)
            {
                _LeftCursorPending_ = true;
            }
            else
            {
                _LeftCursorPending_ = false;
            }

            if (_RightCursorFocused_)
            {
                _RightCursorPending_ = true;

            }
            else
            {
                _RightCursorPending_ = false;
            }
        }

        private void SpectrumShow_MouseUp(object sender, MouseEventArgs e)
        {
            _LeftCursorPending_ = false;
            _RightCursorPending_ = false;
        }

        private void SpectrumShow_MouseLeave(object sender, EventArgs e)
        {

            _LeftCursorPending_ = false;
            _LeftCursorFocused_ = false;

            _RightCursorPending_ = false;
            _RightCursorFocused_ = false;

            this.Invalidate();

        }
    }
}
