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
        public List<Threat> LoadFromCache();
        public List<Threat> UpdateLocalCache(out List<(DifferenceType, string, string)> diffs);
    }
}
