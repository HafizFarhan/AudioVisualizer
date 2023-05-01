using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing.Drawing2D;
using System.Threading;
using PropertyGridEx;
using System.Linq;
using System.Configuration;
using AudioVisualizer;

namespace AudioLoopBack
{
    public partial class mainform : Form
    {
        IWaveIn waveIn;
        WaveOut waveOut;
        NAudio.Utils.CircularBuffer cb = new NAudio.Utils.CircularBuffer(80000);

        Thread DataProcess;
        bool DataProcessStatus = false;
        private System.Threading.Timer Upload_Timer;

        float[] RawFFTData_Old = new float[256];
        float[] RawFFTData_New = new float[256];
        float[] AdjustFFTData_New = new float[256];
        float[] AdjustFFTData_Old = new float[256];

        float[] FFT_DATA_HID = new float[32];

        int SelectWidth = 63;
        int SelectIndex = 2;

        WaveShowPanel wp = new WaveShowPanel();

        public class WaveShowPanel : Panel
        {
            public WaveShowPanel()
            {
                //        base.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                //ControlStyles.ResizeRedraw |
                //ControlStyles.AllPaintingInWmPaint, true);
                base.BackColor = Color.White;
            }
        }

        public mainform()
        {
            InitializeComponent();
            LoadWasapiDevicesCombo();

            HidDev.VendorId = 0x0416;
            HidDev.ProductId = 0x5022;

            HidDev.CheckDevicePresent();

            spectrumShow1.BackgroundColor = GlobalSettings.Default.Background_Color_Spect_C;
            //waveShow1.BackgroundColor = GlobalSettings.Default.Background_Color_Wave_C;
            spectrumShow1.ShowRuler = GlobalSettings.Default.ShowCursor;
            spectrumShow1.RulerLeft = GlobalSettings.Default.Cursor_L;
            spectrumShow1.RulerRight = GlobalSettings.Default.Cursor_R;

            CustomPropertyCollection collection_showcontrol = new CustomPropertyCollection();

            collection_showcontrol.Add(new CustomProperty("BC_Spect", "BackgroundColor", "Appearance", "Background color of control specturm show.", spectrumShow1));
            collection_showcontrol.Add(new CustomProperty("ShowCursor", "ShowRuler", "Appearance", "The cursor of control specturm show.", spectrumShow1));


           // collection_showcontrol.Add(new CustomProperty("BC_Wave", "BackgroundColor", "Appearance", "Background color of control wave show.", waveShow1));

            //propertyGrid1.SelectedObject = collection_showcontrol;

            buttonStartRecording_Click(this, null);
        }

        private void LoadWasapiDevicesCombo()
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            MMDeviceCollection deviceCol = deviceEnum.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            Collection<MMDevice> devices = new Collection<MMDevice>();

            foreach (MMDevice device in deviceCol)
            {
                devices.Add(device);


            }
            this.comboDevices.DataSource = devices;
            this.comboDevices.DisplayMember = "FriendlyName";
        }

        private void buttonStartRecording_Click(object sender, EventArgs e)
        {

            var item=(MMDevice)comboDevices.SelectedItem;
            if(item.FriendlyName.Contains("Microphone"))
            {
                //microphone code goes here
            }
            else
            {
                waveIn = new WasapiLoopbackCapture((MMDevice)comboDevices.SelectedItem);

                // go with the default format as WASAPI doesn't support SRC


                waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
                waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped);
                waveIn.StartRecording();
                //tbx_msg.AppendText("Bits/Sample:"+waveIn.WaveFormat.BitsPerSample+ "\r\n-------\r\n");
                //tbx_msg.AppendText("Channel num:" + waveIn.WaveFormat.Channels + "\r\n-------\r\n");
                //tbx_msg.AppendText("Encoding:" + waveIn.WaveFormat.Encoding + "\r\n-------\r\n");
                //tbx_msg.AppendText("Sample rate:" + waveIn.WaveFormat.SampleRate + "\r\n-------\r\n");


                //buttonStartRecording.Enabled = false;
                DataProcess = new Thread(Processing);
                DataProcess.IsBackground = true;
                DataProcess.Priority = ThreadPriority.Normal;
                DataProcessStatus = true;
                DataProcess.Start();
            }
        

            
        }

        public static float maxValue(float[] s)
        {
            float temp = s[0];
            for (int i = 0; i < s.Length; i++)
            {
                if (temp <= s[i])
                {
                    temp = s[i];
                }
            }
            return temp;
        }


        void Processing()
        {
            int i;
            while (DataProcessStatus)
            {
                float[] Data = new float[512];
                byte[] t = new byte[2048 * 8];
                byte[] d = new byte[8];
                if (cb.Count >= 2048 * 8)
                {

                    cb.Read(t, 0, 2048 * 8);

                    for (i = 0; i < 512; i++)
                    {

                        Data[i] = (BitConverter.ToSingle(t, i * 4 * 4) + BitConverter.ToSingle(t, 4 * (i * 4 + 1))) / 2.0F;
                        //Data[i] = (BitConverter.ToSingle(raw_data, i * 4) + BitConverter.ToSingle(raw_data, (i +1)* 4));

                    }

                    //waveShow1.Data = Data;


                    //for (int piq = 5; piq < 37; piq++)
                    //{
                    //    FFT_DATA_HID[piq - 5] = (Data[piq * 13] + 1)/2.0F;
                    //}


                    float[] Data_RE = new float[512];
                    float[] Data_IM = new float[512];


                    for (i = 0; i < Data_RE.Length; i++)
                        Data_RE[i] = (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (Data_RE.Length - 1)))) * Data[i];


                    for (i = 0; i < Data_IM.Length; i++)
                        Data_IM[i] = 0;

                    int n = TWFFT.FFT(Data_RE, Data_IM);
                    float[] Mod = new float[n / 2];

                    for (int q = 0; q < n / 2; q++)
                    {
                        float am = (float)Math.Sqrt(Math.Pow(Data_RE[q], 2) + Math.Pow(Data_IM[q], 2)) / 5F;
                        if (am > 1)
                            am = 1;
                        // am = 1;
                        Mod[q] = am;// (float)Math.Pow((double)q / 255F, 0.3) * am;//(float)Math.Pow(am, 0.5);
                    }

                    //spectrumShow1.Data =Mod;

                    //B_i' = B_(i-1)' * s' + B_i * (1 - s')

                    Array.Copy(Mod, RawFFTData_New, 256);


                    for (i = 0; i < 256; i++)
                    {
                        AdjustFFTData_New[i] = AdjustFFTData_Old[i] * 0.5F + RawFFTData_New[i] * (1.0F - 0.5F);
                    }


                    //Array.Copy(RawFFTData_New, RawFFTData_Old, 256);
                    Array.Copy(AdjustFFTData_New, AdjustFFTData_Old, 256);



                    spectrumShow1.Data = AdjustFFTData_New;

                    SelectWidth = (int)((spectrumShow1.RulerRight - spectrumShow1.RulerLeft) * 256);
                    SelectIndex = (int)(spectrumShow1.RulerLeft * 256);

                    if (SelectWidth < 32)
                    {
                        SelectWidth = 32;
                    }
                    if (SelectWidth > 256 - 33)
                    {
                        SelectWidth = 256 - 33;
                    }
                    if (SelectIndex > 256 - 32)
                    {
                        SelectIndex = 256 - 32;
                    }
                    //Array.Copy(Mod, FFT_DATA_HID, 32);
                    for (int iq = 0; iq < 32; iq++)
                    {
                        float[] temp_d = new float[SelectWidth / 32];
                        Array.Copy(AdjustFFTData_New, iq * SelectWidth / 32 + SelectIndex, temp_d, 0, SelectWidth / 32);
                        FFT_DATA_HID[iq] = maxValue(temp_d);// (float)Math.Pow(maxValue(temp_d), 0.5);//(AdjustFFTData_New[iq * 8+0] + AdjustFFTData_New[iq * 8+1] + AdjustFFTData_New[iq * 8+2] + AdjustFFTData_New[iq * 8+3] + AdjustFFTData_New[iq * 8+4] + AdjustFFTData_New[iq * 8+5] + AdjustFFTData_New[iq * 8+6] + AdjustFFTData_New[iq * 8+7])/4.0F;
                    }

                }
                Thread.Sleep(2);
            }

        }
        void waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                //buttonStartRecording.Enabled = true;
                

            }
        }


        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
           
            if (this.InvokeRequired)
            {
                //Debug.WriteLine("Data Available");
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {


                cb.Write(e.Buffer, 0, e.BytesRecorded);

            }
        }

        void StopRecording()
        {
            waveIn.StopRecording();
        }

        void StopMatters()
        {
            if (waveIn != null)
            {
                StopRecording();
            }
            DataProcessStatus = false;
            DataProcess = null;
            cb.Reset();

        }

        private void buttonStopRecording_Click(object sender, EventArgs e)
        {

            StopMatters();

        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMatters();
            GlobalSettings.Default.Background_Color_Spect_C = spectrumShow1.BackgroundColor;
          //  GlobalSettings.Default.Background_Color_Wave_C = waveShow1.BackgroundColor;
            GlobalSettings.Default.ShowCursor = spectrumShow1.ShowRuler;
            GlobalSettings.Default.Cursor_L = spectrumShow1.RulerLeft;
            GlobalSettings.Default.Cursor_R = spectrumShow1.RulerRight;
            GlobalSettings.Default.Save();
        }




        private void CloseAll()
        {
            //IsUpload = false;
            // HasCap = false;
            //lbl_t.Text = "";
            //lbl_h.Text = null;
            if (Upload_Timer != null)
            {
                Upload_Timer.Dispose();
            }


        }
        public void Upload_Task(object obj)
        {
            byte[] buf = new byte[65];
            buf[0] = 0;
            for (int i = 0; i < 32; i++)
            {
                 buf[i + 1] = (byte)(FFT_DATA_HID[i] * 16.0F); //This is for rgb led matrix

                // buf[i + 1] = (byte)(FFT_DATA_HID[i] * 255F); //This is for led strip
                //buf[i + 1] = (byte)(FFT_DATA_HID[i] * 239F); //This is for tft glcd
            }

            buf[33] = spectrumShow1.BackgroundColor.R;
            buf[34] = spectrumShow1.BackgroundColor.G;
            buf[35] = spectrumShow1.BackgroundColor.B;

            //buf[36] = waveShow1.BackgroundColor.R;
            //buf[37] = waveShow1.BackgroundColor.G;
            //buf[38] = waveShow1.BackgroundColor.B;
            HidDev.SpecifiedDevice.SendData(buf);
        }
        private void StartUpload()
        {
            // Upload_T = new Thread(Upload_Task);
            //IsUpload = true;
            Upload_Timer = new System.Threading.Timer(new TimerCallback(Upload_Task), null, 0, 8);

            //Upload_T.Start();


        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //24, 189
            // visualizer vs = new visualizer(Data, e);
        }

        private void HidDev_OnDataRecieved(object sender, UsbLibrary.DataRecievedEventArgs args)
        {

        }

        private void HidDev_OnDataSend(object sender, EventArgs e)
        {

        }

        private void HidDev_OnDeviceArrived(object sender, EventArgs e)
        {

        }

        private void HidDev_OnDeviceRemoved(object sender, EventArgs e)
        {

        }

        private void HidDev_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            //tbx_msg.AppendText("The extra device connected.\r\n-------\r\n");
            StartUpload();
        }

        private void HidDev_OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            //tbx_msg.AppendText("The extra device disconnected.\r\n-------\r\n");
            CloseAll();
        }

        //for usb
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            HidDev.RegisterHandle(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            HidDev.ParseMessages(ref m);
            base.WndProc(ref m);	// pass message on to base form
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonStartRecording_Click(this, null);
        }
    }
}
