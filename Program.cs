using System;
using System.IO;
using System.Diagnostics;

namespace Download_Applications
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No file provided.");
                return;
            }

            String csvDirectory = args[0];

            if (File.Exists(csvDirectory))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine($"{DateTime.Now} - Download applications started");
                
                Console.WriteLine(Environment.OSVersion);

                Console.WriteLine($"Number of applicants: {ApplicationDownloader.DownloadApplications(csvDirectory)}");

                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine($"{DateTime.Now} - Download applications ended");
            } else {
                Console.WriteLine($"File does not exist: ${csvDirectory}");
            }

        }
    }
}
