using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatParser.Model.Entities;

namespace ThreatParser.Model
{
    public static class ThreatsRepository
    {
        public static List<Threat> Threats { get; private set; }
        private static string fileUrl = @"https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private static string localCachePath = @"/threats_list.csv";

        static ThreatsRepository()
        {
            Threats = new List<Threat>();
        }

        public static List<Threat> LoadFromDisk()
        {
            throw new NotImplementedException();
        }

        public static bool DownloadFile(out int progress)
        {
            throw new NotImplementedException();
        }

        private static List<Threat> ParseFile()
        {
            throw new NotImplementedException();
        }
    }
}
