using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace unittest.runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var tests = new List<string>(
                Directory.EnumerateFiles("..\\..\\..\\testcase", "*.txt", SearchOption.AllDirectories));

            foreach (var path in tests)
            {
                var answer = File.ReadAllText(path);

                var p = Process.Start(new ProcessStartInfo()
                {
                    FileName = "turtlenet.exe",
                    Arguments = "unittest.exe " + Path.GetFileName(path),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });
                p.WaitForExit();

                var answerFromTurtle = p.StandardOutput.ReadLine();

                if (answer == answerFromTurtle)
                    Console.WriteLine($"  * {path} => PASS");
                else
                    Console.WriteLine($"  * {path} => FAIL");
            }
        }
    }
}
