using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO.Ports;
namespace Stage_GUI
{
    public class EFDControl
    {
        private SerialPort EFDPort;
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
                Form1.LogLine("EFD Port failed to open " + name);
                return;
            }
            Form1.LogLine("EFD Port Created.... Opening Port " + name);
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
                Form1.LogLine("EFD Port closed");
                return;
            }
            EFDPort.WriteLine("1");
            Form1.LogLine("EFD Port Enable");
        }
        public void Disable()
        {
            if (EFDPort == null || !EFDPort.IsOpen)
            {
                Form1.LogLine("EFD Port closed");
                return;
            }
            EFDPort.WriteLine("0");
            Form1.LogLine("EFD Port Disable");
        }
    }
}
