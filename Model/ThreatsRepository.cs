using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ThreatParser.Model
{
    public partial class ThreatsRepository : IThreatsRepository
    {
        public static List<Threat> Threats { get; private set; }

        private static readonly string remoteFileUri = @"https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private static readonly string remoteFileName = "thrlist.xlsx";
        private static readonly string localCacheUri = @"thrlist.csv";

        public bool IsCacheExists()
        {
            return File.Exists(localCacheUri);
        }

        // Exceptions must be catched in presenter
        public void LoadFromCache()
        {
            List<Threat> threats = ReadFromCSV(localCacheUri);
            Threats.Clear();
            Threats.AddRange(threats);
        }

        // Exceptions must be catched in presenter
        public void UpdateLocalCache(out List<(DifferenceType, string, string)> diffs)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFileUri, remoteFileName);

            List<Threat> oldThreats = ReadFromCSV(localCacheUri);
            List<Threat> newThreats = ReadFromXLSX(remoteFileName);
            diffs = CompareLists(oldThreats, newThreats);

            Threats.Clear();
            Threats.AddRange(newThreats);

            WriteToCSV(newThreats);
        }

        private List<Threat> ReadFromXLSX(string fileName)
        {
            var query = new ExcelQueryFactory(fileName);

            query.AddTransformation<Threat>(f => f.Id, v => int.Parse(v));
            query.AddTransformation<Threat>(f => f.ConfidentialityBreach, v => v == "1");
            query.AddTransformation<Threat>(f => f.IntegrityBreach, v => v == "1");
            query.AddTransformation<Threat>(f => f.AvailabilityBreach, v => v == "1");

            string sheetName = "Sheet";
            int rowsCount = query.Worksheet(sheetName).Count() + 3; // + 3 -- twho headers and zero-start index
            List<Threat> threats = (from t in query.WorksheetRange<Threat>("A2", $"H{rowsCount}", sheetName) select t).ToList();
            return threats;
        }

        private List<Threat> ReadFromCSV(string fileName)
        {
            if (!File.Exists(localCacheUri)) throw new FileNotFoundException("Local chache file not found");

            var query = new ExcelQueryFactory(localCacheUri);
            var threats = from t in query.Worksheet<Threat>() select t;
            return new List<Threat>();
        }

        private void WriteToCSV(List<Threat> threats)
        {
            using(var fileStream = new FileStream(localCacheUri, FileMode.Truncate))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(Threat.GetCSVHeader());
                    threats.ForEach(t => streamWriter.WriteLine(t.ToCSVString()));
                }
            }
        }

        private List<(DifferenceType, string, string)> CompareLists(List<Threat> oldList, List<Threat> newList)
        {
            return new List<(DifferenceType, string, string)>(3);
        }
    }
}
