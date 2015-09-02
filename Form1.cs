using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace Stage_GUI
{
    public partial class Form1 : Form
    {
        //work around for broken .NET 3.5
        private void SetDefaultPorts()
        {
            StreamWriter sw = new StreamWriter("props.txt");
            sw.Write("StagePort=COM1;");
            sw.Write("EFDPort=COM2;");
            sw.Close();
        }
        private string GetPort(string name)
        {
            if (!File.Exists("props.txt"))
                SetDefaultPorts();
            StreamReader sr;
            string props;
            try
            {
                sr = new StreamReader("props.txt");
                props = sr.ReadToEnd();
            }
            catch(Exception e)
            {
                LogLine("Unable to open props.txt file, making default");
                return "";
            }
            sr.Close();
            string[] prefs = props.Split(';');
            foreach(string item in prefs)
            {
                string[] parts = item.Split('=');
                if (parts.Length < 2)
                    continue;
                string ptype = parts[0];
                string port = parts[1];

                if (ptype.CompareTo(name) == 0)
                    return port;
            }
            return "";
        }
        private void SavePort(string name,string newport)
        {
            if (!File.Exists("props.txt"))
                SetDefaultPorts();
            StreamReader sr = new StreamReader("props.txt");
            string props = sr.ReadToEnd();
            sr.Close();
            
            StreamWriter sw = new StreamWriter("props.txt");
            string[] prefs = props.Split(';');
            foreach (string item in prefs)
            {
                string[] parts = item.Split('=');
                if (parts.Length < 2)
                    continue;
                string ptype = parts[0];
                string port = parts[1];

                if (ptype.CompareTo(name) == 0)
                {
                    port = newport;
                }
                sw.Write(ptype + "=" + port + ";");
            }
            sw.Close();
        }
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public Form1()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            InitializeComponent();
            textboxInc.Text = "1";
            textboxSpeed.Text = "1";
            openFileDialog1.FileName = "test.txt";
            string[] names = SerialPort.GetPortNames();
            /*comboBoxStagePorts.Items.Add("COM1");//Debug
            comboBoxStagePorts.Items.Add("COM2");//Debug
            comboBoxEFDPortNames.Items.Add("COM4");//Debug
            comboBoxEFDPortNames.Items.Add("COM3");//Debug
            */
            //string selectedStage = (string)Properties.Settings.Default["StagePort"];
            //string selectedEFD = (string)Properties.Settings.Default["EFDPort"];
            string selectedStage = GetPort("StagePort");
            string selectedEFD = GetPort("EFDPort");

            Form1.FillComboBoxNames(names, comboBoxStagePorts, selectedStage);
            Form1.FillComboBoxNames(names, comboBoxEFDPortNames, selectedEFD);
            ShowConsole.ShowConsoleWindow();
        }
        public static void FillComboBoxNames(string[] names, ComboBox cb, string selected)
        {
            foreach (string name in names)
            {
                cb.Items.Add(name);
            }
            if (cb.Items.Count > 0)
                cb.SelectedIndex = 0;
            if (selected != null && selected.Length > 0)
            {
                try
                {
                    for (int i = 0; i < cb.Items.Count; i++)
                    {
                        if (cb.Items[i].ToString().CompareTo(selected) == 0)
                        {
                            cb.SelectedIndex = i;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        public static void LogLine(string log)
        {
            Console.WriteLine(log);
        }
        #region thread
        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker1_ProgressChanged);
        }


        private class ThreadData
        {
            public string EFDName;
            public string StageName;
            public ThreadData(string en,string sn)
            {
                EFDName = en;
                StageName = sn;
            }
        }
        // This event handler is where the actual, 
        // potentially time-consuming work is done. 
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;
            ThreadData myData = (ThreadData)e.Argument;
            string EFDPortName = myData.EFDName;
            EFDControl efd = new EFDControl(EFDPortName);
            efd.Disable();
            string COM = myData.StageName;
            SerialPort stage = null;
            if (COM != EFDPortName)
            {
                try
                {
                    stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                    // Open Serial Port  
                    stage.Open();
                    LogLine("Stage Port Created.... Opening Port " + COM);
                }
                catch (Exception err)
                {
                    LogLine("Stage Port failed to open " + COM + " " + err.Message);
                    goto threadDone;
                }
            }
            else
            {
                //debug case
                stage = efd.EFDPort;
                stage.Close();
                stage.BaudRate = 115200;
                stage.Open();
            }
            // Directory Path  
            //string Name = File_name.Text;
            string path = openFileDialog1.FileName;


            // Bring Stages to Origin (0)
            // Reset Z-Stage
            string z_origin = "1HM Z=0\r";
            stage.Write(z_origin);
            string z_verification = ReadLine(stage);
            Console.WriteLine(z_verification + "\n");

            // Reset X/Y-Stage
            string xy_origin = "2HM X=0 Y=0\r";
            stage.Write(xy_origin);
            string xy_verification = ReadLine(stage);
            Console.WriteLine(xy_verification + "\n");

            LogLine("Begin Program");
            Thread.Sleep(2000);

            StreamReader readFile = new StreamReader(path);
            while(!worker.CancellationPending)
            {          
                string line;
                string[] data;
                line = readFile.ReadLine();
                if(line == null)
                {
                    LogLine("No more lines");
                    goto threadDone;
                }
                else
                {
                    LogLine("Read: "+line);
                    data = line.Split(',');
                    if(data.Length < 5)
                    {
                        LogLine("Not enough parameters");
                        goto threadDone;
                    }
                    const int scaler = 10;
                    UInt32 iXPos    = Convert.ToUInt32(data[0]);
                    UInt32 iYPos    = Convert.ToUInt32(data[1]);
                    UInt32 iZPos    = Convert.ToUInt32(data[2]);
                    UInt32 iSpeed   = Convert.ToUInt32(data[3]);
                    UInt32 iEFD     = Convert.ToUInt32(data[4]);

                    /*iXPos *= scaler;
                    iYPos *= scaler;
                    iZPos *= scaler;*/

                    string x_pos = iXPos.ToString();
                    string y_pos = iYPos.ToString();
                    string z_pos = iZPos.ToString();
                    string speed = iSpeed.ToString();
                    
                    // Generate Speed Command  
                    string Speed_CMD = "2HS X=" + speed + "\r";
                    LogLine(Speed_CMD);
                    stage.Write(Speed_CMD); 
                    string Speed_line = ReadLine(stage);
                    // Ensure Command was successfull
                    //Thread.Sleep(20);

                    // Generate Z-Stage Command
                    string Z_CMD = "1HM Z=" + z_pos + "\r";
                    LogLine(Z_CMD);
                    stage.Write(Z_CMD);
                    string Z_line = ReadLine(stage);
                    // Ensure Command was Successfull 
                    //Thread.Sleep(1000);

                    if (iEFD == 1)
                        efd.Enable();
                    else
                        efd.Disable();

                    // Generate X/Y-Stage Command
                    string XY_CMD = "2HM X=" + x_pos + " Y=" + y_pos + "\r";
                    double x_high_t, x_low_t;
                    double y_high_t, y_low_t;

                    double fudge = 3;
                    x_high_t = Convert.ToDouble(x_pos);
                    x_low_t = x_high_t;
                    x_high_t = x_high_t + fudge;

                    y_high_t = Convert.ToDouble(y_pos);
                    y_low_t = y_high_t;
                    y_high_t = y_high_t + fudge;

                    LogLine(XY_CMD);
                    stage.Write(XY_CMD);
                    string XY_line = ReadLine(stage);
                    int deadman = 20000;
                    while (deadman>0)
                    {
                        // Request XY-position
                        string xy_request = "2HW X Y\r";
                        stage.Write(xy_request);

                        // Read XY-position
                        string msg = ReadLine(stage);// stage.ReadLine();
                        if (msg.Length == 0 || msg.Length <= 4)
                            continue;
                        //msg = ReadLine(stage);// stage.ReadLine();

                        //:A xx.x,yy.y
                        msg=msg.Remove(0, 3);
                        string[] items = msg.Split(' ');
                        if(items.Length < 2)
                        {
                            LogLine("bad xy read");
                            goto threadDone;
                        }
                        double xd = Convert.ToDouble(items[0]);
                        double yd = Convert.ToDouble(items[1]);
                        //xd /= 10;
                        //yd /= 10;
                        if (xd <= x_high_t && xd >= x_low_t && yd <= y_high_t && yd >= y_low_t)
                        {
                            LogLine("Done moving XY");
                            break;
                        }
                        else
                        {
                            LogLine("X->"+xd.ToString()+" Y->"+yd.ToString());
                        }
                        //MessageBox.Show(msg);
                        deadman--;
                        Thread.Sleep(1);
                    }
                    if(deadman <= 0)
                    {
                        LogLine("Deadman timeout");
                        goto threadDone;
                    }
                    // Ensure Command was Successfull
                    //Thread.Sleep(1000);
                    //efd.Disable();
                }                
            }
threadDone:
            if(stage != null)
                stage.Close(); // Close COM Port 
            if (efd != null)
            {
                efd.Shutdown();
            }
            e.Cancel = true;
        }

        // This event handler deals with the results of the 
        // background operation. 
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown. 
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled  
                // the operation. 
                // Note that due to a race condition in  
                // the DoWork event handler, the Cancelled 
                // flag may not have been set, even though 
                // CancelAsync was called.
                //resultLabel.Text = "Canceled";
            }
            else
            {
                // Finally, handle the case where the operation  
                // succeeded.
                //resultLabel.Text = e.Result.ToString();
            }
            this.buttonRun.Enabled = true;
            // Enable the UpDown control. 
            //this.numericUpDown1.Enabled = true;

            // Enable the Start button.
            //startAsyncButton.Enabled = true;

            // Disable the Cancel button.
            //cancelAsyncButton.Enabled = false;
        }

        // This event handler updates the progress bar. 
        private void backgroundWorker1_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            //this.progressBar1.Value = e.ProgressPercentage;
        }


        #endregion



        // Serial Port Read line 
        static string ReadLine(SerialPort stage)
        {
            stage.ReadTimeout = 30000;
            try
            {
                string newline = "";
                while (true)
                {
                    int newb = stage.ReadByte();
                    Console.WriteLine("Rx: " + newb.ToString("X2") + "\t " + newb.ToString() + "\t" + System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb }));
                    if (newb == 0x03 || newb == 0x0A)
                        break;
                    string newdata = System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb });
                    newline += newdata;
                }
                return newline;
            }
            catch(Exception e)
            {
                LogLine("Serial Port error:" + e.Message);
                return null;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Run Program 

        // Manual X+ Movement
        private void X_move_Click(object sender, EventArgs e)
        {
            
            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Speed Command
            string Speed_desired = textboxSpeed.Text;
            string Speed_CMD = "2HS X="+Speed_desired+"\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 
            
            // X+ Command
            string tempinc = textboxInc.Text;
            UInt32 int_inc = Convert.ToUInt32(tempinc);
            int_inc *= 10;
            string INC = int_inc.ToString();//inc.Text; // Increment to move by (microns)
            string XY_CMD = "2HR X="+INC+" Y=0\r";
            Console.WriteLine(XY_CMD);
            stage.Write(XY_CMD);
            string XY_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Close COM Port
            stage.Close();  
        }

        // Manual X+ Movement
        private void X_move_neg_Click(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Speed Command
            string Speed_desired = textboxSpeed.Text;
            string Speed_CMD = "2HS X=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // X- Command
            string tempinc = textboxInc.Text;
            UInt32 int_inc = Convert.ToUInt32(tempinc);
            int_inc *= 10;
            string INC = int_inc.ToString();//inc.Text; // Increment to move by (microns)
            string XY_CMD = "2HR X=-"+INC+" Y=0\r";
            Console.WriteLine(XY_CMD);
            stage.Write(XY_CMD);
            string XY_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Close COM Port
            stage.Close();  


        }

        // Manual Y- Movement
        private void Y_move_neg_Click(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Speed Command
            string Speed_desired = textboxSpeed.Text;
            string Speed_CMD = "2HS Y=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Y- Command
            string tempinc = textboxInc.Text;
            UInt32 int_inc = Convert.ToUInt32(tempinc);
            int_inc *= 10;
            string INC = int_inc.ToString();//inc.Text; // Increment to move by (microns)
            string XY_CMD = "2HR X=0 Y=-"+INC+"\r";
            Console.WriteLine(XY_CMD);
            stage.Write(XY_CMD);
            string XY_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Close COM Port
            stage.Close();  


        }

        // Manual Y+ Movement
        private void Y_move_Click(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Speed Command
            string Speed_desired = textboxSpeed.Text;
            string Speed_CMD = "2HS Y="+Speed_desired+"\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Y+ Command
            string tempinc = textboxInc.Text;
            UInt32 int_inc = Convert.ToUInt32(tempinc);
            int_inc *= 10;
            string INC = int_inc.ToString();//inc.Text; // Increment to move by (microns)
            string XY_CMD = "2HR X=0 Y="+INC+"\r";
            Console.WriteLine(XY_CMD);
            stage.Write(XY_CMD);
            string XY_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Close COM Port
            stage.Close();  

        }

        // Manual Z+ Movement 
        private void Z_move_Click(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Speed Command
            string Speed_desired = textboxSpeed.Text;
            string Speed_CMD = "1HS Z=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Z+ Command
            string tempinc = textboxInc.Text;
            UInt32 int_inc = Convert.ToUInt32(tempinc);
            int_inc *= 10;
            string INC = int_inc.ToString();//inc.Text; // Increment to move by (microns)
            string Z_CMD = "1HR Z="+INC+"\r";
            Console.WriteLine(Z_CMD);
            stage.Write(Z_CMD);
            string Z_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Close COM Port
            stage.Close();  


        }

        // Manual Z- Movement 
        private void Z_move_neg_Click(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Speed Command
            string Speed_desired = textboxSpeed.Text;
            string Speed_CMD = "1HS Z=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Z- Command
            string tempinc = textboxInc.Text;
            UInt32 int_inc = Convert.ToUInt32(tempinc);
            int_inc *= 10;
            string INC = int_inc.ToString();//inc.Text; // Increment to move by (microns)
            string Z_CMD = "1HR Z=-"+INC+"\r";
            Console.WriteLine(Z_CMD);
            stage.Write(Z_CMD);
            string Z_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Close COM Port
            stage.Close();  


        }


        private void inputCOM_TextChanged(object sender, EventArgs e)
        {

        }

        private void File_name_TextChanged(object sender, EventArgs e)
        {

        }


        private void inc_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Speed_TextChanged(object sender, EventArgs e)
        {

        }


        private void Reset_Stage_Click(object sender, EventArgs e)
        {
            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }

            // Bring Stages to Origin (0)

            // Reset Z-Stage
            string z_origin = "1HM Z=0\r";
            stage.Write(z_origin);
            string z_verification = ReadLine(stage);
            Console.WriteLine(z_verification + "\n");

            // Reset X/Y-Stage
            string xy_origin = "2HM X=0 Y=0\r";
            stage.Write(xy_origin);
            string xy_verification = ReadLine(stage);
            Console.WriteLine(xy_verification + "\n");
            Thread.Sleep(500);

            stage.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fname = openFileDialog1.FileName;
            
        }


        private void buttonRun_Click(object sender, EventArgs e)
        {
            InitializeBackgroundWorker();
            string efd = comboBoxEFDPortNames.Items[comboBoxEFDPortNames.SelectedIndex].ToString();
            string stage = comboBoxStagePorts.Items[comboBoxStagePorts.SelectedIndex].ToString();

            backgroundWorker1.RunWorkerAsync(new ThreadData(efd,stage));
        }
        private void buttonGetXY_Click(object sender, EventArgs e)
        {
            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            try
            {
                stage.Open();
            }
            catch(Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Request XY-position
            string xy_request = "2HW X Y\r";
            stage.Write(xy_request);

            // Read XY-position
            string msg = stage.ReadLine();
            MessageBox.Show(msg);

            // Close Serial Port
            stage.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxStagePorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            try
            {
                stage.Open();
            }
            catch (Exception err)
            {
                LogLine("Open failed " + err.Message);
                return;
            }
            // Request XY-position
            string xy_request = "1HW Z\r";
            stage.Write(xy_request);

            // Read XY-position
            string msg = stage.ReadLine();
            MessageBox.Show(msg);

            // Close Serial Port
            stage.Close();

        }

        private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default["StagePort"]=comboBoxStagePorts.SelectedText;
            //Properties.Settings.Default.Save();
            SavePort("StagePort", comboBoxStagePorts.Items[comboBoxStagePorts.SelectedIndex].ToString());
        }

        private void comboBoxEFDPortNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default["EFDPort"] = comboBoxEFDPortNames.SelectedText;
            //Properties.Settings.Default.Save();
            SavePort("EFDPort", comboBoxEFDPortNames.Items[comboBoxEFDPortNames.SelectedIndex].ToString());
        }

        private void buttonGetXY_Click()
        {

        }

    }
}
