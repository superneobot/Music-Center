using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MediaCenter.Model {
    public static partial class Loader {
        public static Task DownloadFile(string local, string ftp, string user, string password) {
            try {
                Task.Factory.StartNew(() => {
                    var folders = GetFilesInFtpDirectory(ftp, user, password);
                    var list = folders.ToList();
                    var local_folders = new List<string>();
                    var ftp_folders = new List<string>();
                    list.RemoveRange(0, 2);
                    foreach (var item in list) {
                        if (Directory.Exists(Path.Combine(local, item))) { } else {
                            createdir(local + @"\" + item);
                        }
                        local_folders.Add(local + @"\" + item);
                        ftp_folders.Add(ftp + item + "/");
                    }
                    var links = new List<string>();

                    for (int i = 0; i < ftp_folders.Count; i++) {
                        var folder = ftp_folders[i];
                        var list2 = GetFilesInFtpDirectory(folder, user, password).ToList();
                        for (int y = 0; y < list2.Count; y++) {
                            var item = list2[y];
                            links.Add(item);
                            //}

                            //for (int x = 0; x < links.Count; x++) {
                            var item2 = links[y];
                            if (item2 != "." & item2 != "..") {
                                item2 = folder + "/" + item2;
                                using (WebClient client = new WebClient()) {
                                    client.Credentials = new NetworkCredential("kalkyneogm_admin", "Sneo2352816botS");
                                    var file = Path.Combine(local_folders[i], Path.GetFileName(item2));
                                    client.DownloadFile(new Uri(item2), file);
                                }
                            }
                        }
                    }
                });
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return Task.CompletedTask;
        }

        // Here i get the list of Sub-directories and the files.   
        public static IEnumerable<string> GetFilesInFtpDirectory(string url, string username, string password) {
            // Get the object used to communicate with the server.
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(username, password);

            using (var response = (FtpWebResponse)request.GetResponse()) {
                using (var responseStream = response.GetResponseStream()) {
                    var reader = new StreamReader(responseStream);
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line) == false) {
                            yield return line.Split(new[] { ' ', '\t' }).Last();
                        }
                    }
                }
            }
        }
        // In this part i create the sub-directories.   
        public static void createdir(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}
