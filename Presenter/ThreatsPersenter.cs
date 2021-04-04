using System;
using System.Collections.Generic;
using ThreatParser.Model;
using ThreatParser.View;

namespace ThreatParser.Presenter
{
    public class ThreatsPersenter : IThreatsPresenter
    {
        private IThreatsView view;
        private IThreatsRepository repository;

        public List<Threat> Threats { get; private set; }

        public ThreatsPersenter(IThreatsView view) 
        {
            this.view = view;
            repository = new ThreatsRepository();
            Threats = new List<Threat>();
            InitializePresenter();
        }

        private void InitializePresenter()
        {
            if (!repository.IsCacheExists())
            {
                if (view.ShowDownloadOffer())
                {
                    UpdateLocalCache();
                } else
                {
                    view.ShowNoCache();
                    return;
                }
            }
            else
            {
                List<Threat> threats;
                try
                {
                    threats = repository.LoadFromCache();
                    Threats.Clear();
                    Threats.AddRange(threats);
                } catch(Exception e)
                {
                    view.ShowCacheError();
                }
                Console.WriteLine();
            }
        }

        public void UpdateLocalCache()
        {
            try
            {
                List<Threat> threats = repository.UpdateLocalCache(out List<(DifferenceType, string, string)> diffs);
                Threats.Clear();
                Threats.AddRange(threats);
                view.ShowDifferences(diffs);
            } catch (Exception e)
            {
                view.ShowDownloadError();
            }
        }

    }
}