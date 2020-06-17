using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LauncherWithAccSupport
{
    internal class Program
    {
        private static Process _clientProcess;
        private static byte _clientAnticheat;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static bool Routine(int dwCtrlType)
        {
        if (dwCtrlType == 2 && !Program._clientProcess.HasExited)
            _clientProcess.Kill();
        return false;
        }
    }
}
