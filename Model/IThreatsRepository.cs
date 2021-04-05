using System.Collections.Generic;

namespace ThreatParser.Model
{
    public interface IThreatsRepository
    {
        public bool IsCacheExists();
        public void UpdateLocalCache(out List<ThreatsDifference> diffs);
        public List<Threat> GetRecordsRange(int startIndex, int count);
    }
}
