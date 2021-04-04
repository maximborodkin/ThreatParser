using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatParser.Model;

namespace ThreatParser.View
{
    public interface IThreatsView
    {
        public bool ShowDownloadOffer();
        public void ShowNoCache();

        public void ShowDownloadError();
    }
}
