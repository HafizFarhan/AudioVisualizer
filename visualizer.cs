using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class visualizer
    {
        public visualizer(float[] Data, PaintEventArgs e)
        {
            SolidBrush Brush_White = new SolidBrush(Color.White);
            SolidBrush Brush_BackColor = new SolidBrush(Color.FromArgb(255, 255, 153, 0));
            SolidBrush Brush_line = new SolidBrush(Color.Black);


            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;

            g.Clear(Color.White);

            Rectangle rect = e.ClipRectangle;

            int bar_width = (int)((double)rect.Width / Data.Length);
            int max_bar_height = rect.Height;

            if (bar_width * Data.Length < rect.Width)
                bar_width++;


            g.SmoothingMode = SmoothingMode.HighSpeed; 
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed; 


            Point[] pf = new Point[Data.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                //int bar_height = (int)((float)max_bar_height * Data[i]);
                //g.FillRectangle(Brush_BackColor, new Rectangle(i * bar_width, max_bar_height - bar_height, bar_width, bar_height));
                pf[i].Y = (int)((float)max_bar_height * Data[i]);
                pf[i].X = i * bar_width;



            }
            g.DrawLines(new Pen(Brush_BackColor, 2), pf);
            g.DrawLine(new Pen(Brush_line, 1), new Point(0, max_bar_height / 2), new Point(rect.Width, max_bar_height / 2));
            myBuffer.Render(e.Graphics);
            g.Dispose();
            myBuffer.Dispose();
        }
    }
}
