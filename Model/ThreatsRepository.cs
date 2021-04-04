using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace ThreatParser.Model
{
    public partial class ThreatsRepository : IThreatsRepository
    {
        private static readonly string remoteFileUri = @"https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private static readonly string remoteFileName = "thrlist.xlsx";
        private static readonly string localCacheUri = @"thrlist.json";

        public bool IsCacheExists()
        {
            return File.Exists(localCacheUri);
        }

        // Exceptions must be catched in presenter
        public List<Threat> LoadFromCache()
        {
            if (!File.Exists(localCacheUri)) throw new FileNotFoundException("Local chache file not found");

            try
            {
                return ReadFromJSON(localCacheUri);
            } catch (Exception)
            {
                throw new IOException();
            }
        }

        // Exceptions must be catched in presenter
        public List<Threat> UpdateLocalCache(out List<(DifferenceType, string, string)> diffs)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFileUri, remoteFileName);

            List<Threat> oldThreats = ReadFromJSON(localCacheUri);
            List<Threat> newThreats = ReadFromXLSX(remoteFileName);
            diffs = CompareLists(oldThreats, newThreats);

            WriteToJSON(newThreats);
            return newThreats;
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

        private List<Threat> ReadFromJSON(string fileName)
        {
            if (!File.Exists(fileName)) return new List<Threat>();

            string json = File.ReadAllText(fileName);
            List<Threat> threats = JsonConvert.DeserializeObject<List<Threat>>(json);

            return threats;
        }

        private void WriteToJSON(List<Threat> threats)
        {
            //using(var fileStream = new FileStream(localCacheUri, FileMode.Create))
            //{
            //    using (var streamWriter = new StreamWriter(fileStream))
            //    {
            //        streamWriter.WriteLine(Threat.GetCSVHeader());
            //        threats.ForEach(t => streamWriter.WriteLine(t.ToCSVString()));
            //    }
            //}
            string json = JsonConvert.SerializeObject(threats);
            File.WriteAllText(localCacheUri, json);
        }

        private List<(DifferenceType, string, string)> CompareLists(List<Threat> oldList, List<Threat> newList)
        {
            return new List<(DifferenceType, string, string)>(3);
        }
    }
}
