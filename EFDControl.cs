using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
namespace Stage_GUI
{
    public class StageController
    {
        public SerialPort sp;
        private static string stagePrecision = "H";
        private static int standardTimeout = 30000;
        public StageController()
        {
            MainGUI.LogLine("Using Existing Port");
        }
        public StageController(string name)
        {
            try
            {
                sp = new SerialPort(name, 115200);
                sp.Open();
            }
            catch (Exception e)
            {
                sp = null;
                MainGUI.LogLine("StageController Port failed to open " + name);
                return;
            }
            MainGUI.LogLine("StageController Port Created.... Opening Port " + name);
        }
        ~StageController()
        {
            Shutdown();
            sp = null;
        }
        public void Shutdown()
        {
            if(sp != null)
                if (sp.IsOpen)
                    sp.Close();
        }
        public string GetStatus()
        {
            sp.Write("1/\r");
            Thread.Sleep(1000);
            string ret = sp.ReadExisting();
            return ret;
        }
        public string WaitForA(int timeout)
        {
            sp.ReadTimeout = timeout;
            try
            {
                string newline = "";
                while (true)
                {
                    int newb = sp.ReadByte();
                    Console.WriteLine("Rx: " + newb.ToString("X2") + "\t " + newb.ToString() + "\t" + System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb }));
                    if (newb == 0x03)
                        continue;
                    if(newb == 0x0A || newb == 'A')
                        break;
                    string newdata = System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb });
                    newline += newdata;
                }
                return newline;
            }
            catch (Exception e)
            {
                MainGUI.LogLine("Serial Port error:" + e.Message);
                return null;
            }
        }
        public string WaitForNewLine(int timeout)
        {
            sp.ReadTimeout = timeout;
            try
            {
                string newline = "";
                while (true)
                {
                    int newb = sp.ReadByte();
                    Console.Write("Rx: " + newb.ToString("X2") + "\t " + newb.ToString() + "\t" + System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb })+"\t");
                    if (newb == 0x03)
                        continue;
                    if (newb == 0x0A)
                        break;
                    string newdata = System.Text.Encoding.UTF8.GetString(new byte[] { (byte)newb });
                    newline += newdata;
                }
                return newline;
            }
            catch (Exception e)
            {
                MainGUI.LogLine("Serial Port error:" + e.Message);
                return null;
            }
        }
        public enum StageAxis
        {
            Z,
            XY
        };
        private string GetStageString(StageAxis axis)
        {
            if (axis == StageAxis.Z)
                return "1";
            else
                return "2";
        }
        public Int32[] GetStagePosition(StageAxis axis)
        {
            string command = GetStageString(axis);
            string precision = "H";
            command += precision + " ";

            string whichaxis = "";
            if (axis == StageAxis.Z)
                whichaxis = "Z";
            else if (axis == StageAxis.XY)
                whichaxis = "X Y";

            command += whichaxis;

            MainGUI.LogLine(command);
            if (sp.IsOpen)
            {
                string old = sp.ReadExisting();
                sp.Write(command + "\r");
            }

            string result = WaitForNewLine(standardTimeout);
            Int32[] retInt = null;
            if (axis == StageAxis.Z)
            {
                int zpos = 0;
                string zpart = result.Substring(2);
                zpos = Convert.ToInt32(zpart);
                retInt = new Int32[] { zpos };
            }
            else if (axis == StageAxis.XY)
            {
                int xpos=0,ypos = 0;
                string zpart = result.Substring(2);//remove :A_
                string[] nums = zpart.Split(' ');
                xpos = Convert.ToInt32(nums[0]);
                ypos = Convert.ToInt32(nums[1]);
                retInt = new Int32[] { xpos, ypos };
            }
            return retInt;
        }
        public int Move(StageAxis axis,Int32[] ammount)
        {
            string command = GetStageString(axis);
            string precision = "H";


            string numbers = "";
            if (axis == StageAxis.Z)
                numbers = "Z=" + ammount[0].ToString();
            else if (axis == StageAxis.XY)
                numbers = "X=" + ammount[0].ToString() + " Y=" + ammount[1].ToString();

            command += precision + "M " + numbers;

            MainGUI.LogLine(command);
            if (sp.IsOpen)
            {
                string old = sp.ReadExisting();
                sp.Write(command + "\r");
            }

            string result = WaitForNewLine(standardTimeout);
            MainGUI.LogLine(result);
            return 0;
        }
        public int ResetXYZ()
        {
            Move(StageAxis.Z,new int[]{0});
            Move(StageAxis.XY,new int[]{0,0});
            return 0;
        }
       /* public int Where(StageAxis axis, ref double[] where)
        {
            string command = GetStageString(axis);
            string precision = stagePrecision;
            
            return 0;
        }*/
        public int SetSpeed(StageAxis axis, Int32[] speeds)
        {
            string command = GetStageString(axis);
            string precision = "H";

            string numbers = "";
            if (axis == StageAxis.Z)
                numbers = "Z=" + speeds[0].ToString();
            else if (axis == StageAxis.XY)
                numbers = "X=" + speeds[0].ToString() + " Y=" + speeds[1].ToString();

            command += precision + "S " + numbers;

            MainGUI.LogLine(command);
            if (sp.IsOpen)
            {
                string old = sp.ReadExisting();
                sp.Write(command + "\r");
            }

            string result = WaitForNewLine(standardTimeout);
            MainGUI.LogLine(result);
            return 0;
        }
    }
    public class EFDControl
    {
        public SerialPort EFDPort;
        public EFDControl(string name)
        {
            try
            {
                EFDPort = new SerialPort(name, 9600);
                EFDPort.Open();
            }
            catch(Exception e)
            {
                EFDPort = null;
                MainGUI.LogLine("EFD Port failed to open " + name);
                return;
            }
            MainGUI.LogLine("EFD Port Created.... Opening Port " + name);
        }
        ~EFDControl()
        {
            EFDPort.Close();
            EFDPort = null;
        }
        public void Shutdown()
        {
            if (EFDPort != null)
            {
                Disable();
                EFDPort.Close();
            }
        }
        public void Enable()
        {
            if(EFDPort == null || !EFDPort.IsOpen)
            {
                MainGUI.LogLine("EFD Port closed");
                return;
            }
            EFDPort.WriteLine("1");
            MainGUI.LogLine("EFD Port Enable");
        }
        public void Disable()
        {
            if (EFDPort == null || !EFDPort.IsOpen)
            {
                MainGUI.LogLine("EFD Port closed already");
                return;
            }
            EFDPort.WriteLine("0");
            MainGUI.LogLine("EFD Port Disable");
        }
    }
}
