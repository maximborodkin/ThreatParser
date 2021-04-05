using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Shapes;
using ThreatParser.Model;

namespace ThreatParser.View
{
    /// <summary>
    /// Interaction logic for ThreatDetailsWindow.xaml
    /// </summary>
    public partial class ThreatDetailsWindow : Window, INotifyPropertyChanged
    {
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

        public ThreatDetailsWindow(Window owner, Threat threat)
        {
            InitializeComponent();
            Owner = owner;
            Threat = threat;
        }
    }

    public class BooleanToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is bool && (bool)value) ? "да" : "нет";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is string && (string)value == "да";
    }
}
