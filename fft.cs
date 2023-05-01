

using System;
using System.Collections.Generic;
using System.Text;


namespace AudioLoopBack
{
    /// <summary>
    /// (Fast Fourier Transform)。 
    /// </summary>
    public class TWFFT
    {
        private TWFFT()
        {
        }
        private static void bitrp(float[] xreal, float[] ximag, int n)
        {
            // Bit-reversal Permutation
            int i, j, a, b, p;
            for (i = 1, p = 0; i < n; i *= 2)
            {
                p++;
            }
            for (i = 0; i < n; i++)
            {
                a = i;
                b = 0;
                for (j = 0; j < p; j++)
                {
                    b = b * 2 + a % 2;
                    a = a / 2;
                }
                if (b > i)
                {
                    float t = xreal[i];
                    xreal[i] = xreal[b];
                    xreal[b] = t;
                    t = ximag[i];
                    ximag[i] = ximag[b];
                    ximag[b] = t;
                }
            }
        }
        public static int FFT(float[] xreal, float[] ximag)
        {
            
            int n = 2;
            while (n <= xreal.Length)
            {
                n *= 2;
            }
            n /= 2;
           
            float[] wreal = new float[n / 2];
            float[] wimag = new float[n / 2];
            float treal, timag, ureal, uimag, arg;
            int m, k, j, t, index1, index2;
            bitrp(xreal, ximag, n);
            
            arg = (float)(-2 * Math.PI / n);
            treal = (float)Math.Cos(arg);
            timag = (float)Math.Sin(arg);
            wreal[0] = 1.0f;
            wimag[0] = 0.0f;
            for (j = 1; j < n / 2; j++)
            {
                wreal[j] = wreal[j - 1] * treal - wimag[j - 1] * timag;
                wimag[j] = wreal[j - 1] * timag + wimag[j - 1] * treal;
            }
            for (m = 2; m <= n; m *= 2)
            {
                for (k = 0; k < n; k += m)
                {
                    for (j = 0; j < m / 2; j++)
                    {
                        index1 = k + j;
                        index2 = index1 + m / 2;
                        t = n * j / m;    
                        treal = wreal[t] * xreal[index2] - wimag[t] * ximag[index2];
                        timag = wreal[t] * ximag[index2] + wimag[t] * xreal[index2];
                        ureal = xreal[index1];
                        uimag = ximag[index1];
                        xreal[index1] = ureal + treal;
                        ximag[index1] = uimag + timag;
                        xreal[index2] = ureal - treal;
                        ximag[index2] = uimag - timag;
                    }
                }
            }
            return n;
        }
        public static int IFFT(float[] xreal, float[] ximag)
        {
            
            int n = 2;
            while (n <= xreal.Length)
            {
                n *= 2;
            }
            n /= 2;
            float[] wreal = new float[n / 2];
            float[] wimag = new float[n / 2];
            float treal, timag, ureal, uimag, arg;
            int m, k, j, t, index1, index2;
            bitrp(xreal, ximag, n);
           
            arg = (float)(2 * Math.PI / n);
            treal = (float)(Math.Cos(arg));
            timag = (float)(Math.Sin(arg));
            wreal[0] = 1.0f;
            wimag[0] = 0.0f;
            for (j = 1; j < n / 2; j++)
            {
                wreal[j] = wreal[j - 1] * treal - wimag[j - 1] * timag;
                wimag[j] = wreal[j - 1] * timag + wimag[j - 1] * treal;
            }
            for (m = 2; m <= n; m *= 2)
            {
                for (k = 0; k < n; k += m)
                {
                    for (j = 0; j < m / 2; j++)
                    {
                        index1 = k + j;
                        index2 = index1 + m / 2;
                        t = n * j / m;    
                        treal = wreal[t] * xreal[index2] - wimag[t] * ximag[index2];
                        timag = wreal[t] * ximag[index2] + wimag[t] * xreal[index2];
                        ureal = xreal[index1];
                        uimag = ximag[index1];
                        xreal[index1] = ureal + treal;
                        ximag[index1] = uimag + timag;
                        xreal[index2] = ureal - treal;
                        ximag[index2] = uimag - timag;
                    }
                }
            }
            for (j = 0; j < n; j++)
            {
                xreal[j] /= n;
                ximag[j] /= n;
            }
            return n;
        }
    }
}
