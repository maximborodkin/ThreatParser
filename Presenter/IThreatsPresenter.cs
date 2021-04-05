namespace ThreatParser.Presenter
{
    public interface IThreatsPresenter
    {
        public void RequestInitialPage();
        public void RequestPreviousPage();
        public void RequestNextPage();
        public void UpdateLocalCache(); 
    }
}
