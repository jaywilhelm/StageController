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
    public partial class MainGUI : Form
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
        public MainGUI()
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

            MainGUI.FillComboBoxNames(names, comboBoxStagePorts, selectedStage);
            MainGUI.FillComboBoxNames(names, comboBoxEFDPortNames, selectedEFD);
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
            StageController stage;

            string COM = myData.StageName;
            if (COM != EFDPortName)//ugly hack for testing with 1 device
                stage = new StageController(COM);
            else
            {
                stage = new StageController();
                stage.sp = efd.EFDPort;
            }
            //SerialPort stage = null;
            // Directory Path  
            //string Name = File_name.Text;
            string path = openFileDialog1.FileName;

            stage.ResetXYZ();
            stage.GetStatus();
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
                    const int scaler = 1;
                    Int32 iXPos = Convert.ToInt32(data[0]);
                    Int32 iYPos = Convert.ToInt32(data[1]);
                    Int32 iZPos = Convert.ToInt32(data[2]);
                    Int32 iSpeed = Convert.ToInt32(data[3]);
                    Int32 iEFD = Convert.ToInt32(data[4]);

                    iXPos *= scaler;
                    iYPos *= scaler;
                    iZPos *= scaler;

                    stage.SetSpeed(StageController.StageAxis.Z, new Int32[] { iSpeed });
                    stage.Move(StageController.StageAxis.Z, new Int32[] { iZPos });
                    if (iEFD == 1)
                        efd.Enable();
                    else
                        efd.Disable();
                    stage.Move(StageController.StageAxis.XY, new Int32[] { iXPos, iYPos });

                    // Ensure Command was Successfull
                    //Thread.Sleep(1000);
                    //efd.Disable();
                }                
            }
threadDone:
            if(stage != null)
                stage.Shutdown(); // Close COM Port 
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
                    newline.Insert(newline.Length, System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb }));
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
            Thread.Sleep(150); // Pause 
            LogLine("X " + stage.ReadExisting());
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
            LogLine("Done X+");

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

        private void button4_Click(object sender, EventArgs e)
        {
            SerialPort sp = new SerialPort("COM13", 115200);
            sp.Open();
            sp.Write(textBoxSend.Text + "\r");
            LogLine(sp.ReadExisting());
            Thread.Sleep(1000);
            LogLine(sp.ReadExisting());
            sp.Close();
        }

    }
}
