using Launcher;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LauncherWithAccSupport
{
    public partial class Form1 : Form
    {
        private static Process _clientProcess;
        private static byte _clientAnticheat;
        public Form1()
        {
           InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "G:\\Fortnite\\workaround\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe";
            string str2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Platanium.dll");
            string arguments = $"-AUTH_LOGIN=unused AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=Prod -epicportal -epiclocale=en-us -AUTH_PASSWORD={this.textBox1.Text}";
            if (!File.Exists(str2))
            {
                int num2 = (int)MessageBox.Show("\"Plantanium.dll\" was not found, please make sure it exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            } else { 
                _clientProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo(path, arguments)
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
                    }
                };

                _clientProcess.Start();
                IntPtr hProcess = Win32.OpenProcess(1082, false, _clientProcess.Id);
                IntPtr procAddress = Win32.GetProcAddress(Win32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                uint num4 = (uint)((str2.Length + 1) * Marshal.SizeOf(typeof(char)));
                IntPtr num5 = Win32.VirtualAllocEx(hProcess, IntPtr.Zero, num4, 12288U, 4U);
                Win32.WriteProcessMemory(hProcess, num5, Encoding.Default.GetBytes(str2), num4, out UIntPtr _);
                Win32.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, num5, 0U, IntPtr.Zero);
            }
        }   

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Form1._clientProcess == null || Form1._clientProcess.HasExited)
                return;
            int num = (int)MessageBox.Show("You can't close the Launcher while Fortnite is running", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            e.Cancel = true;
        }
    }
}
