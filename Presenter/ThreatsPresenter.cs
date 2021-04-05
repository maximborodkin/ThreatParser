using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
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
            var mainDispatcher = Dispatcher.CurrentDispatcher;
            new Thread(() =>
            {
                try
                {
                    repository.UpdateLocalCache(out List<ThreatsDifference> diffs);
                    mainDispatcher?.Invoke(() => view.ShowDifferences(diffs));
                }
                catch (Exception) 
                {
                    mainDispatcher?.Invoke(() => view.ShowDownloadError());
                }
            }).Start();
        }

        public void RequestInitialPage()
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
                //Get dispatcher of MainThread
                var mainDispatcher = Dispatcher.CurrentDispatcher;
                try
                {
                    new Thread(() =>
                    {
                        List<Threat> threats = repository.GetRecordsRange(startIndex, pageRecordsCount);
                        if (threats != null)
                        {
                            currentPageStartIndex = startIndex;
                            //Run ui operation in MainThread
                            mainDispatcher?.Invoke(() => view.ShowThreats(threats));
                        }
                    }).Start();
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