using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatParser.Model
{
    public interface IThreatsRepository
    {
        public bool IsCacheExists();
        public void UpdateLocalCache(out List<(DifferenceType, string, string)> diffs);
        public List<Threat> GetRecordsRange(int startIndex, int count);
    }
}
