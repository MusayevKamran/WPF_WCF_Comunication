using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.ServiceModel;
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
using System.Windows.Threading;
using WpfClient.Properties;
using WpfClient.WcfService;
using Line = WpfClient.WcfService.Line;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Line _line;
        private ClientService _clientService;

        // private readonly Line[] _lines = Enum.GetValues(typeof(Line)).Cast<Line>().ToArray();



        public MainWindow()
        {
            this.InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            _clientService = new ClientService(this);
            _clientService.SubscribeForLine();

            var lineNumber = Settings.Default.LineId;
            if (string.IsNullOrEmpty(lineNumber))
            {
                var popup = new SelectLinePopup(_clientService);
            
                popup.ShowDialog();
            
                lineNumber = Settings.Default.LineId;
            }

            _line = _clientService.GetSelectedLine(lineNumber);

            this.DataContext = _line;

            this.Closing += (sender, args) =>
            {
                try { }
                finally
                {
                    _clientService.UnsubscribeForLine();
                }
            };
        }



        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F8)
            {
                var popup = new SelectLinePopup(_clientService);
            
                popup.ShowDialog();
            
                var lineNumber = Settings.Default.LineId;
            
                _line = _clientService.GetSelectedLine(lineNumber);
            
                this.DataContext = _line;
            }
        }
    }
}
