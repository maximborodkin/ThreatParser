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
        private List<Threat> threats;

        public bool IsCacheExists() => File.Exists(localCacheUri);

        // Exceptions must be catched in presenter
        public List<Threat> GetRecordsRange(int startIndex, int count)
        {
            if (threats == null)
            {
                threats = ReadFromJSON(localCacheUri);
            }
            if (startIndex < 0 || startIndex >= threats.Count) return null;
            return (from t in threats select t).Skip(startIndex).Take(count).ToList();
        }

        // Exceptions must be catched in presenter
        public void UpdateLocalCache(out List<ThreatsDifference> diffs)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFileUri, remoteFileName);

            List<Threat> oldThreats = ReadFromJSON(localCacheUri);
            List<Threat> newThreats = ReadFromXLSX(remoteFileName);
            diffs = CompareLists(oldThreats, newThreats);

            WriteToJSON(newThreats);

            if (threats == null) threats = new List<Threat>();
            threats.Clear();
            threats.AddRange(newThreats);
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
            string json = JsonConvert.SerializeObject(threats);
            File.WriteAllText(localCacheUri, json);
        }

        private List<ThreatsDifference> CompareLists(List<Threat> oldList, List<Threat> newList)
        {
            List<ThreatsDifference> threatsDifferences = new List<ThreatsDifference>();
            newList.ForEach(newThreat =>
            {
                var oldThreat = oldList.Find(o => o.Id == newThreat.Id);
                if(oldThreat == null)
                {
                    threatsDifferences.Add(new ThreatsDifference(DifferenceType.Add, null, newThreat));
                } 
                else if (!oldThreat.Equals(newThreat))
                {
                    threatsDifferences.Add(new ThreatsDifference(DifferenceType.Change, oldThreat, newThreat));
                }
            });
            List<Threat> deletedThreats = oldList.FindAll(o => newList.All(n => n.Id != o.Id)).ToList();
            deletedThreats.ForEach(d => threatsDifferences.Add(new ThreatsDifference(DifferenceType.Remove, d, null)));

            return threatsDifferences;
        }
    }
}
