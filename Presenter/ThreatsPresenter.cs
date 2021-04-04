using System;
using System.Collections.Generic;
using ThreatParser.Model;
using ThreatParser.View;

namespace ThreatParser.Presenter
{
    public class ThreatsPresenter : IThreatsPresenter
    {
        private IThreatsView view;
        private IThreatsRepository repository;
        public int currentPageStartIndex = 0;
        private static readonly int pageRecordsCount = 15;

        public ThreatsPresenter(IThreatsView view) 
        {
            this.view = view;
            repository = new ThreatsRepository();
        }

        private bool CheckCache()
        {
            if (!repository.IsCacheExists())
            {
                if (!view.ShowDownloadOffer())
                {
                    view.ShowNoCache();
                    return false;
                }
                else
                {
                    UpdateLocalCache();
                }
            }
            return true;
        }

        public void UpdateLocalCache()
        {
            try
            {
                repository.UpdateLocalCache(out List<(DifferenceType, string, string)> diffs);
                view.ShowDifferences(diffs);
            }
            catch (Exception)
            {
                view.ShowDownloadError();
            }
        }

        public void RequestinitialPage()
        {
            GetRecordsRange(0);
        }

        public void RequestPreviousPage()
        {
            int startRangeIndex = currentPageStartIndex - pageRecordsCount;
            GetRecordsRange(startRangeIndex >= 0 ? startRangeIndex : 0);
        }

        public void RequestNextPage()
        {
            GetRecordsRange(currentPageStartIndex + pageRecordsCount);
        }

        private void GetRecordsRange(int startIndex)
        {
            if (CheckCache())
            {
                try
                {
                    List<Threat> threats = repository.GetRecordsRange(startIndex, pageRecordsCount);
                    if(threats != null)
                    {
                        currentPageStartIndex = startIndex;
                        view.ShowThreats(threats);
                    }
                }
                catch (IndexOutOfRangeException) { }
                catch (Exception)
                {
                    view.ShowCacheError();
                }
            }
        }
    }
}