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

        public ThreatsPersenter(IThreatsView view) 
        {
            this.view = view;
            repository = new ThreatsRepository();
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
                }
            }
            else
            {
                
            }
        }

        public void UpdateLocalCache()
        {
            List<(DifferenceType, string, string)> diffs;
            try
            {
                repository.UpdateLocalCache(out diffs);
            } catch (Exception e)
            {
                view.ShowDownloadError();
            }
        }
    }
}