using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ThreatParser.Model;

namespace ThreatParser.View
{
    public partial class ThreatsDifferenceListWindow : Window
    {
        public ObservableCollection<ThreatsDifference> DifferencesList { get; private set; } = new ObservableCollection<ThreatsDifference>();
        private static readonly string oldThreatTitle = "Старая запись";
        private static readonly string newThreatTitle = "Изменённая запись";
        private static readonly string addedThreatTitle = "Новая запись";
        private static readonly string removedThreatTitle = "Удалённая запись";
        public ThreatsDifferenceListWindow(Window owner, List<ThreatsDifference> diffs)
        {
            InitializeComponent();
            Owner = owner;
            DifferencesList.AddRange(diffs);
            Owner.Closed += (_, _) => Close();
            Loaded += ThreatsDifferenceListWindow_Loaded;
        }

        private void ThreatsDifferenceListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DifferencesListView.ItemsSource = DifferencesList;
        }

        private void DifferencesListView_MouseDoubleClick(object sender, MouseButtonEventArgs a)
        {
            var threadsDifference = (sender as ListView)?.SelectedItem as ThreatsDifference;
            if(threadsDifference != null)
            {
                if(threadsDifference.DifferenceType == DifferenceType.Add)
                {
                    ThreatDetailsWindow newThreatDetails = new ThreatDetailsWindow(this, threadsDifference.NewThreat, addedThreatTitle);
                    newThreatDetails.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    newThreatDetails.Show();
                } else if(threadsDifference.DifferenceType == DifferenceType.Remove)
                {
                    ThreatDetailsWindow oldThreatDetails = new ThreatDetailsWindow(this, threadsDifference.OldThreat, removedThreatTitle);
                    oldThreatDetails.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    oldThreatDetails.Show();
                } else
                {
                    ThreatDetailsWindow oldThreatDetails = new ThreatDetailsWindow(this, threadsDifference.OldThreat, oldThreatTitle);
                    oldThreatDetails.WindowStartupLocation = WindowStartupLocation.Manual;
                    oldThreatDetails.Left = (SystemParameters.PrimaryScreenWidth / 2) - (oldThreatDetails.Width + 50);
                    oldThreatDetails.Top = (SystemParameters.PrimaryScreenHeight / 2) - oldThreatDetails.Height;
                    oldThreatDetails.Show();

                    ThreatDetailsWindow newThreatDetails = new ThreatDetailsWindow(this, threadsDifference.NewThreat, newThreatTitle);
                    newThreatDetails.WindowStartupLocation = WindowStartupLocation.Manual;
                    newThreatDetails.Left = oldThreatDetails.Left + oldThreatDetails.Width + 50;
                    newThreatDetails.Top = oldThreatDetails.Top;
                    newThreatDetails.Show();
                }
            }
        }
    }

    public class EnumToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DifferenceType)) return value;
            switch((DifferenceType)value)
            {
                case DifferenceType.Add:
                    return "Добавление";
                case DifferenceType.Change:
                    return "Изменение";
                case DifferenceType.Remove:
                    return "Удаление";
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as string == "Добавление") return DifferenceType.Add;
            else if (value as string == "Удаление") return DifferenceType.Remove;
            return DifferenceType.Remove;
        }
    }

    public class EnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DifferenceType)) return new SolidColorBrush(Colors.Black);
            switch ((DifferenceType)value)
            {
                case DifferenceType.Add:
                    return new SolidColorBrush(Colors.Green);
                case DifferenceType.Change:
                    return new SolidColorBrush(Colors.Blue);
                case DifferenceType.Remove:
                    return new SolidColorBrush(Colors.DarkRed);
                default:
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as SolidColorBrush)?.Color == Colors.Green) return DifferenceType.Add;
            else if ((value as SolidColorBrush)?.Color == Colors.Red) return DifferenceType.Remove;
            return DifferenceType.Remove;
        }
    }

    public class ThreadsDifferenceToIdValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var diff = value as ThreatsDifference;
            if (diff != null)
            {
                if (diff.OldThreat != null) return diff.OldThreat.Id;
                else if (diff.NewThreat != null) return diff.NewThreat.Id;
                else return 0;
            }
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ThreatsDifference(DifferenceType.Add, null, null);
        }
    }
}
