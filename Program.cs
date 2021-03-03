using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;

namespace Download_Applications
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - Download applications started");
            
            Console.WriteLine(Environment.OSVersion);

            Console.WriteLine(ApplicationDownloader.DownloadApplications("results-form940.csv"));

            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"{DateTime.Now} - Download applications ended");
        }
    }
}
