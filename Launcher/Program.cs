using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Launcher
{
    public class Program
    {
        private static Process _clientProcess;
        private static byte _clientAnticheat;
        private static void Main(string[] args)
        {
            string path = "";
            Win32.AllocConsole();

            Console.WriteLine("Do you want to use predefined path?");
            Console.WriteLine("y/n?");
            string answer = Console.ReadLine();
            if (answer == "n")
            {
                Console.WriteLine("Enter Path!");
                path = Console.ReadLine();
            } else if (answer == "y")
            {
                path = "G:\\Fortnite\\workaround";
            }

            Console.WriteLine("Anticheat Bypass");
            Console.WriteLine("0, 1, 2, or main?");
            string anticheatpath = Console.ReadLine();
            switch (anticheatpath)
            {
                case "0":
                    _clientAnticheat = 0;
                    break;
                case "1":
                    _clientAnticheat = 1;
                    break;
                case "2":
                    _clientAnticheat = 2;
                    break;
                case "main":
                    _clientAnticheat = 2;
                    break;
                   
            }
            if (string.IsNullOrEmpty(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Path!");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            string str1 = Path.Combine(path, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");

            if (!File.Exists(str1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fortnite was not found!");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                string str2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Platanium.dll");

                if (!File.Exists(str2))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Platanium.dll was not found!");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    string arguments = "-AUTH_LOGIN=\"" + "imqpixel@modnite.net\" -AUTH_PASSWORD=unused -AUTH_TYPE=epic";
                    switch (_clientAnticheat)
                    {
                        case 0:
                            arguments += " -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -nobe -fltoken=none";
                            break;
                        case 1:
                            arguments += " -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=f7b9gah4h5380d10f721dd6a";
                            break;
                        case 2:
                            arguments += " -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=10ga222d803bh65851660E3d";
                            break;
                    }
                    Program._clientProcess = new Process()
                    {
                        StartInfo = new ProcessStartInfo(str1, arguments)
                        {
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = false
                        }
                    };
                    Program._clientProcess.Start();
                    IntPtr hProcess = Win32.OpenProcess(1082, false, Program._clientProcess.Id);
                    IntPtr procAddress = Win32.GetProcAddress(Win32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    uint num4 = (uint)((str2.Length + 1) * Marshal.SizeOf(typeof(char)));
                    IntPtr num5 = Win32.VirtualAllocEx(hProcess, IntPtr.Zero, num4, 12288U, 4U);
                    Win32.WriteProcessMemory(hProcess, num5, Encoding.Default.GetBytes(str2), num4, out UIntPtr _);
                    Win32.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, num5, 0U, IntPtr.Zero);
                    Program._clientProcess.WaitForExit();
                }
            }
        }
        private static void SwapLauncher()
        {
            if (File.Exists("FortniteLauncher.exe.original"))
            {
                File.Move("FortniteLauncher.exe", "FortniteLauncher.exe.custom");
                File.Move("FortniteLauncher.exe.original", "FortniteLauncher.exe");
            }
            if (File.Exists("FortniteLauncher.exe.custom"))
                return;
            File.Move("FortniteLauncher.exe", "FortniteLauncher.exe.original");
            File.Move("FortniteLauncher.exe.custom", "FortniteLauncher.exe");
        }
    }
}
