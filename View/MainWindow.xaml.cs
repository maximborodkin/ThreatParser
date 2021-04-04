using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThreatParser.Model;
using ThreatParser.Presenter;
using ThreatParser.View;

namespace ThreatParser
{
    public partial class MainWindow : Window, IThreatsView
    {
        private IThreatsPresenter presenter;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            presenter = new ThreatsPersenter(this);

        }

        public bool ShowDownloadOffer()
        {
            string question = "There is no downloaded local cache file. Would you like to download it now?";
            return MessageBox.Show(question, "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowNoCache()
        {
            throw new NotImplementedException();
        }

        public void ShowThreats(List<Threat> threats)
        {
            throw new NotImplementedException();
        }

        public void ShowDownloadError()
        {
            throw new NotImplementedException();
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            presenter.UpdateLocalCache();
        }

        public void ShowCacheError()
        {
            throw new NotImplementedException();
        }

        public void ShowDifferences(List<(DifferenceType, string, string)> diffs)
        {
            throw new NotImplementedException();
        }
    }
}
