using System.Collections.Generic;
using ThreatParser.Model;

namespace ThreatParser.View
{
    public interface IThreatsView
    {
        public bool ShowDownloadOffer();
        public void ShowNoCache();
        public void ShowDownloadError();
        public void ShowCacheError();
        public void ShowDifferences(List<ThreatsDifference> diffs);
        public void ShowThreats(List<Threat> threats);
    }
}
