using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Jupiter;


namespace Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "GDMO.exe",
                    Arguments = " DiMaOAuthKey.value",
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true
                }

            };

            process.Start();

            
        }
    }
}
