using System;
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
                    UpdateFile();
                } else
                {
                    view.ShowNoCache();
                }
            }
            else
            {
                view.ShowThreats(repository.LoadFromCache());
            }
        }

        public void UpdateFile()
        {
            repository.DownloadFile();
            view.ShowThreats(repository.LoadFromCache());
        }
    }
}