using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreatParser.Model
{
    public class ThreatsRepository : IThreatsRepository
    {
        public delegate void OnDownloadSuccess(List<Threat> threats);
        public delegate void OnDownloadError(string message);
        public static List<Threat> Threats { get; private set; }

        private static string remoteFileUri = @"https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private static string remoteFileName = "thrlist.xlsx";
        private static string localCacheUri = @"threats_list.csv";

        public bool IsCacheExists()
        {
            return File.Exists(localCacheUri);
        }

        public List<Threat> LoadFromCache()
        {
            throw new NotImplementedException();
        }

        public void DownloadFile()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFileUri, remoteFileName);
            ParseToCSV();
        }

        private void ParseToCSV()
        {
            //This method should contains parse from xlsx to csv.
            //But now it is saving same file with csv extension
            File.Copy(remoteFileName, localCacheUri, true);
        }
    }
}
