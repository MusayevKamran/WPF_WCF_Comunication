using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using WpfClient.Properties;
using WpfClient.WcfService;


namespace WpfClient
{
    /// <summary>
    /// Interaction logic for SelectLinePopup.xaml
    /// </summary>
    public partial class SelectLinePopup : Window
    {
        private readonly ClientService _clientService;
        public SelectLinePopup(ClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var lines = _clientService.GetLinesId();
            
            if (sender is ComboBox comboBox)
            {
                comboBox.ItemsSource = lines;
                var selectedIndex = lines.ToList().IndexOf(short.Parse(Settings.Default.LineId));
                comboBox.SelectedIndex = selectedIndex;
            }
        }
        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedIdem = sender as ComboBox;
            var line = selectedIdem?.SelectedItem;

            if (!string.IsNullOrEmpty(line?.ToString()))
            {
                Settings.Default.LineId = line.ToString();
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            this.Close();
        }
    }
}
