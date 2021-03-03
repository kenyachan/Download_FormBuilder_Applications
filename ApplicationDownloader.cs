namespace Download_Applications
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Diagnostics;

    public class ApplicationDownloader 
    {
        private static readonly String DOWNLOADSFOLDERPATH = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";
        public static int DownloadApplications(String csvPath)
        {
            CreateApplicationsFolder();

            List<Application> applications = ReadCSV(csvPath);

            foreach (Application application in applications)
            {
                if (DownloadCV(application))
                    MoveCV(application);
                
                if (DownloadSC(application))
                    MoveSC(application);
            }

            return applications.Count();
        }

        private static String MoveSC(Application application)
        {
            String extension = Path.GetExtension($"{DOWNLOADSFOLDERPATH}/{application.ScName}");
            String fullName;
            String newFileName;

            if (!application.OtherName.Equals(String.Empty))
                fullName = $"{application.FirstName} {application.OtherName} {application.LastName}";
            else
                fullName = $"{application.FirstName} {application.LastName}";
            
            newFileName = $"{fullName} SC{extension}";

            // increment for duplicate files
            int i = 1;

            while (File.Exists($"{DOWNLOADSFOLDERPATH}/Applications/{newFileName}"))
            {
                newFileName = $"{fullName} SC ({i}){extension}";
                i++;
            }

            File.Move($"{DOWNLOADSFOLDERPATH}/{application.ScName}", $"{DOWNLOADSFOLDERPATH}/Applications/{newFileName}");
            
            return $"{DOWNLOADSFOLDERPATH}/Applications/{newFileName}";
        }

        private static String MoveCV(Application application)
        {
            String extension = Path.GetExtension($"{DOWNLOADSFOLDERPATH}/{application.CvName}");
            String fullName;
            String newFileName;

            if (!application.OtherName.Equals(String.Empty))
                fullName = $"{application.FirstName} {application.OtherName} {application.LastName}";
            else
                fullName = $"{application.FirstName} {application.LastName}";
            
            newFileName = $"{fullName} CV{extension}";

            // increment for duplicate files
            int i = 1;

            while (File.Exists($"{DOWNLOADSFOLDERPATH}/Applications/{newFileName}"))
            {
                newFileName = $"{fullName} CV ({i}){extension}";
                i++;
            }

            File.Move($"{DOWNLOADSFOLDERPATH}/{application.CvName}", $"{DOWNLOADSFOLDERPATH}/Applications/{newFileName}");
            
            return $"{DOWNLOADSFOLDERPATH}/Applications/{newFileName}";
        }

        private static bool CreateApplicationsFolder()
        {
            // check if "'UserProfile'/Downloads/Applications" folder exists or create it
            if (!Directory.Exists($"{DOWNLOADSFOLDERPATH}/Applications")) 
            {
                Console.WriteLine($"\"{DOWNLOADSFOLDERPATH}/Applications\" does not exist. Creating folder.");
                Directory.CreateDirectory($"{DOWNLOADSFOLDERPATH}/Applications");
                return true;
            }

            return false;
        }

    private static bool DownloadSC(Application application)
        {
            if (application.ScLink.Equals(String.Empty))
            {
                Console.WriteLine($"No Selection Criteria attached for applicant {application.FirstName} {application.LastName}");
                return false;
            }

            Console.WriteLine($"Downloading {application.ScName}");
            // navigates to url (downloads the application)
            String uri = application.ScLink;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);

            // Check if file is downloaded
            DateTime timeout = DateTime.Now.AddSeconds(20);

            while (DateTime.Now <= timeout)
            {
                if (File.Exists($"{DOWNLOADSFOLDERPATH}/{application.ScName}"))
                {
                    Console.WriteLine($"File downloaded : {DOWNLOADSFOLDERPATH}/{application.ScName}");
                    return true;
                }

                System.Threading.Thread.Sleep(200);
            }

            Console.WriteLine($"File failed to download : {application.ScName}");
            return false;
        }

        private static bool DownloadCV(Application application)
        {
            if (application.CvLink.Equals(String.Empty))
            {
                Console.WriteLine($"No CV attached for applicant {application.FirstName} {application.LastName}");
                return false;
            }

            Console.WriteLine($"Downloading {application.CvName}");
            // navigates to url (downloads the application)
            String uri = application.CvLink;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);

            // Check if file is downloaded
            DateTime timeout = DateTime.Now.AddSeconds(20);

            while (DateTime.Now <= timeout)
            {
                if (File.Exists($"{DOWNLOADSFOLDERPATH}/{application.CvName}"))
                {
                    Console.WriteLine($"File downloaded : {DOWNLOADSFOLDERPATH}/{application.CvName}");
                    return true;
                }

                System.Threading.Thread.Sleep(200);
            }

            Console.WriteLine($"File failed to download : {application.CvName}");
            return false;
        }

        /**
        Reads the CSV into a List<Application>
        */
        private static List<Application> ReadCSV(string path) 
        {
            List<Application> applications = new List<Application>();
            IEnumerable<String> csvFile = File.ReadLines(path);

            foreach (String line in csvFile) 
            {
                String[] strArr = line.Split(',');

                // Removes first and last " characters from strings
                strArr = strArr.Select(t => t.Trim('\"')).ToArray();    

                Application application = new Application();

                application.SubmittedDate = strArr[0];
                application.FirstName = strArr[1];
                application.LastName = strArr[2];
                application.OtherName = strArr[3];
                application.EmailAddress = strArr[4];
                application.PhoneNumber = strArr[5];
                application.CvLink = strArr[6];
                application.CvName = strArr[7];
                application.ScLink = strArr[8];
                application.ScName = strArr[9];

                applications.Add(application);
            }

            // Remove first 2 list items (header rows in CSV)
            applications.RemoveRange(0, 2);

            return applications;
        }  
    }
}