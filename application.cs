namespace Download_Applications
{
    using System;
    using System.Diagnostics;
    using System.IO;

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

        public Application()
        {

        }

        public bool DownloadCV(String downloadFolderPath)
        {
            if (this.CvLink.Equals(String.Empty))
            {
                Console.WriteLine($"No CV attached for applicant {this.FirstName} {this.LastName}");
                return false;
            }

            Console.WriteLine($"Downloading {this.CvName}");
            // navigates to url (downloads the application)
            String uri = this.CvLink;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);

            // Check if file is downloaded
            DateTime timeout = DateTime.Now.AddSeconds(20);

            while (DateTime.Now <= timeout)
            {
                if (File.Exists($"{downloadFolderPath}/{this.CvName}"))
                {
                    Console.WriteLine($"File downloaded : {downloadFolderPath}/{this.CvName}");
                    return true;
                }

                System.Threading.Thread.Sleep(200);
            }

            Console.WriteLine($"File failed to download : {this.CvName}");
            return false;
        }
    }
}