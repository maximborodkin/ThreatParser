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
        public ObservableCollection<Threat> CurrentPage { get; private set; } = new ObservableCollection<Threat>();
        private IThreatsPresenter presenter;

        private const string downloadOfferMessage = "Локальный кэш не найден. Создать его сейчас?";
        private const string noCacheMessage = "Ну и зря. Сиди теперь без файла. Можешь скачать его по кнопке \"Обновить\"";
        private const string downloadErrorMessage = "Ошибка при загрузке файла. Попробуйте ещё раз.";
        private const string cacheErrorMessage = "Ошибка при загрузке кэша. Попробуйте заново скачать файл.";
        private const string noDifferencesMessage = "Файл успешно загружен. Изменений не найдено.";

        public MainWindow()
        {
            InitializeComponent();
            presenter = new ThreatsPresenter(this);
            Loaded += (_,_) => presenter.RequestInitialPage();
        }

        public bool ShowDownloadOffer()
        {
            return MessageBox.Show(downloadOfferMessage, null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowNoCache()
        {
            MessageBox.Show(noCacheMessage, null, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ShowThreats(List<Threat> threats)
        {
            RefreshButton.IsEnabled = true;
            CurrentPage.Clear();
            CurrentPage.AddRange(threats);
        }

        public void ShowDownloadError()
        {
            RefreshButton.IsEnabled = true;
            MessageBox.Show(downloadErrorMessage, null, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowCacheError()
        {
            RefreshButton.IsEnabled = true;
            MessageBox.Show(cacheErrorMessage, null, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowDifferences(List<ThreatsDifference> diffs)
        {
            RefreshButton.IsEnabled = true;
            presenter.RequestInitialPage();
            if(diffs.Count == 0)
            {
                MessageBox.Show(noDifferencesMessage, null, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            } else
            {
                new ThreatsDifferenceListWindow(this, diffs).Show();
            }
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
            RefreshButton.IsEnabled = false;
        }

        private void ThreadsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Threat threat = (sender as ListView)?.SelectedItem as Threat;
            if(threat != null)
            {
                ThreatDetailsWindow detailsWindow = new ThreatDetailsWindow(this, threat);
                detailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                detailsWindow.Show();
            }
        }
    }
}
