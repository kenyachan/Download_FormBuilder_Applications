namespace Download_Applications
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;

    public class Application
    {
        private String _submittedDate;
        private String _firstName;
        private String _lastName;
        private String _otherName;
        private String _emailAddress;
        private String _phoneNumber;
        private String _cvLink;
        private String _cvName;
        private String _scLink;
        private String _scName;

        // consider removing
        private int _downloadTimeOut = 20;
        private String _downloadFolderPath;

        public string SubmittedDate { get => _submittedDate; set => _submittedDate = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string OtherName { get => _otherName; set => _otherName = value; }
        public string EmailAddress { get => _emailAddress; set => _emailAddress = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public string CvLink { get => _cvLink; set => _cvLink = value; }
        public string CvName { get => _cvName; set => _cvName = value; }
        public string ScLink { get => _scLink; set => _scLink = value; }
        public string ScName { get => _scName; set => _scName = value; }

        //consider removing
        public int DownloadTimeOut { get => _downloadTimeOut; set => _downloadTimeOut = value; }
        public string DownloadFolderPath { get => _downloadFolderPath; }

        public const int CV = 1;
        public const int SC = 2;

        public Application()
        {
            this._downloadFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";
        }

        // file is either 1 or 2, CV or SC as defined by the const above
        public bool Download(int file)
        {   
            if (file != CV && file != SC) 
                throw new ArgumentException("Uknown appFile provided. Please use Application.CV or Application.SC");
            
            String uri = file == CV ? this.CvLink : this.ScLink;
            String fileName = file == CV ? this.CvName : this.ScName;

            if (fileName.Equals(String.Empty))
            {
                Console.WriteLine($"No {(file == CV ? "CV" : "SC")} attached for applicant " +
                $"{this.FirstName}{(this.OtherName.Equals(String.Empty) ? String.Empty : $" {this.OtherName}")} {this.LastName}");
                return false;
            }

            Console.WriteLine($"Downloading {(file == CV ? "CV" : "SC")} file: {fileName}");

            Process.Start("Open", uri);

            return true;
            /* 
            DateTime timeout = DateTime.Now.AddSeconds(this.DownloadTimeOut);

            do
            {
                if (DateTime.Now >= timeout)
                    throw new System.TimeoutException($"Timed out waiting to download file: {fileName}");

                System.Threading.Thread.Sleep(200);
            } while (!File.Exists($"{this.DownloadFolderPath}/{fileName}"));

            return $"{this.DownloadFolderPath}/{fileName}"; */
        }
    }
}