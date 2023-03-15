using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MediaCenter.Model {
    public static partial class Loader {
        public static void DownloadFile(string local, string ftp, string user, string password) {

        }
        // Here i get the list of Sub-directories and the files.   
        public static string[] Return(string filepath, string username, string password) {
            List<string> directories = new List<string>();
            try {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(filepath);
                ftpRequest.Credentials = new NetworkCredential(username, password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line)) {
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }
            } catch (WebException e) {
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                System.Windows.Forms.MessageBox.Show(status.ToString());
            }
            return directories.ToArray();
        }
        // In this part i create the sub-directories.   
        public static void createdir(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}
