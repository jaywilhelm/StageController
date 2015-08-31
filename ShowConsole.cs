using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
namespace Stage_GUI
{
    public class ShowConsole
    {
        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
    }
    /*[DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeConsole();

    [DllImport("kernel32", SetLastError = true)]
    static extern bool AttachConsole(int dwProcessId);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [STAThread]
    static void ShowConsole()
    {
        //TODO: better handling of command args, (handle help (--help /?) etc.)
        string mode = args.Length > 0 ? args[0] : "gui"; //default to gui

        if (mode == "gui")
        {
            MessageBox.Show("Welcome to GUI mode");

            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
        else if (mode == "console")
        {

            //Get a pointer to the forground window.  The idea here is that
            //IF the user is starting our application from an existing console
            //shell, that shell will be the uppermost window.  We'll get it
            //and attach to it
            IntPtr ptr = GetForegroundWindow();

            int  u;

            GetWindowThreadProcessId(ptr, out u);

            Process process = Process.GetProcessById(u);

            if (process.ProcessName == "cmd" )    //Is the uppermost window a cmd process?
            {
                AttachConsole(process.Id);

                //we have a console to attach to ..
                Console.WriteLine("hello. It looks like you started me from an existing console.");
            }
            else
            {
                //no console AND we're in console mode ... create a new console.

                AllocConsole();

                Console.WriteLine(@"hello. It looks like you double clicked me to start
                AND you want console mode.  Here's a new console.");
                Console.WriteLine("press any key to continue ...");
                Console.ReadLine();       
            }

            FreeConsole();
        }
        }
    }*/
}

