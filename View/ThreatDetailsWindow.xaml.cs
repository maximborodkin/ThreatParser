using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ThreatParser.Model;

namespace ThreatParser.View
{
    public partial class ThreatDetailsWindow : Window, INotifyPropertyChanged
    {
        private string title;
        public string WindowTitle 
        {
            get => title;
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTitle)));
            }
        }

        private Threat threat;
        public Threat Threat 
        { 
            get => threat; 
            private set
            {
                threat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Threat)));
            }
        }     

        public event PropertyChangedEventHandler PropertyChanged;

        public ThreatDetailsWindow(Window owner, Threat threat, string title) : this(owner, threat)
        {
            WindowTitle = title;
        }

        public ThreatDetailsWindow(Window owner, Threat threat)
        {
            InitializeComponent();
            Owner = owner;
            Threat = threat;
            Owner.Closed += (_, _) => Close();
        }
    }

    internal class BooleanToStringValueConverter : IValueConverter
    {
        private const string yes = "да";
        private const string no = "нет";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is bool && (bool)value) ? yes : no;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is string && (string)value == yes;
    }
}
