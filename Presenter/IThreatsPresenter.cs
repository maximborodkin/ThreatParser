using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatParser.Model;

namespace ThreatParser.Presenter
{
    public interface IThreatsPresenter
    {
        public void RequestinitialPage();
        public void RequestPreviousPage();
        public void RequestNextPage();
        public void UpdateLocalCache(); 
    }
}
