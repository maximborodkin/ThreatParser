using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ThreatParser.Model;
using ThreatParser.Presenter;
using ThreatParser.View;

namespace ThreatParser
{
    public partial class MainWindow : Window, IThreatsView
    {
        private IThreatsPresenter presenter;
        public ObservableCollection<Threat> CurrentPage { get; set; } = new ObservableCollection<Threat>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            presenter = new ThreatsPresenter(this);
            presenter.RequestinitialPage();
            ThreadsList.ItemsSource = CurrentPage;
        }

        public bool ShowDownloadOffer()
        {
            string question = "There is no downloaded local cache file. Would you like to download it now?";
            return MessageBox.Show(question, "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowNoCache()
        {
            MessageBox.Show("Ну и зря. Сиди теперь без файла");
            //throw new NotImplementedException();
        }

        public void ShowThreats(List<Threat> threats)
        {
            CurrentPage.Clear();
            threats.ForEach(t => CurrentPage.Add(t));
        }

        public void ShowDownloadError()
        {
            MessageBox.Show("Ошибка при загрузке файла");
            //throw new NotImplementedException();
        }

        public void ShowCacheError()
        {
            MessageBox.Show("Ошибка при загрузке кэша");
            //throw new NotImplementedException();
        }

        public void ShowDifferences(List<(DifferenceType, string, string)> diffs)
        {
            MessageBox.Show("Сейчас должны показаться различия");
            //throw new NotImplementedException();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.RequestPreviousPage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.RequestNextPage();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.UpdateLocalCache();
        }

        private void ThreadsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Threat threat = (sender as ListView)?.SelectedItem as Threat;
            if(threat != null)
            {
                ThreatDetailsWindow detailsWindow = new ThreatDetailsWindow(this, threat);
                detailsWindow.Show();
            }
        }
    }
}
