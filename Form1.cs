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
        public Form1()
        {
            InitializeComponent();
            string[] names = SerialPort.GetPortNames();
            foreach(string name in names)
                comboBoxPorts.Items.Add(name);
                comboBoxPorts.SelectedIndex = 0; 
    
        }

        // Serial Port Read line 
        static string ReadLine(SerialPort stage)
        {
            string newline = "";
            while (true)
            {
                int newb = stage.ReadByte();
                Console.WriteLine("Rx: " + newb.ToString("X2") + "\t " + newb.ToString() + "\t" + System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb }));
                if (newb == 0x03)    
                    break;
                newline.Insert(newline.Length, System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb }));
            }
            return newline;
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Run Program 
        private void button2_Click(object sender, EventArgs e)
        {
            // Define Serial Port  
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            stage.Open();
            Console.WriteLine("Serial Port Created.... Opening Port \n");

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

            Thread.Sleep(2000);

                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] data;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        data = line.Split(',');
                        string x_pos = data.GetValue(0).ToString(); 
                        string y_pos = data.GetValue(1).ToString();
                        string z_pos = data.GetValue(2).ToString();
                        string speed = data.GetValue(3).ToString();

                        // Generate Speed Command  
                        string Speed_CMD = "2HS X=" + speed + "\r";
                        stage.Write(Speed_CMD);
                        string Speed_line = ReadLine(stage);
                        // Ensure Command was successfull
                        Thread.Sleep(20);

                        // Generate Z-Stage Command
                        string Z_CMD = "1HM Z=" + z_pos + "\r";
                        stage.Write(Z_CMD);
                        string Z_line = ReadLine(stage);
                        // Ensure Command was Successfull 
                        Thread.Sleep(1000);

                        // Generate X/Y-Stage Command
                        string XY_CMD = "2HM X=" + x_pos + " Y=" + y_pos + "\r";
                        stage.Write(XY_CMD);
                        string XY_line = ReadLine(stage);
                        // Ensure Command was Successfull
                        Thread.Sleep(1000);
                    }
                    
                }
            stage.Close(); // Close COM Port 
            

        }

        // Manual X+ Movement
        private void X_move_Click(object sender, EventArgs e)
        {
            
            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            stage.Open();

            // Speed Command
            string Speed_desired = Speed.Text;
            string Speed_CMD = "2HS X="+Speed_desired+"\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 
            
            // X+ Command
            string INC = inc.Text; // Increment to move by (microns)
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            stage.Open();

            // Speed Command
            string Speed_desired = Speed.Text;
            string Speed_CMD = "2HS X=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // X- Command
            string INC = inc.Text; // Increment to move by (microns)
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            stage.Open();

            // Speed Command
            string Speed_desired = Speed.Text;
            string Speed_CMD = "2HS Y=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Y- Command
            string INC = inc.Text; // Increment to move by (microns)
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            stage.Open();

            // Speed Command
            string Speed_desired = Speed.Text;
            string Speed_CMD = "2HS Y="+Speed_desired+"\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Y+ Command
            string INC = inc.Text; // Increment to move by (microns)
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            stage.Open();

            // Speed Command
            string Speed_desired = Speed.Text;
            string Speed_CMD = "1HS Z=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Z+ Command
            string INC = inc.Text; // Increment to move by (microns)
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open COM Port  
            stage.Open();

            // Speed Command
            string Speed_desired = Speed.Text;
            string Speed_CMD = "1HS Z=" + Speed_desired + "\r";
            Console.WriteLine(Speed_CMD);
            stage.Write(Speed_CMD);
            string Speed_line = ReadLine(stage);
            Thread.Sleep(20); // Pause 

            // Z- Command
            string INC = inc.Text; // Increment to move by (microns)
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            stage.Open();

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

        private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            // Define Serial Port  
            //string COM = inputCOM.Text;
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            stage.Open();
            
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
            string COM = comboBoxPorts.Text;
            System.IO.Ports.SerialPort stage = new System.IO.Ports.SerialPort(COM, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            // Open Serial Port  
            stage.Open();

            // Request XY-position
            string xy_request = "1HW Z\r";
            stage.Write(xy_request);

            // Read XY-position
            string msg = stage.ReadLine();
            MessageBox.Show(msg);

            // Close Serial Port
            stage.Close();

        }



    }
}
