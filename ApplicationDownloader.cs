namespace Download_Applications
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class ApplicationDownloader 
    {
        private static readonly String DOWNLOADSFOLDERPATH = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";
        public static int DownloadApplications(String csvPath)
        {
            CreateApplicationsFolder();

            List<Application> applications = ReadCSV(csvPath);

            foreach (Application application in applications)
            {
                if (application.Download(Application.CV))
                {
                    DateTime timeout = DateTime.Now.AddSeconds(10);

                    do
                    {
                        if (DateTime.Now > timeout)
                            throw new System.TimeoutException($"Timed out waiting for file to download: {application.CvName}");
                        
                        System.Threading.Thread.Sleep(200);

                    } while (!File.Exists($"{DOWNLOADSFOLDERPATH}/{application.CvName}"));

                    MoveCV(application);
                }

                if (application.Download(Application.SC))
                {
                    DateTime timeout = DateTime.Now.AddSeconds(10);

                    do
                    {
                        if (DateTime.Now > timeout)
                        {
                            TimeoutException te = new TimeoutException($"Timed out waiting for file to download: {application.ScName}");
                            te.Data.Add("expected file", application.CvName);
                            throw te;
                        }
                            
                        System.Threading.Thread.Sleep(200);

                    } while (!File.Exists($"{DOWNLOADSFOLDERPATH}/{application.ScName}"));

                    MoveSC(application);
                }
            }
            return applications.Count();
        }

        private static String MoveSC(Application application)
        {
            String extension = Path.GetExtension($"{DOWNLOADSFOLDERPATH}/{application.ScName}");
            String fullName, newFileName;

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (!application.OtherName.Equals(String.Empty))
                fullName = $"{textInfo.ToTitleCase(application.FirstName)} {textInfo.ToTitleCase(application.OtherName)} {textInfo.ToTitleCase(application.LastName)}";
            else
                fullName = $"{textInfo.ToTitleCase(application.FirstName)} {textInfo.ToTitleCase(application.LastName)}";
            
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
            String fullName, newFileName;

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (!application.OtherName.Equals(String.Empty))
                fullName = $"{textInfo.ToTitleCase(application.FirstName)} {textInfo.ToTitleCase(application.OtherName)} {textInfo.ToTitleCase(application.LastName)}";
            else
                fullName = $"{textInfo.ToTitleCase(application.FirstName)} {textInfo.ToTitleCase(application.LastName)}";
            
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

        /**
        Reads the CSV into a List<Application>
        */
        private static List<Application> ReadCSV(string path) 
        {
            List<Application> applications = new List<Application>();
            IEnumerable<String> csvFile = File.ReadLines(path);

            foreach (String line in csvFile) 
            {
                // Delimiter pattern in case of comma's in file names.
                String pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
                String[] strArr = Regex.Split(line,pattern);

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